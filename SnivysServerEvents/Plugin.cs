using System;
using Exiled.API.Features;
using SnivysServerEvents.Configs;
using Server = Exiled.Events.Handlers.Server;

namespace SnivysServerEvents
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance; 
        public override string Name { get; } = "Snivy's Custom In Round Events";
        public override string Author { get; } = "Vicious Vikki, with the assistance from Lucid & Jamwolff";
        public override string Prefix { get; } = "VVEvents";
        public override Version Version { get; } = new Version(1, 5, 0);
        public override Version RequiredExiledVersion { get; } = new Version(8, 11, 0);
        public static int ActiveEvent = 0;
        
        public EventHandlers.EventHandlers EventHandlers;
        public override void OnEnabled()
        {
            Instance = this;
            EventHandlers = new EventHandlers.EventHandlers(this);
            if(Instance.Config.RandomlyStartingEvents)
                Server.RoundStarted += EventHandlers.OnRoundStart;
            Server.RoundEnded += EventHandlers.OnEndingRound;
            Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            if(Instance.Config.RandomlyStartingEvents)
                Server.RoundStarted -= EventHandlers.OnRoundStart;
            Server.RoundEnded -= EventHandlers.OnEndingRound;
            Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            EventHandlers = null;
            Instance = null;
            base.OnDisabled();
        }
    }
}