using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SnivysServerEvents.EventHandlers;

namespace SnivysServerEvents.Commands.EventsCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class PeanutInfectionCommand : ICommand
    {
        public string Command { get; set; } = "173Infection";
        public string[] Aliases { get; set; } = ["PeanutInfection", "Infection"];
        public string Description { get; set; } = "Starts the 173 Infection";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("vvevents.run"))
            {
                response = "You do not have the required permission to use this command";
                return false;
            }
            var infectionEventHandlers = new PeanutInfectionEventHandlers();
            response = "Starting Peanut Infection Event";
            return true;
        }
    }
}