using System;
using System.Collections.Generic;
using MEC;
using SnivysServerEvents.Configs;
using Exiled.API.Features;
using UnityEngine;
using Random = System.Random;

namespace SnivysServerEvents.Events;
public class VariableLightsEventHandlers
{
    private static CoroutineHandle _lightChangingHandle;
    private static VariableLightsConfig _config;
    private static bool _vleStarted;
    public VariableLightsEventHandlers()
    {
        _config = new VariableLightsConfig();
        Plugin.ActiveEvent += 1;
        Start();
    }

    public void Start()
    {
        _vleStarted = true;
        Map.ResetLightsColor();
        Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        _lightChangingHandle = Timing.RunCoroutine(VariableLightsTiming());
    }

    public static IEnumerator<float> VariableLightsTiming()
    {
        Random random = new Random();
        if (!_vleStarted)
        {
            Map.ResetLightsColor();
            yield break;
        }
        for (;;)
        {
            float aRandomNumber = (float)random.NextDouble();
            if (_config.ColorChanging)
            {
                float rRandomNumber = (float)random.NextDouble();
                float gRandomNumber = (float)random.NextDouble();
                float bRandomNumber = (float)random.NextDouble();
                foreach (Room room in Room.List)
                    room.Color = new Color(rRandomNumber, gRandomNumber, bRandomNumber, aRandomNumber);
            }
            else
            {
                foreach (Room room in Room.List)
                    room.Color = new Color(1f, 1f, 1f, aRandomNumber);
            }
            yield return Timing.WaitForSeconds(_config.TimeForChange);
        }
    }
    public static void EndEvent()
    {
        if (_vleStarted)
        {
            _vleStarted = false;
            Timing.KillCoroutines(_lightChangingHandle);
            Map.ResetLightsColor();
        }
    }
}