using Chat.Bot.Bot.Models;

namespace Chat.Bot.Bot.Services.Interfaces
{
    public interface ICommandValidation
    {
        Command IsValidCommand(string command);
    }
}
