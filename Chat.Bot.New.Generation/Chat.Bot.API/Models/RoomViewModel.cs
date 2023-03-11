using System.ComponentModel.DataAnnotations;

namespace Chat.Bot.API.Models
{
    public class RoomViewModel
    {
        [Key]
        public Guid RoomId { get; set; }

        [Required(ErrorMessage = "Fill the field Room Name")]
        [MinLength(5)]
        public string RoomName { get; set; }
    }
}
