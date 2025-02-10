using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.AntiFraud.Infrastructure.Messaging
{
    public static class KafkaProducer
    {
        public static void AddKafkaProducer(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                SecurityProtocol = SecurityProtocol.Plaintext

            };

            services.AddSingleton<IProducer<string, string>>(new ProducerBuilder<string, string>(config).Build());
        }
    }
}