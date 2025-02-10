using System.ComponentModel.DataAnnotations;

namespace Yape.AntiFraud.Domain.Entities
{
    public class FraudCheck
    {
        [Key]
        public Guid FraudCheckId { get; set; } = Guid.NewGuid();
        public Guid TransactionId { get; set; }
        public Guid SourceAccountId { get; set; }
        public string Status { get; set; } = "P";
        public string? Reason { get; set; }
        public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    }
}
