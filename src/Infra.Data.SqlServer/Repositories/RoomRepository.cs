using Domain.Chats;
using Domain.Repositories;
using Infra.Data.SqlServer.Contexts;

namespace Infra.Data.SqlServer.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(ChatBotContext context): base(context)
        {
        }
    }
}
