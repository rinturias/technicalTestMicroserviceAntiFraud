using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Yape.AntiFraud.Domain.Enums;


namespace Yape.AntiFraud.Domain.Entities
{
    [Table("transactions")]
    public class Transactio
    {
        [Key]
        [Column("transactionid")]
        public Guid TransactionId { get; set; }

        [Required]
        [Column("sourceaccountid")]
        public Guid SourceAccountId { get; set; }

        [Required]
        [Column("targetaccountid")]
        public Guid TargetAccountId { get; set; }

        [Required]
        [Column("transfertypeid")]
        public int TransferTypeId { get; set; }

        [Required]
        [Column("amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(10)]
        public string Status { get; set; } = TransactionStatus.Pending;

        [Required]
        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}