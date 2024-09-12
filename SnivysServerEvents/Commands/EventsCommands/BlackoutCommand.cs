using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SnivysServerEvents.EventHandlers;

namespace SnivysServerEvents.Commands.EventsCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class BlackoutCommand : ICommand
    {
        public string Command { get; set; } = "Blackout";
        public string[] Aliases { get; set; } = ["LightsOut"];
        public string Description { get; set; } = "Starts the Blackout Event";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("vvevents.run"))
            {
                response = "You do not have the required permission to use this command";
                return false;
            }
            var blackoutEventHandlers = new BlackoutEventHandlers();
            response = "Starting Blackout Event";
            return true;
        }
    }
}