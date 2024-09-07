﻿using System;
using CommandSystem;
using Exiled.API.Features;
using SnivysServerEvents.Commands.EventsCommands;

namespace SnivysServerEvents.Commands
{
        [CommandHandler(typeof(RemoteAdminCommandHandler))]
        [CommandHandler(typeof(GameConsoleCommandHandler))]
        public class MainCommand : ParentCommand
        {
                public override string Command { get; } = "VVE";
                public override string Description { get; } = "Main command for Snivy's Server Events";
                public override string[] Aliases { get; } = ["SSE", "SnivysServerEvents"];
                
                public override void LoadGeneratedCommands()
                {
                        try
                        {
                                RegisterCommand(new BlackoutCommand());
                                RegisterCommand(new ChaoticCommand());
                                RegisterCommand(new FreezingTemperaturesCommand());
                                RegisterCommand(new NameRedactedCommand());
                                RegisterCommand(new PeanutHydraCommand());
                                RegisterCommand(new PeanutInfectionCommand());
                                RegisterCommand(new ShortCommand());
                                RegisterCommand(new VariableLightCommand());
                                RegisterCommand(new StopCommand());
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
                        response = "Please enter a valid event to run:\n";
                        foreach (var x in Commands)
                        {
                                string args = "";
                                if (x.Value is IUsageProvider usage)
                                {
                                        foreach (string arg in usage.Usage)
                                        {
                                                args += $"[{arg}] ";
                                        }
                                }

                                response += $"{x.Key}{args}: {x.Value.Description}.\n";
                        }

                        return false;
                }
        }
}