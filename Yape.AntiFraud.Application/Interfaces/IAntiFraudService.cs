using Yape.AntiFraud.Application.DTO;

namespace Yape.AntiFraud.Application.Interfaces
{
    public interface IAntiFraudService
    {
        Task<string> ValidateTransactionAsync(TransactionDto transaction);

    }
}
