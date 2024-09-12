using System.Collections.Generic;
using MEC;
using SnivysServerEvents.Configs;
using Exiled.API.Features;
using UnityEngine;
using Random = System.Random;

namespace SnivysServerEvents.EventHandlers;
public class VariableLightsEventHandlers
{
    private static CoroutineHandle _lightChangingHandle;
    private static VariableLightsConfig _config;
    private static bool _vleStarted;
    public VariableLightsEventHandlers()
    {
        if (_vleStarted) return;
        _config = Plugin.Instance.Config.VariableLightsConfig;
        Plugin.ActiveEvent += 1;
        Start();
    }

    private static void Start()
    {
        _vleStarted = true;
        Map.ResetLightsColor();
        Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        _lightChangingHandle = Timing.RunCoroutine(VariableLightsTiming());
    }

    private static IEnumerator<float> VariableLightsTiming()
    {
        Random random = new Random();
        if (!_vleStarted)
        {
            Map.ResetLightsColor();
            yield break;
        }
        for (;;)
        {
            if (!_config.ColorChanging)
            {
                if (_config.DifferentLightsPerRoom)
                    foreach (Room room in Room.List)
                        room.Color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(),(float)random.NextDouble());
                else
                {
                    float aRandomNumber = (float)random.NextDouble();
                    foreach (Room room in Room.List)
                        room.Color = new Color(aRandomNumber, aRandomNumber, aRandomNumber, aRandomNumber);
                }
            }
            else
            {
                if (_config.DifferentLightsPerRoom)
                    foreach (Room room in Room.List)
                        room.Color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
                else
                {
                    float aRandomNumber = (float)random.NextDouble();
                    Log.Debug(aRandomNumber);
                    float rRandomNumber = (float)random.NextDouble();
                    Log.Debug(rRandomNumber);
                    float gRandomNumber = (float)random.NextDouble();
                    Log.Debug(gRandomNumber);
                    float bRandomNumber = (float)random.NextDouble();
                    Log.Debug(bRandomNumber);
                    foreach (Room room in Room.List)
                        room.Color = new Color(rRandomNumber, gRandomNumber, bRandomNumber, aRandomNumber);
                }
            }
            yield return Timing.WaitForSeconds(_config.TimeForChange);
        }
    }
    public static void EndEvent()
    {
        if (!_vleStarted) return;
        _vleStarted = false;
        Timing.KillCoroutines(_lightChangingHandle);
        Map.ResetLightsColor();
        Plugin.ActiveEvent -= 1;
    }
}