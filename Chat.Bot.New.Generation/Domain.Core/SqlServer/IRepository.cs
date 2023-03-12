namespace Domain.Core.SqlServer
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetFiltered(Filter filter);
        Task<IEnumerable<TEntity>> GetAll();
        Task<int> SaveChanges();
        Task Add(TEntity obj);
        void Update(TEntity obj);
    }

}
