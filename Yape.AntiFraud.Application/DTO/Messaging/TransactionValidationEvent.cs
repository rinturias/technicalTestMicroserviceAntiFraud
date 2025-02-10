using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.AntiFraud.Application.DTO.Messaging
{
    public class TransactionValidationEvent
    {
        public Guid TransactionId { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
    }
}
