using Chat.Bot.Bot.Enums;
using Chat.Bot.Bot.Extensions;
using Chat.Bot.Bot.Models;
using Chat.Bot.Bot.Services.Interfaces;

namespace Chat.Bot.Bot.Services
{
    public class CommandValidation : ICommandValidation
    {
        public Command IsValidCommand(string command)
        {
            var commandValidate = new Command();

            if (command.StartsWith(CommandEnum.Stock.ToDescriptionString()))
                commandValidate.IsValid = true;
            else
            {
                commandValidate.IsValid = false;
                commandValidate.Value= command;
                return commandValidate;
            }

            commandValidate.Name = CommandEnum.Stock;
            commandValidate.Value = command.Substring(command.IndexOf(@"=") + 1);

            return commandValidate;
        }
    }
}
