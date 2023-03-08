using Domain.Core.SqlServer;
using Infra.Data.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.SqlServer.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected ChatBotContext Db;
        protected DbSet<TEntity> DbSet;

        protected Repository(ChatBotContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public virtual Task<List<TEntity>> GetAll()
        {
            return DbSet.ToListAsync();
        }

        public Task<int> SaveChanges()
        {
            return Db.SaveChangesAsync();
        }
    }
}
