using Domain.Chats;
using Domain.Core.SqlServer;

namespace Domain.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesFiltered(Filter filter);
        Task AddMessage(Message message);
    }
}
