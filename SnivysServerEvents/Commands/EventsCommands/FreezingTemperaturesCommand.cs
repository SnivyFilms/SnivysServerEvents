using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SnivysServerEvents.EventHandlers;

namespace SnivysServerEvents.Commands.EventsCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class FreezingTemperaturesCommand : ICommand
    {
        public string Command { get; set; } = "FreezingTemps";
        public string[] Aliases { get; set; } = ["Cold", "Freezing"];
        public string Description { get; set; } = "Starts the Freezing Temperature Event";
        public bool SanitizeResponse { get; set; } = false;

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("vvevents.run"))
            {
                response = "You do not have the required permission to use this command";
                return false;
            }
            var freezingTemperaturesHandlers = new FreezingTemperaturesEventHandlers();
            response = "Starting Freezing Temperature Event";
            return true;
        }
    }
}