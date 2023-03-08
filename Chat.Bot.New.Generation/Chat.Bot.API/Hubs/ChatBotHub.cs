using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Bot.API.Hubs
{
    public class ChatBotHub : Hub
    {
        private readonly ILoggerAdapter<ChatBotHub> _loggerAdapter;

        public ChatBotHub(ILoggerAdapter<ChatBotHub> logger)
        {
            _loggerAdapter = logger;
        }


        public async Task SendMessage(string user, string message)
        {
            _loggerAdapter.LogInformation($"Message: {message}");
            await Clients.All.SendAsync("Message", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            _loggerAdapter.LogInformation(Context.ConnectionId);
            if (!Context.User.Identity.IsAuthenticated)
            {
                // possible send a message back to the client (and show the result to the user)
                //this.Clients.SendUnauthenticatedMessage("You don't have the correct permissions for this action.");
                return;
            }

            await base.OnConnectedAsync();
        }
    }
}
