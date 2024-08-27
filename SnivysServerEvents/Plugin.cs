using System;
using Exiled.API.Features;
using SnivysServerEvents.Configs;
using SnivysServerEvents.Events;
using Server = Exiled.Events.Handlers.Server;

namespace SnivysServerEvents
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance; 
        public override string Name { get; } = "Snivy's Custom In Round Events";
        public override string Author { get; } = "Vicious Vikki, with the assistance from Lucid";
        public override string Prefix { get; } = "VVEvents";
        public override Version Version { get; } = new Version(1, 3, 0);
        public override Version RequiredExiledVersion { get; } = new Version(8, 11, 0);
        public static int ActiveEvent = 0;
        
        public EventHandlers eventHandlers;
        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers(this);
            Server.RoundEnded += eventHandlers.OnEndingRound;
            Server.WaitingForPlayers += eventHandlers.OnWaitingForPlayers;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            Server.RoundEnded -= eventHandlers.OnEndingRound;
            Server.WaitingForPlayers -= eventHandlers.OnWaitingForPlayers;
            eventHandlers = null;
            base.OnDisabled();
        }
    }
}