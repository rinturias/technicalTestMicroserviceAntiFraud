using Microsoft.EntityFrameworkCore;
using Yape.AntiFraud.Domain.Entities;
using Yape.AntiFraud.Domain.Enums;
using Yape.AntiFraud.Domain.Interfaces.Repository;
using Yape.AntiFraud.Infrastructure.Context;

namespace Yape.AntiFraud.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IContextDatabase _context;
        public TransactionRepository(IContextDatabase contextDatabase)
        {
            this._context = contextDatabase;

        }
        public async Task AddAsync(Transactio Ent)
        {

            await _context.Transactions.AddAsync(Ent);

        }
        public async Task<decimal> GetDailyTransactionTotalAsync(Guid sourceAccountId)
        {
            var totalAmount = await _context.Transactions
                .AsNoTracking()
                .Where(x => x.Status != TransactionStatus.Rejected &&
                            x.SourceAccountId == sourceAccountId &&
                            x.CreatedAt.Date == DateTime.Today)
                .SumAsync(x => x.Amount);

            return totalAmount;
        }
    }
}
