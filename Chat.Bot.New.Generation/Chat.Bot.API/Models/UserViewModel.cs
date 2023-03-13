using System.ComponentModel.DataAnnotations;

namespace Chat.Bot.API.Models
{
    public class UserViewModel
    {
        [Key]
        public Guid UserId { get; set; }
        [Required]

        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ConnectionId { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
