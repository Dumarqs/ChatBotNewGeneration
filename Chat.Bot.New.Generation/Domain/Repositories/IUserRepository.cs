using Domain.Chats;
using Domain.Core.SqlServer;

namespace Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> Authenticate(string email);
        Task<User> GetById(Guid id);
    }
}
