using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SnivysServerEvents.EventHandlers;

namespace SnivysServerEvents.Commands.EventsCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class NameRedactedCommand : ICommand
    {
        public string Command { get; set; } = "NameRedacted";
        public string[] Aliases { get; set; } = [];
        public string Description { get; set; } = "Removes player's nicknames and sets them to something else";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("vvevents.run"))
            {
                response = "You do not have the required permission to use this command";
                return false;
            }
            var nameRedactedHandler = new NameRedactedEventHandlers();
            response = "Starting Name Redacted Event";
            return true;
        }
    }
}