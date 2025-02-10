using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Yape.AntiFraud.Application.DTO;

namespace Yape.AntiFraud.Infrastructure.Messaging
{
    public class KafkaConsumer : BackgroundService
    {
       
     
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly string _inputTopic;
        private readonly string _bootstrapServers;
        private readonly string _groupId;
        private readonly AutoOffsetReset _autoOffsetReset;

        public KafkaConsumer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _bootstrapServers = _configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
            _groupId = _configuration["Kafka:GroupId"] ?? "default-group";
            _inputTopic = _configuration["Kafka:InputTopic"] ?? "default-topic";
            _autoOffsetReset = Enum.TryParse(_configuration["Kafka:AutoOffsetReset"], out AutoOffsetReset offsetReset) ? offsetReset : AutoOffsetReset.Latest;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                SecurityProtocol = SecurityProtocol.Plaintext,
                GroupId = _groupId,
                AutoOffsetReset = _autoOffsetReset
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
                await Task.Yield();
            }
        }
    }
}