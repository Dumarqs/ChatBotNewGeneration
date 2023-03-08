using Domain.Dtos;

namespace Application.Services.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetAllRoom();
    }
}
