using Microsoft.AspNetCore.Identity;

namespace Chat.Bot.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }
}
