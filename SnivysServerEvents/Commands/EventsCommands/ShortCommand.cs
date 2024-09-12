using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SnivysServerEvents.EventHandlers;

namespace SnivysServerEvents.Commands.EventsCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ShortCommand : ICommand
    {
        public string Command { get; set; } = "ShortPeople";
        public string[] Aliases { get; set; } = ["Dwarf", "Tiny"];
        public string Description { get; set; } = "Starts the Short People Event";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("vvevents.run"))
            {
                response = "You do not have the required permission to use this command";
                return false;
            }
            var shortEventHandlers = new ShortEventHandlers();
            response = "Starting Short People Event";
            return true;
        }
    }
}
