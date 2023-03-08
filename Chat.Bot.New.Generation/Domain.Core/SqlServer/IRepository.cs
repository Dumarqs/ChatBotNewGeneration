namespace Domain.Core.SqlServer
{
    public interface IRepository<TEntity> : IDisposable 
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<int> SaveChanges();
    }

}
