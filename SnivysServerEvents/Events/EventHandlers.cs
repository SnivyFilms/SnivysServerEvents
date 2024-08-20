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
using Events = SnivysServerEvents.Events;
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
        
        //Ending round
        public void OnEndingRound(RoundEndedEventArgs ev)
        {
            Log.Debug("Checking if an event is active");
            if (Plugin.ActiveEvent != 0)
            {
                Log.Debug("Disabling Event Handlers, Clearing Generator Count");
                _activatedGenerators = 0;
                BlackoutEventHandlers.EndEvent();
                PeanutHydraEventHandlers.EndEvent();
                PeanutInfectionEventHandlers.EndEvent();
                VariableLightsEventHandlers.EndEvent();
                ShortEventHandlers.EndEvent();
                FreezingTemperaturesEventHandlers.EndEvent();
                Plugin.ActiveEvent = 0;
            }
        }

        /*public void OnRoundStarted()
        {
            throw new System.NotImplementedException();
        }*/

        
        //Blackout
        public void OnGeneratorEngagedBOE(GeneratorActivatingEventArgs ev)
        {
            Log.Debug("Adding amount of generators to count");
            _activatedGenerators = Generator.Get(GeneratorState.Engaged).Count();
            Log.Debug("Checking if generators is 3");
            if (_activatedGenerators == 3)
            {
                Log.Debug("Disabling Blackout Event");
                BlackoutEventHandlers.EndEvent();
                Plugin.ActiveEvent -= 1;
                _activatedGenerators = 0;
            }
        }
        
        // Peanut Hydra
        public void OnKillingPIE(DiedEventArgs ev)
        {
            Log.Debug("Checking if the killer was 173");
            if (ev.Attacker.Role == RoleTypeId.Scp173 && ev.DamageHandler.Type == DamageType.Scp173)
            {
                Log.Debug("Setting the killed to 173");
                Timing.CallDelayed(0.5f, () => ev.Player.Role.Set(RoleTypeId.Scp173, SpawnReason.ForceClass, RoleSpawnFlags.None));
            }
        }
        
        // Peanut Hydra
        public void OnDyingPHE(DyingEventArgs ev)
        {
            if (ev.Player.Role != RoleTypeId.Scp173) return;
            _PHELastKnownHeath = ev.Player.Health;
            _PHELastKnownScale = ev.Player.Scale.y;
        }
        
        public void OnDiedPHE(DiedEventArgs ev)
        {
            if (ev.TargetOldRole != RoleTypeId.Scp173) return;
            //Get the player who died and set them back as 173 
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
        public void OnRoleSwapSE(ChangingRoleEventArgs ev)
        {
            foreach (var player in Player.List)
            {
                player.Scale = new Vector3(ShortEventHandlers.GetPlayerSize(), ShortEventHandlers.GetPlayerSize(), ShortEventHandlers.GetPlayerSize());
            }
        }
    }
}