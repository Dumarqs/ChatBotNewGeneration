using Chat.Bot.Bot.Enums;

namespace Chat.Bot.Bot.Models
{
    public class Command
    {
        public bool IsValid { get; set; }
        public CommandEnum Name { get; set; }
        public string Value { get; set; }
    }
}
