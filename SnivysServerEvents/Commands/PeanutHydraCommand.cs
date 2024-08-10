using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SnivysServerEvents.Events;

namespace SnivysServerEvents.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class PeanutHydraCommand : ICommand
    {
        public string Command { get; set; } = "VVE_SCP-173_Hydra";
        public string[] Aliases { get; set; } = Array.Empty<string>();
        public string Description { get; set; } = "Starts the 173 Hydra";
        public bool SanitizeResponse { get; set; } = true;

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("vvevents.run"))
            {
                response = "You do not have the required permission to use this command";
                return false;
            }
            var hydraEventHandlers = new PeanutHydraEventHandlers();
            response = "Starting Peanut Hydra Event";
            return true;
        }
    }
}