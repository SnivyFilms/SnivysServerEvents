using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SnivysServerEvents.EventHandlers;

namespace SnivysServerEvents.Commands.EventsCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ChaoticCommand : ICommand
    {
        public string Command { get; set; } = "Chaotic";
        public string[] Aliases { get; set; } = Array.Empty<string>();
        public string Description { get; set; } = "Starts the Chaotic Event";
        public bool SanitizeResponse { get; set; } = false;

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("vvevents.run"))
            {
                response = "You do not have the required permission to use this command";
                return false;
            }
            var chaoticHandlers = new ChaoticEventHandlers();
            response = "Starting Chaotic Event";
            return true;
        }
    }
}