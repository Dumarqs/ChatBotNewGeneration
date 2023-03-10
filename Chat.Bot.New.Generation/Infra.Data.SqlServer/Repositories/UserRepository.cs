using Domain.Chats;
using Domain.Repositories;
using Infra.Data.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.SqlServer.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ChatBotContext _context;

        public UserRepository(ChatBotContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> Authenticate(string email) => await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
    }
}
