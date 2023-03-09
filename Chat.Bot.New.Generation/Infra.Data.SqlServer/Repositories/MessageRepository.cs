using Domain.Chats;
using Domain.Repositories;
using Infra.Data.SqlServer.Contexts;

namespace Infra.Data.SqlServer.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ChatBotContext context) : base(context)
        {
        }
    }
}
