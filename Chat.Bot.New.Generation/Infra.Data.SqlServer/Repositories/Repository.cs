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

        public void Dispose() => Db.Dispose();


        public async virtual Task<IEnumerable<TEntity>> GetAll() => await DbSet.AsNoTracking().ToListAsync();

        public async Task<int> SaveChanges() => await Db.SaveChangesAsync();
    }
}
