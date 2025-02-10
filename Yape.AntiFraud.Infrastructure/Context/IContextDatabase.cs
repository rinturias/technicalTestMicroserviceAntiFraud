using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Yape.AntiFraud.Domain.Entities;

namespace Yape.AntiFraud.Infrastructure.Context
{
    public interface IContextDatabase
    {
        DbSet<Transactio> Transactions { get; set; }
        DbSet<FraudCheck> FraudChecks { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DatabaseFacade Database { get; }
    }
}
