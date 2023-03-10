using Domain.Core.SqlServer;
using Infra.Data.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

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

        public async virtual Task<IEnumerable<TEntity>> GetFiltered(Filter filter)
        {
            if (!String.IsNullOrEmpty(filter.Search))
                return await DbSet.AsNoTracking().Where(filter.Field, filter.Search).Skip(filter.Skip).Take(filter.Take).ToListAsync();
            else
                return await DbSet.AsNoTracking().Skip(filter.Skip).Take(filter.Take).ToListAsync();
        }


        public async Task<int> SaveChanges() => await Db.SaveChangesAsync();

        public async virtual Task Add(TEntity obj) => await DbSet.AddAsync(obj);
    }
}
