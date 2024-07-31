using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Mirror;
using MEC;
using PlayerRoles;
using PluginAPI.Events;
using UnityEngine;
using Player = Exiled.Events.Handlers.Player;
using System;

namespace SnivysServerEvents.Events
{
    
    public class EventHandlers
    {
        public Plugin plugin;
        public EventHandlers(Plugin plugin) => this.plugin = plugin;
        
        private static int _activatedGenerators = 0;

        private Vector3 PeanutHydraEventDeathLocation;

        private static float _PHEScale = 1.0f;
        
        public void OnEndingRound(RoundEndedEventArgs ev)
        {
            _activatedGenerators = 0;
            //BlackoutEventHandlers.EndEvent();
            //PeanutInfectionEventHandlers.EndEvent();
        }

        /*public void OnRoundStarted()
        {
            throw new System.NotImplementedException();
        }*/

        public void OnGeneratorEngagedBOE(GeneratorActivatingEventArgs ev)
        {
            _activatedGenerators = Generator.Get(GeneratorState.Engaged).Count();

            if (_activatedGenerators == 3)
            {
                BlackoutEventHandlers.EndEvent();
            }
        }
        public void OnKillingPIE(DiedEventArgs ev)
        {
            if (ev.Attacker.Role == RoleTypeId.Scp173 && ev.DamageHandler.Type == DamageType.Scp173)
            {
                Timing.CallDelayed(2.0f, () => ev.Player.Role.Set(RoleTypeId.Scp173, SpawnReason.ForceClass, RoleSpawnFlags.None));
            }
        }
        public void OnDyingPHE(DyingEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Scp173)
            {
                PeanutHydraEventDeathLocation = ev.Player.Position;
            }
        }

        /*public void OnDiedPHE(DiedEventArgs ev)
        {
            if (ev.TargetOldRole == RoleTypeId.Scp173)
            {
                ev.Player.Role.Set(RoleTypeId.Scp173, SpawnReason.ForceClass, RoleSpawnFlags.None);
                _PHEScale -= 0.05f;
                ev.Player.Scale.Set(_PHEScale, _PHEScale, _PHEScale);
                List<Player> deadPlayers = Player.Get(Team.Dead);
                //Player.Get(RoleTypeId.Spectator).GetRandomValue().RoleSet(RoleTypeId.Scp173);
                foreach (var p in Player.List)
                {
                    p.Role.Set(RoleTypeId.Scp173, SpawnReason.ForceClass, RoleSpawnFlags.None);
                }
            }
        }*/
    }
}