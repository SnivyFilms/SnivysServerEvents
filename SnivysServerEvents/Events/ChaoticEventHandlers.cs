using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using MEC;
using SnivysServerEvents.Configs;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using PlayerRoles;
using Cassie = Exiled.API.Features.Cassie;
using Item = Exiled.API.Features.Items.Item;
using Map = Exiled.API.Features.Map;
using PlayerAPI = Exiled.API.Features.Player;
using PlayerEvent = Exiled.Events.Handlers.Player;
using Random = System.Random;
using Warhead = Exiled.API.Features.Warhead;

namespace SnivysServerEvents.Events;
public class ChaoticEventHandlers
{
    private static CoroutineHandle _choaticHandle;
    private static CoroutineHandle _fakeWarheadHandle;
    private static ChaoticConfig _config;
    private static bool _ceStarted;
    private static bool _ceMedicalItemEvent;
    private static bool _ceFakeWarheadEvent;
    private static float _previousWarheadTime;
    public ChaoticEventHandlers()
    {
        _config = Plugin.Instance.Config.ChaoticConfig;
        Plugin.ActiveEvent += 1;
        Start();
    }

    private void Start()
    {
        _ceStarted = true;
        Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        _choaticHandle = Timing.RunCoroutine(ChaoticTiming());
    }

