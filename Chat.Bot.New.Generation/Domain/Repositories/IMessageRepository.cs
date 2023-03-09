using Domain.Chats;
using Domain.Core.SqlServer;

namespace Domain.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
    }
}
