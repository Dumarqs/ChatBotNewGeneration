using Chat.Bot.UI.Model;

namespace Chat.Bot.UI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseModel> Login(LoginModel loginModel);
        Task Logout();
        Task<ResponseModel> Register(RegisterModel registerModel);
    }
}
