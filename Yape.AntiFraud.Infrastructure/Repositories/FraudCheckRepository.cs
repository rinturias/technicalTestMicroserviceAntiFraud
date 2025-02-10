using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.AntiFraud.Domain.Entities;
using Yape.AntiFraud.Domain.Interfaces.Repository;
using Yape.AntiFraud.Infrastructure.Context;

namespace Yape.AntiFraud.Infrastructure.Repositories
{
    public class FraudCheckRepository : IFraudCheckRepository
    {

        private readonly IContextDatabase _context;
        public FraudCheckRepository(IContextDatabase contextDatabase)
        {
            this._context = contextDatabase;

        }

        public async Task AddAsync(FraudCheck Ent)
        {
            await _context.FraudChecks.AddAsync(Ent);
        }
    }
}