    private static IEnumerator<float> ChaoticTiming()
    {
        Random random = new Random();
        if (!_ceStarted)
        {
            Map.ResetLightsColor();
            yield break;
        }
        for (;;)
        {
            int chaosRandomNumber = random.Next(minValue:1, maxValue:16);
            Log.Debug(chaosRandomNumber);
            if (_config.ChaosEventEndsOtherEvents)
            {
                Log.Debug("Event ends other events active, ending other events.");
                BlackoutEventHandlers.EndEvent();
                FreezingTemperaturesEventHandlers.EndEvent();
                PeanutHydraEventHandlers.EndEvent();
                PeanutInfectionEventHandlers.EndEvent();
                ShortEventHandlers.EndEvent();
                VariableLightsEventHandlers.EndEvent();
                NameRedactedEventHandlers.EndEvent();
            }
            switch (chaosRandomNumber)
            {
                case 1:
                    if (_config.ItemStealEvent)
                    {
                        Log.Debug("Item Steal Chaos Event is active, running code");
                        foreach (PlayerAPI player in PlayerAPI.List)
                        {
                            Log.Debug("Checking if players aren't a SCP or is dead");
                            if (player.Role.Team != Team.SCPs || player.Role.Team != Team.Dead)
                            {
                                Log.Debug($"Showing Broadcast to {player} saying their items got stolen");
                                player.Broadcast(new Exiled.API.Features.Broadcast(_config.ItemStolenText,
                                    (ushort)_config.BroadcastDisplayTime));
                                Log.Debug($"Clearing Items from {player}");
                                player.ClearItems();
                            }
                        }
                    }
                    else
                        Log.Debug("Item Steal Chaos Event disabled");
                    break;
                case 2:
                    if (_config.GiveRandomItemEvent)
                    {
                        Log.Debug("Give Random Item Event is active, running code");
                        Log.Debug("Getting a list of both standard items and custom items");
                        ItemType[] standardItems = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToArray();
                        List<CustomItem> customItems = CustomItem.Registered.ToList();
                        foreach (PlayerAPI player in PlayerAPI.List)
                        {
                            Log.Debug("Checking if players aren't a SCP or is dead");
                            if (player.Role.Team != Team.SCPs || player.Role.Team != Team.Dead)
                            {
                                int randomItemGiveRng = random.Next(minValue: 1, maxValue: 2);
                                Log.Debug($"Deciding if {player} gets a Custom Item or a Regular Item");
                                if (_config.GiveRandomItemCustomitems)
                                {

                                    switch (randomItemGiveRng)
                                    {
                                        case 1:
                                            CustomItem randomCustomItem = customItems[random.Next(customItems.Count)];
                                            randomCustomItem.Give(player);
                                            Log.Debug($"{player} has received custom item id {randomCustomItem}");
                                            break;
                                        case 2:
                                            ItemType randomStandardItem =
                                                standardItems[random.Next(standardItems.Length)];
                                            player.AddItem(randomStandardItem);
                                            Log.Debug($"{player} has received item id {randomStandardItem}");
                                            break;
                                    }
                                }
                                else
                                {
                                    ItemType randomStandardItem =
                                        standardItems[random.Next(standardItems.Length)];
                                    player.AddItem(randomStandardItem);
                                }
                                player.Broadcast(new Exiled.API.Features.Broadcast(_config.GiveRandomItemText,
                                    (ushort)_config.BroadcastDisplayTime));
                            }
                        }
                    }
                    else
                        Log.Debug("Give Random Item Event is disabled.");
                    break;
                case 3:
                    if (_config.RandomTeleportEvent)
                    {
                        Log.Debug("Random Teleport Event is active, running code");
                        foreach (PlayerAPI player in PlayerAPI.List)
                        {
                            if (!_config.TeleportToFacilityAfterNuke && Warhead.IsDetonated)
                            {
                                Log.Debug($"Warhead has been detonated, teleporting {player} somewhere on Surface");
                                player.Teleport(Room.List.Where(r => r.Type is RoomType.Surface).GetRandomValue());
                            }
                            else if (!_config.TeleportToLightAfterDecom && Map.DecontaminationState == DecontaminationState.Finish)
                            { 
                                Log.Debug($"Light has been decontaminated, teleporting {player} somewhere else in the facility");
                                player.Teleport(Room.List.Where(r => r.Zone is not ZoneType.LightContainment).GetRandomValue());
                            }
                            else
                            {
                                Log.Debug($"Teleporting {player} randomly into the facility");
                                player.Teleport(Room.List.GetRandomValue());
                            }
                            player.Broadcast(new Exiled.API.Features.Broadcast(_config.RandomTeleportText, (ushort)_config.BroadcastDisplayTime));
                        }
                    }
                    else
                        Log.Debug("Random Teleport Event is disabled");
                    break;
                case 4:
                    if (_config.FakeAutoNuke)
                    {
                        Log.Debug("Fake auto nuke event is active, running code");
                        if (!Warhead.IsDetonated || !Warhead.IsInProgress)
                        {
                            _ceFakeWarheadEvent = true;
                            Log.Debug("Saving previous warhead time in the event of the nuke being activated before the fake auto nuke");
                            _previousWarheadTime = Warhead.RealDetonationTimer;
                            Log.Debug("Starting warhead");
                            Warhead.Start();
                            Log.Debug("Checking if the Warhead is locked, if not lock it");
                            if (!Warhead.IsLocked)
                                Warhead.IsLocked = true;
                            foreach (PlayerAPI player in PlayerAPI.List)
                            {
                                Log.Debug($"Displaying the fake autonuke start message to {player}");
                                player.Broadcast(new Exiled.API.Features.Broadcast(_config.FakeAutoNukeStartText,
                                    (ushort)_config.BroadcastDisplayTime));
                            }

                            Log.Debug("Starting a Coroutine to track warhead timing");
                            _fakeWarheadHandle = Timing.RunCoroutine(WarheadTiming());
                        }
                        else
                            Log.Debug("Warhead is either detonated or is in progress, not running this event");
                    }
                    else
                        Log.Debug("Fake auto nuke event is disabled");
                    break;
                case 5:
                    if (_config.RemoveWeaponsEvent)
                    {
                        Log.Debug("Remove Weapons event is active, running code");
                        foreach (PlayerAPI player in PlayerAPI.List)
                        {
                            if (player.Role.Team != Team.SCPs || player.Role.Team != Team.Dead)
                            {
                                foreach (var item in player.Items)
                                {
                                    Log.Debug($"Checking if {player} has a weapon");
                                    if (IsWeapon(item))
                                    {
                                        Log.Debug($"Taking away {player}'s {item}");
                                        player.RemoveItem(item);
                                        player.Broadcast(new Exiled.API.Features.Broadcast(_config.RemoveWeaponsText,
                                            (ushort)_config.BroadcastDisplayTime));
                                    }
                                }
                            }
                        }
                    }
                    else
                        Log.Debug("Remove Weapons Event is disabled");
                    break;
                case 6:
                    if (_config.GiveRandomWeaponsEvent)
                    {
                        Log.Debug("Giving random weapons is active, running code");
                        foreach (PlayerAPI player in PlayerAPI.List)
                        {
                            if (player.Role.Team != Team.SCPs || player.Role.Team != Team.Dead)
                            {
                                Log.Debug($"Checking if {player} is able to get a random weapon");
                                ItemType randomWeapon = WeaponTypes[random.Next(WeaponTypes.Length)];
                                if (_config.GiveRandomWeaponsToUnarmedPlayers)
                                {
                                    foreach (var item in player.Items)
                                    {
                                        Log.Debug($"Checking if {player} has a weapon");
                                        if (IsWeapon(item))
                                        {
                                            Log.Debug($"{player} has a weapon, not granting another");
                                            player.Broadcast(new Exiled.API.Features.Broadcast(_config.GiveRandomWeaponsTextFail,
                                                (ushort)_config.BroadcastDisplayTime));
                                        }
                                        else
                                        {
                                            if (_config.GiveAllRandomWeapons)
                                            {
                                                Log.Debug($"Giving {player} a random weapon");
                                                player.AddItem(randomWeapon);
                                            }
                                            else if (_config.GiveRandomWeaponsDefined == null)
                                            {
                                                Log.Warn("VVE Chaos Event: GiveAllRandomWeapons is false but the GiveRandomWeaponsDefined is empty! Falling back to any random weapon.");
                                                player.AddItem(randomWeapon);
                                            }
                                            else
                                            {
                                                Log.Debug($"Giving {player} a predefined weapon");
                                                player.AddItem(
                                                    _config.GiveRandomWeaponsDefined[
                                                        random.Next(_config.GiveRandomWeaponsDefined.Count)]);
                                            }

                                            player.Broadcast(new Exiled.API.Features.Broadcast(_config.GiveRandomWeaponsText,
                                                (ushort)_config.BroadcastDisplayTime));
                                        }
                                    }
                                }
                                else
                                {
                                    if (player.IsInventoryFull)
                                    {
                                        Log.Debug($"{player}'s inventory is full, not giving any weapon");
                                        player.Broadcast(new Exiled.API.Features.Broadcast(_config.GiveRandomWeaponsTextFail,
                                            (ushort)_config.BroadcastDisplayTime));
                                    }
                                    else
                                    {
                                        if (_config.GiveAllRandomWeapons)
                                        {
                                            Log.Debug($"Giving {player} a random weapon");
                                            player.AddItem(randomWeapon);
                                        }
                                        else if (_config.GiveRandomWeaponsDefined == null)
                                        {
                                            Log.Warn("VVE Chaos Event: GiveAllRandomWeapons is false but the GiveRandomWeaponsDefined is empty! Falling back to any random weapon.");
                                            player.AddItem(randomWeapon);
                                        }
                                        else
                                        {
                                            Log.Debug($"Giving {player} a predefined weapon");
                                            player.AddItem(
                                                _config.GiveRandomWeaponsDefined[
                                                    random.Next(_config.GiveRandomWeaponsDefined.Count)]);
                                        }
                                        player.Broadcast(new Exiled.API.Features.Broadcast(_config.GiveRandomWeaponsText,
                                            (ushort)_config.BroadcastDisplayTime));
                                    }
                                }
                            }
                        }
                    }
                    else
                        Log.Debug("Giving random weapons event is disabled");
                    break;
                case 7:
                    if (_config.DeathMatchEvent)
                    {
                        Log.Debug("Death Match Event is active, running code");
                        foreach (PlayerAPI player in PlayerAPI.List)
                        {
                            Log.Debug("Checking if SCPs should be affected");
                            if (!_config.DeathMatchEventAffectsSCPs)
                            {
                                Log.Debug("SCPs aren't affected");
                                if (player.Role.Team != Team.SCPs || player.Role.Team != Team.Dead)
                                {
                                    Log.Debug($"Setting {player}'s health to {_config.DeathMatchHealth}");
                                    player.Health = _config.DeathMatchHealth;
                                    player.Broadcast(new Exiled.API.Features.Broadcast(_config.DeathMatchText,
                                        (ushort)_config.BroadcastDisplayTime));
                                }
                            }
                            else
                            {
                                Log.Debug("SCPs are affected");
                                Log.Debug($"Setting {player}'s health to {_config.DeathMatchHealth}");
                                player.Health = _config.DeathMatchHealth;
                                player.Broadcast(new Exiled.API.Features.Broadcast(_config.DeathMatchText,
                                    (ushort)_config.BroadcastDisplayTime));
                            }
                        }
                    }
                    else
                        Log.Debug("Death Match Event is disabled");
                    break;
                case 8:
                    if (_config.ChaosEventEnablesOtherEvents)
                    {
                        Log.Debug("Chaos Event Enables other Events true, running Blackout Event");
                        var blackoutEventHandlers = new BlackoutEventHandlers();
                    }
                    else
                        Log.Debug("Chaos Event enabling other events is disabled");
                    break;
                case 9:
                    if (_config.ChaosEventEnablesOtherEvents)
                    {
                        Log.Debug("Chaos Event Enables other Events true, running Freezing Temperature Event");
                        var freezingTemperaturesEventHandlers = new FreezingTemperaturesEventHandlers();
                    }
                    else
                        Log.Debug("Chaos Event enabling other events is disabled");
                    break;
                case 10:
                    if (_config.ChaosEventEnablesOtherEvents)
                    {
                        Log.Debug("Chaos Event Enables other Events true, running Peanut Hydra Event");
                        var peanutHydraEventHandlers = new PeanutHydraEventHandlers();
                    }
                    else
                        Log.Debug("Chaos Event enabling other events is disabled");
                    break;
                case 11:
                    if (_config.ChaosEventEnablesOtherEvents)
                    {
                        Log.Debug("Chaos Event Enables other Events true, running Peanut Infection Event");
                        var peanutInfectionEventHandlers = new PeanutInfectionEventHandlers();
                    }
                    else
                        Log.Debug("Chaos Event enabling other events is disabled");
                    break;
                case 12:
                    if (_config.ChaosEventEnablesOtherEvents)
                    {
                        Log.Debug("Chaos Event Enables other Events true, running Short People Event");
                        var shortEventHandlers = new ShortEventHandlers();
                    }
                    else
                        Log.Debug("Chaos Event enabling other events is disabled");
                    break;
                case 13:
                    if (_config.ChaosEventEnablesOtherEvents)
                    {
                        Log.Debug("Chaos Event Enables other Events true, running Variable Lights Event");
                        var variableLightsEvent = new VariableLightsEventHandlers();
                    }
                    else
                        Log.Debug("Chaos Event enabling other events is disabled");
                    break;
                case 14:
                    if (_config.FBIOpenUpEvent)
                    {
                        Log.Debug("FBI Open Up Event active, running code");
                        Log.Debug("Getting a random non-foundation player");
                        PlayerAPI FBIOpenUpTarget = GetRandomPlayerFBI();
                        Log.Debug($"Showing {FBIOpenUpTarget} the warning message they are about to have the foundation teleport to them");
                        FBIOpenUpTarget.Broadcast(new Exiled.API.Features.Broadcast(_config.FBIOpenUpTargetText, (ushort)_config.BroadcastDisplayTime));
                        yield return Timing.WaitForSeconds(_config.FBITeleportTime);
                        foreach (PlayerAPI player in PlayerAPI.List)
                        {
                            if (player.Role.Team == Team.FoundationForces)
                            {
                                Log.Debug($"{player} is on the MTF, Showing warning text");
                                player.Broadcast(new Exiled.API.Features.Broadcast(_config.FBIOpenUpMTFText, (ushort)_config.BroadcastDisplayTime));
                                Log.Debug($"Teleporting {player} to {FBIOpenUpTarget}");
                                player.Teleport(FBIOpenUpTarget.Position);
                            }
                        }
                    }
                    else
                        Log.Debug("FBI Open Up Event disabled");
                    break;
                case 15:
                    if (_config.GrenadeFeetEvent)
                    {
                        Log.Debug("Grenade Feet Event active, running code");
                        foreach (PlayerAPI player in PlayerAPI.List)
                        {
                            Log.Debug($"Grenade feet warning being shown to {player}");
                            player.Broadcast(new Exiled.API.Features.Broadcast(_config.GrenadeFeetText, (ushort) _config.BroadcastDisplayTime));
                        }

                        yield return Timing.WaitForSeconds(random.Next(minValue: 1, maxValue: 50));

                        foreach (PlayerAPI player in PlayerAPI.List)
                        {
                            Log.Debug($"Spawning a grenade on {player}");
                            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                            if (_config.GrenadeFeetRandomFuse)
                                grenade.FuseTime = random.Next(minValue: 1, maxValue: 50);
                            else
                                grenade.FuseTime = _config.GrenadeFeetFuse;
                            grenade.SpawnActive(player.Position);
                        }
                    }
                    else
                        Log.Debug("Grenade Feet Event disabled");
                    break;
                case 16:
                    if (_config.UnsafeMedicalItemsEvent)
                    {
                        Log.Debug("Unsafe medical items event active, running code");
                        Log.Debug("Activating Event Handlers for on using medical item");
                        PlayerEvent.UsingItemCompleted += Plugin.Instance.eventHandlers.OnUsingMedicalItemCE;
                        _ceMedicalItemEvent = true;
                        foreach (PlayerAPI player in PlayerAPI.List)
                        {
                            if (player.Role.Team != Team.SCPs)
                            {
                                Log.Debug($"Displaying Unsafe Medical Items Warning to {player}");
                                player.Broadcast(new Exiled.API.Features.Broadcast(_config.UnsafeMedicalItemsText,
                                    (ushort)_config.BroadcastDisplayTime));
                            }
                        }
                        if (_config.UnsafeMedicalItemsUseRandomTime)
                            yield return Timing.WaitForSeconds(random.Next(minValue: 1, maxValue: 50));
                        else
                            yield return Timing.WaitForSeconds(_config.UnsafeMedicalItemsFixedTime);
                        Log.Debug("Disabling Event Handlers for on using medical item events");
                        PlayerEvent.UsingItemCompleted -= Plugin.Instance.eventHandlers.OnUsingMedicalItemCE;
                        _ceMedicalItemEvent = false;
                        foreach (PlayerAPI player in PlayerAPI.List)
                        {
                            if (player.Role.Team != Team.SCPs)
                            {
                                Log.Debug($"Displaying Unsafe Medical Items Warning to {player}");
                                player.Broadcast(new Exiled.API.Features.Broadcast(_config.UnsafeMedicalItemsSafeToUseText,
                                    (ushort)_config.BroadcastDisplayTime));
                            }
                        }
                    }
                    else
                        Log.Debug("Unsafe medical Items Event disabled");
                    break;
                case 17:
                    if (_config.ChaosEventEnablesOtherEvents)
                    {
                        Log.Debug("Chaos Event Enables other Events true, running Name Redacted Event");
                        var nameRedactedHandlers = new NameRedactedEventHandlers();
                    }
                    else
                        Log.Debug("Chaos Event enabling other events is disabled");
                    break;
            }
            yield return Timing.WaitForSeconds(_config.TimeForChaosEvent);
        }
    }

