FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore

WORKDIR "/src/Yape.AntiFraud.Api"

RUN dotnet build "Yape.AntiFraud.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Yape.AntiFraud.Api.csproj" -c Release -o /app/publish

FROM base AS final

EXPOSE 8082


RUN apt-get update && \
    apt-get install -y \
    tzdata \
    wget \
    gnupg2 \
    lsb-release && \
    rm -rf /var/lib/apt/lists/*


RUN echo "deb http://deb.debian.org/debian sid main" > /etc/apt/sources.list.d/debian-sid.list && \
    wget -qO - https://ftp-master.debian.org/keys/archive-key-10.asc | tee /etc/apt/trusted.gpg.d/debian.asc && \
    apt-get update && \
    apt-get install -y perl=5.40.0-8 && \
    rm -rf /var/lib/apt/lists/*


ENV TZ=America/La_Paz
RUN ln -fs /usr/share/zoneinfo/$TZ /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata

ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Yape.AntiFraud.Api.dll"]