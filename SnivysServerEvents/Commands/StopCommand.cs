using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SnivysServerEvents.Commands.EventsCommands;

namespace SnivysServerEvents.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class StopCommand : ICommand
    {
        public string Command { get; set; } = "Stop";
        public string[] Aliases { get; set; } = ["Kill", "Terminate", "End"];
        public string Description { get; set; } = "Stops all events";
        public bool SanitizeResponse { get; set; } = false;
        
        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("vvevents.stop"))
            {
                response = "You do not have the required permission to use this command";
                return false;
            }
            
            response = "Stopping all events";
            EventHandlers.EventHandlers.StopEventsCommand();
            return true;
        }
    }
}