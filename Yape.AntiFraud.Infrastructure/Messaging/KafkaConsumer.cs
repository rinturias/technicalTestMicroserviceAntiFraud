using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Yape.AntiFraud.Application.DTO;

namespace Yape.AntiFraud.Infrastructure.Messaging
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _inputTopic = "transactions";
        private readonly string _bootstrapServers = "192.168.100.30:9094";

        public KafkaConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                SecurityProtocol = SecurityProtocol.Plaintext,
                GroupId = "anti-fraud-service",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_inputTopic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    var transaction = JsonConvert.DeserializeObject<TransactionDto>(consumeResult.Message.Value);

                    if (transaction != null)
                    {

                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var transactionProcessor = scope.ServiceProvider.GetRequiredService<Application.Interfaces.ITransactionProcessor>();
                            await transactionProcessor.ProcessTransactionAsync(transaction);
                        }
                    }
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Error al consumir el mensaje de Kafka: {ex.Error.Reason}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado en KafkaConsumer: {ex.Message}");
                }
            }
        }
    }
}