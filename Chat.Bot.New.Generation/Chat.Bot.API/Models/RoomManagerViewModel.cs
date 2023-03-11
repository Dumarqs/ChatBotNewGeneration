using Chat.Bot.API.ViewModels;

namespace Chat.Bot.API.Models
{
    public class RoomManagerViewModel
    {
        public Guid RoomId { get; set; }
        public IList<UsersConnectedViewModel> UserConnected { get; set; }
    }
}
