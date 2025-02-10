namespace Yape.AntiFraud.Domain.Interfaces.Repository
{
    public interface ITransactionProcessor
    {
        Task ProcessTransactionAsync(dynamic transaction);
    }
}
