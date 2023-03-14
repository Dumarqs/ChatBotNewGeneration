using Domain.Chats;
using Domain.Core.SqlServer;
using Domain.Repositories;
using Infra.Data.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Infra.Data.SqlServer.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly ChatBotContext _context;
        public MessageRepository(ChatBotContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetMessagesFiltered(Filter filter) => await _context.Messages.AsNoTracking()
                                                                        .Where(filter.Field, filter.Search).Skip(filter.Skip).Take(filter.Take)
                                                                        .Include(u => u.User).ToListAsync();

        public async Task AddMessage(Message message)
        {
            await _context.Set<Message>().AddAsync(message);

            _context.Entry(message.User).State = EntityState.Unchanged;
            await _context.SaveChangesAsync();
        }
    }
}
