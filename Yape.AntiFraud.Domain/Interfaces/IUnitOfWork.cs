namespace Yape.AntiFraud.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        Task SaveChangesAsync();
    }
}
