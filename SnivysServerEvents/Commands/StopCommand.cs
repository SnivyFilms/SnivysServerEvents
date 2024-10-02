using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace SnivysServerEvents.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class StopCommand : ICommand
    {
        public string Command { get; set; } = "Stop";
        public string[] Aliases { get; set; } = ["Terminate", "End"];
        public string Description { get; set; } = "Stops all events";
        
        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("vvevents.stop"))
            {
                response = "You do not have the required permission to use this command";
                return false;
            }
            
            response = "Stopping all events";
            Log.Debug($"{sender} has stopped all events");
            EventHandlers.EventHandlers.StopEventsCommand();
            return true;
        }
    }
}