namespace Chat.Bot.API.ViewModels
{
    public class RoomManagerViewModel
    {
        public Guid RoomId { get; set; }
        public IList<UsersConnectedViewModel> UserConnected { get; set; }
    }
}
