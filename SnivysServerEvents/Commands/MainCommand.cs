using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using Exiled.API.Features;

namespace SnivysServerEvents.Commands
{
        [CommandHandler(typeof(RemoteAdminCommandHandler))]
        [CommandHandler(typeof(GameConsoleCommandHandler))]
        public class MainCommand : ParentCommand
        {
                public override string Command { get; } = "VVE";
                public override string Description { get; } = "Main command for Snivy's Server Events";
                public override string[] Aliases { get; } = Array.Empty<string>();
                
                public override void LoadGeneratedCommands()
                {
                        try
                        {
                                RegisterCommand(new BlackoutCommand());
                                RegisterCommand(new FreezingTemperaturesCommand());
                                RegisterCommand(new PeanutHydraCommand());
                                RegisterCommand(new PeanutInfectionCommand());
                                RegisterCommand(new ShortCommand());
                                RegisterCommand(new VariableLightCommand());
                        }
                        catch (Exception e)
                        {
                                Log.Warn("An exception was caught while registering commands.");
                                Log.Debug($"{e}");
                        }
                }

                public MainCommand() => LoadGeneratedCommands();
                protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
                {
                        response = "Please enter a valid event to run:";
                        foreach (var x in this.Commands)
                        {
                                string args = "";
                                if (x.Value is IUsageProvider usage)
                                {
                                        foreach (string arg in usage.Usage)
                                        {
                                                args += $"[{arg}] ";
                                        }
                                }
                        }

                        return false;
                }
        }
}