using Domain.Chats;
using Domain.Repositories;
using Infra.Data.SqlServer.Contexts;

namespace Infra.Data.SqlServer.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ChatBotContext context) : base(context)
        {
        }
    }
}
