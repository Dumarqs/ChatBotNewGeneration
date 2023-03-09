using Domain.Core.SqlServer;
using Domain.Dtos;

namespace Application.Services.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetAllRoom();
        Task<IEnumerable<RoomDto>> GetRoomFiltered(Filter filter);
        Task AddRoom(RoomDto roomDto);
    }
}
