namespace Chat.Bot.Consumer.Services.Interfaces
{
    public interface IAuthenticateConsumer
    {
        Task<string> AuthenticateConsumerAsync(string userConsumer);
    }
}
