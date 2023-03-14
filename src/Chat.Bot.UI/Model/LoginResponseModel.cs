namespace Chat.Bot.UI.Model
{
    public class LoginResponseModel
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
    }
}
