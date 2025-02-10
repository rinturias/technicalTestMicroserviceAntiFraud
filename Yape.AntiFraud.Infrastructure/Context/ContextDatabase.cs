using Microsoft.EntityFrameworkCore;
using Yape.AntiFraud.Domain.Entities;

namespace Yape.AntiFraud.Infrastructure.Context
{
    public class ContextDatabase : DbContext, IContextDatabase
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) : base(options)
        {

        }
        public DbSet<Transactio> Transactions { get; set; }
        public DbSet<FraudCheck> FraudChecks { get; set; }
        public DbContext GetInstance() => this;
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}