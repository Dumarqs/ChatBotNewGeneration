using Domain.Core.Hub;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Bot.API.Hubs
{
    public class ChatBotHub : Hub<IChatBotHub>
    {
        private readonly ILoggerAdapter<ChatBotHub> _loggerAdapter;

        public ChatBotHub(ILoggerAdapter<ChatBotHub> logger)
        {
            _loggerAdapter= logger;
        }

        public async Task SendMessage(string message)
        {
            _loggerAdapter.LogInformation($"Message: {message}");
            await Clients.All.SendMessage(message);
        }
    }
}