    private static IEnumerator<float> WarheadTiming()
    {
        for (;;)
        {
            if (Warhead.DetonationTimer > _config.FakeAutoNukeTimeOut)
            {
                Log.Debug(
                    $"Warhead time hasn't reached {_config.FakeAutoNukeTimeOut} yet. Waiting half a second and checking again");
                Log.Debug($"Detonation Time is {Warhead.DetonationTimer}");
            }
            else
            {
                Log.Debug("Time has been reached, stopping warhead");
                Warhead.IsLocked = false;
                Warhead.DetonationTimer = _previousWarheadTime;
                Warhead.Stop();
                foreach (PlayerAPI player in PlayerAPI.List)
                {
                    Log.Debug($"Displaying the fake out message to {player}");
                    player.Broadcast(new Exiled.API.Features.Broadcast(_config.FakeAutoNukeFakeoutText,
                        (ushort)_config.BroadcastDisplayTime));
                }

                _ceFakeWarheadEvent = false;
                yield break;
            }
            yield return Timing.WaitForSeconds(0.5f);
        }
    }
    private static readonly ItemType[] WeaponTypes =
    [
        ItemType.GunA7,
        ItemType.GunCom45,
        ItemType.GunCrossvec,
        ItemType.GunLogicer,
        ItemType.GunRevolver,
        ItemType.GunShotgun,
        ItemType.GunAK,
        ItemType.GunCOM15,
        ItemType.GunCOM18,
        ItemType.GunE11SR,
        ItemType.GunFSP9,
        ItemType.GunFRMG0,
        ItemType.Jailbird,
        ItemType.ParticleDisruptor,
        ItemType.GrenadeFlash,
        ItemType.GrenadeHE
    ];
    private static bool IsWeapon(Item item)
    {
        return WeaponTypes.Contains(item.Type);
    }
    private static PlayerAPI GetRandomPlayerFBI()
    {
        Random random = new Random();
        List<PlayerAPI> fbiOpenUpPossibleTargets = PlayerAPI.List.Where(p => p.Role != (RoleTypeId)Team.FoundationForces || p.Role != (RoleTypeId)Team.Scientists || p.Role != (RoleTypeId)Team.Dead).ToList();
        if (fbiOpenUpPossibleTargets.Count == 0)
            return null;
        int index = random.Next(fbiOpenUpPossibleTargets.Count);
        return fbiOpenUpPossibleTargets[index];
    }
    public static void EndEvent()
    {
        if (!_ceStarted) return;
        _ceStarted = false;
        Timing.KillCoroutines(_choaticHandle);
        Plugin.ActiveEvent -= 1;
        if (_ceMedicalItemEvent)
        {
            PlayerEvent.UsingItemCompleted -= Plugin.Instance.eventHandlers.OnUsingMedicalItemCE;
            _ceMedicalItemEvent = false;
        }
        if (_ceFakeWarheadEvent)
        {
            Timing.KillCoroutines(_fakeWarheadHandle);
            _ceFakeWarheadEvent = false;
        }
    }
}