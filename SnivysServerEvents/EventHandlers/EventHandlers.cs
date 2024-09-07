using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using PluginAPI.Events;
using SnivysServerEvents.EventHandlers;
using UnityEngine;
using Random = System.Random;
using Events = SnivysServerEvents.EventHandlers;

// ReSharper disable InconsistentNaming

namespace SnivysServerEvents.EventHandlers
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
            Log.Debug("Checking if an event is active at round end");
            EndEvents();
        }
        
        //Waiting for Players
        public void OnWaitingForPlayers()
        {
            Log.Debug("Checking if an event is active at waiting for players");
            EndEvents();
        }

        //Stop Events Command
        public static void StopEventsCommand()
        {
            Log.Debug("Killing events due to the stop command being used");
            EndEvents();
        }

        //Ends Events method, On Round End, On Waiting For Players, and Stop Commands points here
        private static void EndEvents()
        {
            Log.Debug("Checking again if there's events active");
            if (Plugin.ActiveEvent == 0) return;
            Log.Debug("Disabling Event Handlers, Clearing Generator Count");
            _activatedGenerators = 0;
            BlackoutEventHandlers.EndEvent();
            PeanutHydraEventHandlers.EndEvent();
            PeanutInfectionEventHandlers.EndEvent();
            VariableLightsEventHandlers.EndEvent();
            ShortEventHandlers.EndEvent();
            FreezingTemperaturesEventHandlers.EndEvent();
            ChaoticEventHandlers.EndEvent();
            NameRedactedEventHandlers.EndEvent();
            Plugin.ActiveEvent = 0;
        }
        
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
        
        // Peanut Infection
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
        
        //Chaos Event
        public void OnUsingMedicalItemCE(UsingItemCompletedEventArgs ev)
        {
            if (ev.Usable.Type is ItemType.Adrenaline or ItemType.Painkillers or ItemType.Medkit or ItemType.SCP500)
            {
                Random random = new();
                int chance = random.Next(minValue: 1, maxValue: 2);
                if (chance == 1)
                    return;
                chance = random.Next(minValue: 1, maxValue: 41);
                switch (chance)
                {
                    case 1:
                        break;
                    case 2:
                        ev.Player.EnableEffect(EffectType.Asphyxiated, 1, random.Next());
                        break;
                    case 3:
                        ev.Player.EnableEffect(EffectType.Bleeding, 1, random.Next());
                        break;
                    case 4:
                        ev.Player.EnableEffect(EffectType.Blinded, 1, random.Next());
                        break;
                    case 5:
                        ev.Player.EnableEffect(EffectType.Burned, 1, random.Next());
                        break;
                    case 6:
                        ev.Player.EnableEffect(EffectType.Concussed, 1, random.Next());
                        break;
                    case 7:
                        ev.Player.EnableEffect(EffectType.Corroding, 1, random.Next());
                        break;
                    case 8:
                        ev.Player.EnableEffect(EffectType.Deafened, 1, random.Next());
                        break;
                    case 9:
                        ev.Player.EnableEffect(EffectType.Decontaminating, 1, random.Next());
                        break;
                    case 10:
                        ev.Player.EnableEffect(EffectType.Disabled, 1, random.Next());
                        break;
                    case 11:
                        ev.Player.EnableEffect(EffectType.Ensnared, 1, random.Next());
                        break;
                    case 12:
                        ev.Player.EnableEffect(EffectType.Exhausted, 1, random.Next());
                        break;
                    case 13:
                        ev.Player.EnableEffect(EffectType.Flashed, 1, random.Next());
                        break;
                    case 14:
                        ev.Player.EnableEffect(EffectType.Ghostly, 1, random.Next());
                        break;
                    case 15:
                        ev.Player.EnableEffect(EffectType.Hemorrhage, 1, random.Next());
                        break;
                    case 16:
                        ev.Player.EnableEffect(EffectType.Hypothermia, 1, random.Next());
                        break;
                    case 17:
                        ev.Player.EnableEffect(EffectType.Invigorated, 1, random.Next());
                        break;
                    case 18:
                        ev.Player.EnableEffect(EffectType.Invisible, 1, random.Next());
                        break;
                    case 19:
                        ev.Player.EnableEffect(EffectType.Poisoned, 1, random.Next());
                        break;
                    case 20:
                        ev.Player.EnableEffect(EffectType.Scp207, (byte)random.Next(minValue: 1, maxValue: 3), random.Next());
                        break;
                    case 21:
                        ev.Player.EnableEffect(EffectType.Scp1853, 1, random.Next());
                        break;
                    case 22:
                        ev.Player.EnableEffect(EffectType.Slowness, (byte)random.Next(minValue: 1, maxValue: 255), random.Next());
                        break;
                    case 23:
                        ev.Player.EnableEffect(EffectType.Stained, 1, random.Next());
                        break;
                    case 24:
                        ev.Player.EnableEffect(EffectType.Traumatized, 1, random.Next());
                        break;
                    case 25:
                        ev.Player.EnableEffect(EffectType.Vitality, 1, random.Next());
                        break;
                    case 26:
                        ev.Player.EnableEffect(EffectType.AmnesiaItems, 1, random.Next());
                        break;
                    case 27:
                        ev.Player.EnableEffect(EffectType.AmnesiaVision, 1, random.Next());
                        break;
                    case 28:
                        ev.Player.EnableEffect(EffectType.AntiScp207, (byte)random.Next(minValue: 1, maxValue:3), random.Next());
                        break;
                    case 29:
                        ev.Player.EnableEffect(EffectType.BodyshotReduction, (byte)random.Next(minValue: 1, maxValue: 4), random.Next());
                        break;
                    case 30:
                        ev.Player.EnableEffect(EffectType.CardiacArrest, 1, random.Next());
                        break;
                    case 31:
                        ev.Player.EnableEffect(EffectType.DamageReduction, (byte)random.Next(minValue: 1, maxValue: 255), random.Next());
                        break;
                    case 32:
                        ev.Player.EnableEffect(EffectType.FogControl, (byte)random.Next(minValue: 1, maxValue: 7), random.Next());
                        break;
                    case 33:
                        ev.Player.EnableEffect(EffectType.MovementBoost, (byte)random.Next(minValue: 1, maxValue: 255), random.Next());
                        break;
                    case 34:
                        ev.Player.EnableEffect(EffectType.PocketCorroding, 1, random.Next());
                        break;
                    case 35:
                        ev.Player.EnableEffect(EffectType.RainbowTaste, (byte)random.Next(minValue: 1, maxValue: 3), random.Next());
                        break;
                    case 36:
                        ev.Player.EnableEffect(EffectType.SeveredHands, 1, random.Next());
                        break;
                    case 37:
                        ev.Player.EnableEffect(EffectType.SilentWalk, 1, random.Next());
                        break;
                    case 38:
                        ev.Player.EnableEffect(EffectType.SinkHole, 1, random.Next());
                        break;
                    case 39:
                        ev.Player.EnableEffect(EffectType.SpawnProtected, 1, random.Next());
                        break;
                    case 40:
                        ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                        grenade.FuseTime = random.Next();
                        grenade.SpawnActive(ev.Player.Position);
                        break;
                    case 41:
                        ev.Player.Kill("You used a medical item unsafely");
                        break;
                }

            }
        }
    }
}