using Confluent.Kafka;
using System.Text.Json;
using System.Transactions;
using Yape.AntiFraud.Application.DTO;
using Yape.AntiFraud.Application.DTO.Messaging;
using Yape.AntiFraud.Application.Interfaces;
using Yape.AntiFraud.Domain.Entities;
using Yape.AntiFraud.Domain.Interfaces;
using Yape.AntiFraud.Domain.Interfaces.Repository;

namespace Yape.AntiFraud.Application.UseCases.Transactions
{
    public class AntiFraudService : Interfaces.ITransactionProcessor
    {
        private readonly IAntiFraudService _antiFraudService;
        private readonly IProducer<string, string> _producer;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionRepository _iTransactionRepository;
        private readonly IFraudCheckRepository _fraudCheckRepository;
        private readonly string _outputTopic = "fraud-validated";
        const int _limitAmount = 2000;
        const int _limitTransactionAccumulatedDay = 20000;
        public AntiFraudService(IUnitOfWork unitOfWork, IProducer<string, string> producer,ITransactionRepository transactionRepository, IFraudCheckRepository fraudCheckRepository)
        {
            _unitOfWork = unitOfWork;
            _producer = producer;
            _iTransactionRepository = transactionRepository;
            _fraudCheckRepository= fraudCheckRepository;
        }
        public async Task ProcessTransactionAsync(TransactionDto transaction)
        {
            try
            {             
                _unitOfWork.BeginTransaction();
             
                 var transactionAccumulated = await _iTransactionRepository.GetDailyTransactionTotalAsync(transaction.SourceAccountId);
                 var statusTransactionAccumulate =   (transactionAccumulated + transaction.Amount) > _limitTransactionAccumulatedDay ? Domain.Enums.TransactionStatus.Rejected: Domain.Enums.TransactionStatus.Approved;

                var domainTransaction = new Transactio
                {
                    TransactionId = transaction.TransactionId,
                    SourceAccountId = transaction.SourceAccountId,
                    TargetAccountId = transaction.TargetAccountId,
                    TransferTypeId = transaction.TransferTypeId,
                    Amount = transaction.Amount,
                    CreatedAt = transaction.CreatedAt,
                    Status = statusTransactionAccumulate
                };

                await _iTransactionRepository.AddAsync(domainTransaction);
                await _unitOfWork.SaveChangesAsync(); 

                var objFraudCheck = new FraudCheck
                {
                    TransactionId = transaction.TransactionId,
                    SourceAccountId = transaction.SourceAccountId,
                    Status = statusTransactionAccumulate,
                };

                if (transaction.Amount > _limitAmount && statusTransactionAccumulate == Domain.Enums.TransactionStatus.Approved)
                {
                    domainTransaction.Status = Domain.Enums.TransactionStatus.Rejected;
                    objFraudCheck.Status = Domain.Enums.TransactionStatus.Rejected;
                    objFraudCheck.Reason = "El monto de la transacción excede el límite.";
                }


                await _fraudCheckRepository.AddAsync(objFraudCheck);
                await _unitOfWork.SaveChangesAsync();



                var messageTransactionValidation = new TransactionValidationEvent
                {
                    TransactionId = transaction.TransactionId,
                    Reason = objFraudCheck.Reason ?? "Monto de transacción permitido.",
                    Status = objFraudCheck.Status
                };

                var dataMessaje = JsonSerializer.Serialize(messageTransactionValidation);
                _producer.Produce("TransactionValidation", new Message<string, string> { Key = transaction.TransactionId.ToString(), Value = dataMessaje });

                Console.WriteLine($"Transacción procesada y enviada a la cola: {transaction.TransactionId}");
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar la transacción {transaction.TransactionId}: {ex.Message}");
                HandleException("COD501", "error");
            }
        }
        private ResultService HandleException(string errorCode, string message, string error = null)
        {

            _unitOfWork.Rollback();
            return new ResultService
            {
                Success = false,
                CodError = errorCode,
                Messaje = message,
                Error = error
            };
        }

    }
}