using Microsoft.Extensions.DependencyInjection;
using Yape.AntiFraud.Application.Interfaces;
using Yape.AntiFraud.Application.UseCases.Transactions;

namespace Yape.AntiFraud.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITransactionProcessor, AntiFraudService>();

            return services;
        }
    }
}
