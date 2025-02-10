using Yape.AntiFraud.Application.DTO;

namespace Yape.AntiFraud.Application.Interfaces
{
    public interface ITransactionProcessor
    {
        Task ProcessTransactionAsync(TransactionDto transaction);
    }
}
