using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SnivysServerEvents.EventHandlers;

namespace SnivysServerEvents.Commands.EventsCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class VariableLightCommand : ICommand
    {
        public string Command { get; set; } = "VariableLights";
        public string[] Aliases { get; set; } = ["RandomLights", "ColorfulLights"];
        public string Description { get; set; } = "Starts the Variable Lights Event. (PHOTOSENSITIVITY WARNING!)";
        public bool SanitizeResponse { get; set; } = false;

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("vvevents.run"))
            {
                response = "You do not have the required permission to use this command";
                return false;
            }
            var variableEventHandlers = new VariableLightsEventHandlers();
            response = "Starting Variable Lights Event.";
            return true;
        }
    }
}