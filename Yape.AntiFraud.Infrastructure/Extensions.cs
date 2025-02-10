using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yape.AntiFraud.Domain.Interfaces;
using Yape.AntiFraud.Domain.Interfaces.Repository;
using Yape.AntiFraud.Infrastructure.Context;
using Yape.AntiFraud.Infrastructure.Messaging;
using Yape.AntiFraud.Infrastructure.Repositories;
using Yape.AntiFraud.Infrastructure.UnitOfWorks;

namespace Yape.AntiFraud.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ContextDatabase>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddHostedService<KafkaConsumer>();
            services.AddKafkaProducer(configuration);
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped< IFraudCheckRepository, FraudCheckRepository> ();
            services.AddScoped<IContextDatabase, ContextDatabase>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            return services;
        }
    }
}
