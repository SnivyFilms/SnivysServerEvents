using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using UnityEngine;
using Random = System.Random;
// ReSharper disable InconsistentNaming

namespace SnivysServerEvents.Events
{
    
    public class EventHandlers
    {
        public Plugin Plugin;
        public EventHandlers(Plugin plugin) => Plugin = plugin;
        
        private static int _activatedGenerators;
        private static float _PHEScale;
        private static float _PHENewHealth;
        private static float _PHELastKnownHeath;
        private static float _PHELastKnownScale;
        
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
            if (ev.Player.Role != RoleTypeId.Scp173) return;
            _PHELastKnownHeath = ev.Player.Health;
            _PHELastKnownScale = ev.Player.Scale.y;
        }
        
        public void OnDiedPHE(DiedEventArgs ev)
        {
            if (ev.TargetOldRole != RoleTypeId.Scp173) return;
            //Set the player who died and set them back as 173 
            ev.Player.Role.Set(RoleTypeId.Scp173, SpawnReason.ForceClass, RoleSpawnFlags.None);
            //calculate the new scale and health
            _PHEScale = Mathf.Max(0.1f, _PHELastKnownScale / 2);
            _PHENewHealth = _PHELastKnownHeath / 2;
            //apply them to the formerly dead player
            ev.Player.Health = Mathf.Max(_PHENewHealth, 1);
            ev.Player.Scale.Set(_PHEScale, _PHEScale, _PHEScale);
            //Get a random spectator and set them as a duplicate 173
            Player newPlayer = GetRandomSpectator();
            switch (newPlayer)
            {
                case null when PeanutHydraEventHandlers.Config.UseAttackersIfNeeded:
                    Log.Debug("No spectators found to become the new SCP-173, using attacker...");
                    newPlayer = ev.Attacker;
                    break;
                case null:
                    Log.Debug("No spectators found to become the new SCP-173");
                    return;
            }
            newPlayer.Role.Set(RoleTypeId.Scp173, SpawnReason.ForceClass, RoleSpawnFlags.None);
            newPlayer.Position = ev.Player.Position;
            newPlayer.Health = _PHENewHealth;
            newPlayer.Scale = new Vector3(_PHEScale, _PHEScale, _PHEScale);
        }

        private static Player GetRandomSpectator()
        {
            // Get a list of players with the Spectator role
            List<Player> spectators = Player.List.Where(p => p.Role == RoleTypeId.Spectator).ToList();

            // If there are no spectators, return null
            if (spectators.Count == 0)
                return null;

            // Select a random spectator
            Random random = new();
            int index = random.Next(spectators.Count);
            return spectators[index];
        }
    }
}