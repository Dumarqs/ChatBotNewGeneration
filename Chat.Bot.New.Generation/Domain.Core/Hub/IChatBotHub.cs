namespace Domain.Core.Hub
{
    public interface IChatBotHub
    {
        Task SendMessage(string message);
    }
}
