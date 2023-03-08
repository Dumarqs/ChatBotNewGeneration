namespace Domain.Core.SqlServer
{
    public interface IRepository<TEntity> : IDisposable 
    {
        Task<List<TEntity>> GetAll();
        Task<int> SaveChanges();
    }

}
