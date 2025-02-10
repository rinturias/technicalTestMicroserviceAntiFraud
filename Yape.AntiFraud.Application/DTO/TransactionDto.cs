namespace Yape.AntiFraud.Application.DTO
{
    public class TransactionDto
    {
        public Guid TransactionId { get; set; }
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
