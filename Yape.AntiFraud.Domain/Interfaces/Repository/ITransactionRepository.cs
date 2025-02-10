using Yape.AntiFraud.Domain.Entities;

namespace Yape.AntiFraud.Domain.Interfaces.Repository
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transactio Ent);
        Task<decimal> GetDailyTransactionTotalAsync(Guid sourceAccountId);
    }
}
