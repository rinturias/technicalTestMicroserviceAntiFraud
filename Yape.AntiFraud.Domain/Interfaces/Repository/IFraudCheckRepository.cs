using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.AntiFraud.Domain.Entities;

namespace Yape.AntiFraud.Domain.Interfaces.Repository
{
    public interface IFraudCheckRepository
    {
        Task AddAsync(FraudCheck Ent);
    }
}
