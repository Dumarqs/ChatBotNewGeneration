using Application.Services.Interfaces;
using AutoMapper;
using Chat.Bot.API.ViewModels;
using Domain.Dtos;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Bot.API.Hubs
{
    public class ChatBotHub : Hub
    {
        private readonly ILoggerAdapter<ChatBotHub> _loggerAdapter;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private IList<RoomManagerViewModel> _rooms = new List<RoomManagerViewModel>();
        private IList<string> _bots = new List<string>();

        public ChatBotHub(ILoggerAdapter<ChatBotHub> logger, IMessageService messageService, IMapper mapper)
        {
            _loggerAdapter = logger;
            _messageService = messageService;
            _mapper = mapper;
        }


        public async Task SendMessage(MessageViewModel messageViewModel)
        {
            _loggerAdapter.LogInformation($"Message: {messageViewModel.Text}");

            await RoomExists(messageViewModel.RoomId);
            await Clients.Group(messageViewModel.RoomId.ToString()).SendAsync("Message", messageViewModel);

            if (!messageViewModel.IsCommand())
            {
                var messageDto = _mapper.Map<MessageDto>(messageViewModel);
                //await _messageService.SaveMessage(messageDto);
            }
        }

        public override async Task OnConnectedAsync()
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                // possible send a message back to the client (and show the result to the user)
                //this.Clients.SendUnauthenticatedMessage("You don't have the correct permissions for this action.");
                return;
            }
            else
            {
                if(Context.User.Identity.Name == "BOT")
                {
                    _bots.Add(Context.ConnectionId);
                }

                await base.OnConnectedAsync();
            }            
        }

        private async Task RoomExists(Guid roomName)
        {
            var room = _rooms.First(f => f.RoomId == roomName);
            if (room == null)
            {
                room = new RoomManagerViewModel
                {
                    RoomId = roomName,
                    UserConnected = new List<UsersConnectedViewModel>()
                };

                _rooms.Add(room);
                await AddBotsToGroup(room);
            }
            await TryAddToGroup(room);
        }

        private async Task TryAddToGroup(RoomManagerViewModel room)
        {
            var user = room.UserConnected.First(f => f.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                room.UserConnected.Add(user);

                await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomId.ToString());
            }
        }

        private async Task AddBotsToGroup(RoomManagerViewModel room)
        {
            foreach(var bot in _bots)
            {
                await Groups.AddToGroupAsync(bot, room.RoomId.ToString());
            }
        }
    }
}
