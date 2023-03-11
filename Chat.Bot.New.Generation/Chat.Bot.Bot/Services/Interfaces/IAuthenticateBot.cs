namespace Chat.Bot.Bot.Services.Interfaces
{
    public interface IAuthenticateBot
    {
        Task<string> AuthenticateBotAsync(); 
    }
}
