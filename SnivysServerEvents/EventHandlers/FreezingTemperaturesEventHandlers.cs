using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Interactables.Interobjects.DoorUtils;
using LightContainmentZoneDecontamination;
using MEC;
using SnivysServerEvents.Configs;
using UnityEngine;
using CheckpointDoor = Exiled.API.Features.Doors.CheckpointDoor;

namespace SnivysServerEvents.EventHandlers;

public class FreezingTemperaturesEventHandlers
{
    private static CoroutineHandle _freezingTemperaturesHandle;
    private static FreezingTemperaturesConfig _config;
    private static bool _fteStarted;
    public FreezingTemperaturesEventHandlers()
    {
        Log.Debug("Checking if Freezing Temperatures Event has already started");
        if (_fteStarted) return;
        _config = Plugin.Instance.Config.FreezingTemperaturesConfig;
        Plugin.ActiveEvent += 1;
        _fteStarted = true;
        Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        DecontaminationController.Singleton.NetworkRoundStartTime = -1.0;
        _freezingTemperaturesHandle = Timing.RunCoroutine(FreezingTemperaturesTiming());
        Log.Debug("Stopping regular decontamination, starting custom freezing temperatures system");
    }
    
    private static IEnumerator<float> FreezingTemperaturesTiming()
    {
        Log.Debug("Doing a quick check to make sure the event isn't started properly");
        if (!_fteStarted)
            yield break;
        
        Log.Debug("Making and defining the SCP-244 instance that will be used to freeze each zone");
        Scp244 freezing = (Scp244)Item.Create(ItemType.SCP244a);
        freezing.Scale = new Vector3(0.01f, 0.01f, 0.01f);
        freezing.Primed = true;
        freezing.MaxDiameter = 10;
        Log.Debug($"Info about SCP-244 that will be used. Scale: {freezing.Scale}, Primed: {freezing.Primed}, Max Diameter {freezing.MaxDiameter} (decently sure this doesnt change the cloud size but idk");
        
        Log.Debug($"Waiting {_config.LightTimeWarning} seconds");
        yield return Timing.WaitForSeconds(_config.LightTimeWarning);
        Log.Debug("Showing Light Half Time Remaining Message");
        Cassie.MessageTranslated(_config.LightHalfTimeRemainingWarningMessage, _config.LightHalfTimeRemainingWarningText);
        
        Log.Debug($"Waiting {_config.LightCompleteFreezeTime} seconds");
        yield return Timing.WaitForSeconds(_config.LightCompleteFreezeTime);
        Log.Debug("Showing Light Frozen Over Message");
        Cassie.MessageTranslated(_config.LightFrozenOverMessage, _config.LightFrozenOverText);
        
        Log.Debug("Locking and closing Light Containment Zone Elevators");
        if (!Lift.Get(ElevatorType.LczA).IsLocked) 
            Lift.Get(ElevatorType.LczA).ChangeLock(DoorLockReason.AdminCommand);
        if (!Lift.Get(ElevatorType.LczB).IsLocked) 
            Lift.Get(ElevatorType.LczB).ChangeLock(DoorLockReason.AdminCommand);
        
        Log.Debug("Locking and closing each door in Light Containment Zone");
        Log.Debug("Spawning SCP-244s in each room in Light Containment Zone");
        foreach (Room rooms in Room.List)
        {
            if (rooms.Zone == ZoneType.LightContainment)
            {
                foreach (Door door in Door.List)
                {
                    if (door.Zone == ZoneType.LightContainment)
                    {
                        if (!door.IsLocked)
                            door.ChangeLock(DoorLockType.AdminCommand);
                        if (door.IsOpen)
                            door.IsOpen = false;
                    }
                }
                freezing.CreatePickup(rooms.Position);
            }
        }
        
        Log.Debug($"Waiting {_config.KillPlayersInZoneAfterTime} seconds");
        yield return Timing.WaitForSeconds(_config.KillPlayersInZoneAfterTime);
        foreach (Player player in Player.List)
        {
            if (player.Zone == ZoneType.LightContainment)
            {
                Log.Debug($"Killing {player} as they are in Light Containment Zone");
                player.Kill(_config.PlayersDeathReason);
            }
        }
        
        Log.Debug($"Waiting {_config.HeavyTimeWarning} seconds");
        yield return Timing.WaitForSeconds(_config.HeavyTimeWarning);
        Log.Debug("Showing Heavy Half Time Remaining Message");
        Cassie.MessageTranslated(_config.HeavyHalfTimeRemainingWarningMessage, _config.HeavyHalfTimeRemainingWarningText);
        
        Log.Debug($"Waiting {_config.HeavyCompleteFreezeTime} seconds");
        yield return Timing.WaitForSeconds(_config.HeavyCompleteFreezeTime);
        Log.Debug("Showing Heavy Frozen over Cassie Message");
        Cassie.MessageTranslated(_config.HeavyFrozenOverMessage, _config.HeavyFrozenOverText);
        
        Log.Debug("Locking Nuke & SCP-049 Elevators");
        if (!Lift.Get(ElevatorType.Nuke).IsLocked) 
            Lift.Get(ElevatorType.Nuke).ChangeLock(DoorLockReason.AdminCommand);
        if (!Lift.Get(ElevatorType.Scp049).IsLocked) 
            Lift.Get(ElevatorType.Scp049).ChangeLock(DoorLockReason.AdminCommand);
        Log.Debug("Closing and locking all Heavy Containment Zone Doors");
        Log.Debug("Spawning SCP-244's in each room");
        foreach (Room rooms in Room.List)
        {
            if (rooms.Zone == ZoneType.HeavyContainment)
            {
                foreach (Door door in Door.List)
                {
                    if (door.Zone == ZoneType.HeavyContainment)
                    {
                        if (!door.IsLocked)
                            door.ChangeLock(DoorLockType.AdminCommand);
                        if (door.IsOpen)
                            door.IsOpen = false;
                    }
                    else if (door is CheckpointDoor checkpointDoor)
                    {
                        checkpointDoor.IsOpen = false;
                        if (!checkpointDoor.IsLocked)
                            checkpointDoor.ChangeLock((DoorLockType)DoorLockReason.AdminCommand);
                    }
                }
                freezing.CreatePickup(rooms.Position);
            }
        }
        
        Log.Debug($"Waiting {_config.KillPlayersInZoneAfterTime} seconds");
        yield return Timing.WaitForSeconds(_config.KillPlayersInZoneAfterTime);
        foreach (Player player in Player.List)
        {
            if (player.Zone == ZoneType.HeavyContainment)
            {
                Log.Debug($"Killing {player} as they are in Heavy Containment Zone");
                player.Kill(_config.PlayersDeathReason);
            }
        }
        
        Log.Debug($"Waiting {_config.EntranceTimeWarning} seconds");
        yield return Timing.WaitForSeconds(_config.EntranceTimeWarning);
        Log.Debug("Showing Entrance Half Time Remaining Warning");
        Cassie.MessageTranslated(_config.EntranceHalfTimeRemainingWarningMessage, _config.EntranceHalfTimeRemainingWarningText);
        
        Log.Debug($"Waiting {_config.EntranceCompleteFreezeTime} seconds");
        yield return Timing.WaitForSeconds(_config.EntranceCompleteFreezeTime);
        Log.Debug("Showing Entrance Frozen Over Message");
        Cassie.MessageTranslated(_config.EntranceFrozenOverMessage, _config.EntranceFrozenOverText);
        
        Log.Debug("Locking Gate A and B elevators");
        if (!Lift.Get(ElevatorType.GateA).IsLocked) 
            Lift.Get(ElevatorType.GateA).ChangeLock(DoorLockReason.AdminCommand);
        if (!Lift.Get(ElevatorType.GateB).IsLocked) 
            Lift.Get(ElevatorType.GateB).ChangeLock(DoorLockReason.AdminCommand);
        Log.Debug("Closing and Locking all doors in Entrance Zone");
        Log.Debug("Spawning SCP-244 in each Entrance Zone Room");
        foreach (Room rooms in Room.List)
        {
            if (rooms.Zone == ZoneType.Entrance)
            {
                foreach (Door door in Door.List)
                {
                    if (door.Zone == ZoneType.Entrance)
                    {
                        if (!door.IsLocked)
                            door.ChangeLock(DoorLockType.AdminCommand);
                        if (door.IsOpen)
                            door.IsOpen = false;
                    }
                }
                freezing.CreatePickup(rooms.Position);
            }
        }
        
        Log.Debug($"Waiting {_config.KillPlayersInZoneAfterTime} seconds");
        yield return Timing.WaitForSeconds(_config.KillPlayersInZoneAfterTime);
        foreach (Player player in Player.List)
        {
            if (player.Zone == ZoneType.Entrance)
            {
                Log.Debug($"Killing {player} as they are in Entrance Zone");
                player.Kill(_config.PlayersDeathReason);
            }
        }
        
        Log.Debug("Ending the event as this event has ran through all it needs to do");
        EndEvent();
    }
    public static void EndEvent()
    {
        if (!_fteStarted) return;
        _fteStarted = false;
        Plugin.ActiveEvent -= 1;
        Timing.KillCoroutines(_freezingTemperaturesHandle);
    }
}