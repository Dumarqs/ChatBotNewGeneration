using Domain.Dtos;

namespace Application.Services.Interfaces
{
    public interface IMessageService
    {
        Task SaveMessage(MessageDto roomDto);
        Task<IEnumerable<MessageDto>> GetLastMessages(int messageQty, string roomId);
    }
}
