using Domain.Dtos;

namespace Application.Services.Interfaces
{
    public interface IBotService
    {
        Task SendMessageAsync(MessageDto message, string uri);
    }
}
