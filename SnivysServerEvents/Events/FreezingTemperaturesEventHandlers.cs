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

namespace SnivysServerEvents.Events;

public class FreezingTemperaturesEventHandlers
{
    private static CoroutineHandle _freezingTemperaturesHandle;
    private static FreezingTemperaturesConfig _config;
    private static bool _fteStarted;
    public FreezingTemperaturesEventHandlers()
    {
        if (_fteStarted) return;
        _config = Plugin.Instance.Config.FreezingTemperaturesConfig;
        Plugin.ActiveEvent += 1;
        Start();
    }

    public void Start()
    {
        _fteStarted = true;
        Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        DecontaminationController.Singleton.NetworkRoundStartTime = -1.0;
        _freezingTemperaturesHandle = Timing.RunCoroutine(FreezingTemperaturesTiming());
    }

    public static IEnumerator<float> FreezingTemperaturesTiming()
    {
        if (!_fteStarted)
        {
            yield break;
        }
        yield return Timing.WaitForSeconds(_config.LightTimeWarning);
        Cassie.MessageTranslated(_config.LightHalfTimeRemainingWarningMessage, _config.LightHalfTimeRemainingWarningText);
        
        yield return Timing.WaitForSeconds(_config.LightCompleteFreezeTime);
        Cassie.MessageTranslated(_config.LightFrozenOverMessage, _config.LightFrozenOverText);
        
        if (!Lift.Get(ElevatorType.LczA).IsLocked) 
            Lift.Get(ElevatorType.LczA).ChangeLock(DoorLockReason.AdminCommand);
        if (!Lift.Get(ElevatorType.LczB).IsLocked) 
            Lift.Get(ElevatorType.LczB).ChangeLock(DoorLockReason.AdminCommand);
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
                Scp244 freezing = (Scp244)Item.Create(ItemType.SCP244a);
                freezing.Scale = new Vector3(0.01f, 0.01f, 0.01f);
                freezing.Primed = true;
                freezing.MaxDiameter = 10;
                freezing.CreatePickup(rooms.Position);
            }
        }
        yield return Timing.WaitForSeconds(_config.KillPlayersInZoneAfterTime);
        foreach (Player player in Player.List)
        {
            if (player.Zone == ZoneType.LightContainment)
                player.Kill(_config.PlayersDeathReason);
        }
        
        yield return Timing.WaitForSeconds(_config.HeavyTimeWarning);
        Cassie.MessageTranslated(_config.HeavyHalfTimeRemainingWarningMessage, _config.HeavyHalfTimeRemainingWarningText);
        
        yield return Timing.WaitForSeconds(_config.HeavyCompleteFreezeTime);
        Cassie.MessageTranslated(_config.HeavyFrozenOverMessage, _config.HeavyFrozenOverText);
        
        if (!Lift.Get(ElevatorType.Nuke).IsLocked) 
            Lift.Get(ElevatorType.Nuke).ChangeLock(DoorLockReason.AdminCommand);
        if (!Lift.Get(ElevatorType.Scp049).IsLocked) 
            Lift.Get(ElevatorType.Scp049).ChangeLock(DoorLockReason.AdminCommand);
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
                Scp244 freezing = (Scp244)Item.Create(ItemType.SCP244a);
                freezing.Scale = new Vector3(0.01f, 0.01f, 0.01f);
                freezing.Primed = true;
                freezing.MaxDiameter = 10;
                freezing.CreatePickup(rooms.Position);
            }
        }
        yield return Timing.WaitForSeconds(_config.KillPlayersInZoneAfterTime);
        foreach (Player player in Player.List)
        {
            if (player.Zone == ZoneType.HeavyContainment)
                player.Kill(_config.PlayersDeathReason);
        }
        
        yield return Timing.WaitForSeconds(_config.EntranceTimeWarning);
        Cassie.MessageTranslated(_config.EntranceHalfTimeRemainingWarningMessage, _config.EntranceHalfTimeRemainingWarningText);
        
        yield return Timing.WaitForSeconds(_config.EntranceCompleteFreezeTime);
        Cassie.MessageTranslated(_config.EntranceFrozenOverMessage, _config.EntranceFrozenOverText);
        
        if (!Lift.Get(ElevatorType.GateA).IsLocked) 
            Lift.Get(ElevatorType.GateA).ChangeLock(DoorLockReason.AdminCommand);
        if (!Lift.Get(ElevatorType.GateB).IsLocked) 
            Lift.Get(ElevatorType.GateB).ChangeLock(DoorLockReason.AdminCommand);
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
                Scp244 freezing = (Scp244)Item.Create(ItemType.SCP244a);
                freezing.Scale = new Vector3(0.01f, 0.01f, 0.01f);
                freezing.Primed = true;
                freezing.MaxDiameter = 10;
                freezing.CreatePickup(rooms.Position);
            }
        }
        yield return Timing.WaitForSeconds(_config.KillPlayersInZoneAfterTime);
        foreach (Player player in Player.List)
        {
            if (player.Zone == ZoneType.Entrance)
                player.Kill(_config.PlayersDeathReason);
        }
    }
    public static void EndEvent()
    {
        if (_fteStarted)
        {
            _fteStarted = false;
            Timing.KillCoroutines(_freezingTemperaturesHandle);
            Map.ResetLightsColor();
        }
    }
}