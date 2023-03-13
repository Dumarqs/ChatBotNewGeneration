using Application.Services.Interfaces;
using AutoMapper;
using Chat.Bot.API.Models;
using Domain.Core.SqlServer;
using Domain.Dtos;
using Infra.CrossCutting.Log.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Chat.Bot.API.Hubs
{
    [Authorize]
    public class ChatBotHub : Hub
    {
        private readonly ILoggerAdapter<ChatBotHub> _loggerAdapter;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public ChatBotHub(ILoggerAdapter<ChatBotHub> logger, IMessageService messageService, IMapper mapper,
                                        IUserService userService, IRoomService roomService)
        {
            _loggerAdapter = logger;
            _messageService = messageService;
            _mapper = mapper;
            _userService = userService;
            _roomService = roomService;
        }


        public async Task SendMessage(MessageViewModel messageViewModel)
        {
            _loggerAdapter.LogInformation($"Message: {messageViewModel.Text} by User: {messageViewModel.User.UserId}");

            messageViewModel.User.ConnectionId = Context.ConnectionId;
            await Clients.Group(messageViewModel.RoomId.ToString()).SendAsync("Message", messageViewModel);

            if (!messageViewModel.IsCommand())
            {
                var messageDto = _mapper.Map<MessageDto>(messageViewModel);
                await _messageService.SaveMessage(messageDto);
            }
        }

        public override async Task OnConnectedAsync()
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                await Clients.User(Context.ConnectionId).SendAsync("You don't have the correct permissions for this action.");
                return;
            }
            else
            {
                if (Context.User.IsInRole("Bot"))
                {
                    var rooms = await _roomService.GetAllRoom();
                    foreach(var room in rooms)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomId.ToString());
                    }

                    await UpdateConnectionId();
                    _loggerAdapter.LogInformation($"Bot {Context.ConnectionId} is connected.");
                }
                else if (Context.User.IsInRole("User"))
                {
                    await UpdateConnectionId();
                }
                else
                    throw new Exception("Role not recognized.");

                await base.OnConnectedAsync();
            }            
        }

        public async Task AddBotsToNewGroup(RoomViewModel room)
        {
            var filter = new Filter { Field = "Role = @0", Search = "Bot" };
            var bots = await _userService.GetUserFiltered(filter);

            foreach(var bot in bots)
                if(!string.IsNullOrEmpty(bot.ConnectionId))
                    await Groups.AddToGroupAsync(bot.ConnectionId, room.RoomId.ToString());             
        }

        public async Task AddUserToGroups(IList<RoomViewModel> rooms)
        {
            foreach (var room in rooms)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomId.ToString());
            }   
        }

        private async Task UpdateConnectionId()
        {
            var user = await _userService.GetUser(new Guid(Context.User.Claims.First(f => f.Type == ClaimTypes.NameIdentifier).Value));
            user.ConnectionId = Context.ConnectionId;

            await _userService.UpdateUser(user);
        }
    }
}
