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
        public override Version Version { get; } = new Version(1, 1, 0);
        public override Version RequiredExiledVersion { get; } = new Version(8, 9, 6);
        public static int ActiveEvent = 0;
        
        public EventHandlers eventHandlers;
        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers(this);
            //Server.RoundStarted += eventHandlers.OnRoundStarted;
            Server.RoundEnded += eventHandlers.OnEndingRound;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            eventHandlers = null;
            base.OnDisabled();
        }
    }
}