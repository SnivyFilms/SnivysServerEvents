using System;
using System.Collections.Generic;
using Exiled.API.Extensions;
using MEC;
using SnivysServerEvents.Configs;
using Exiled.API.Features;
using UnityEngine;
using Random = System.Random;

namespace SnivysServerEvents.Events;
public class NameRedactedEventHandlers
{
    private static NameRedactedConfig _config;
    private static bool _nreStarted;
    public NameRedactedEventHandlers()
    {
        if (_nreStarted) return;
        _config = Plugin.Instance.Config.NameRedactedConfig;
        Plugin.ActiveEvent += 1;
        Start();
    }

    private static void Start()
    {
        _nreStarted = true;
        Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        foreach (Player player in Player.List)
            player.DisplayNickname = _config.NameRedactedName;
    }

    
    public static void EndEvent()
    {
        if (!_nreStarted) return;
        _nreStarted = false;
        Plugin.ActiveEvent -= 1;
        foreach (Player player in Player.List)
            player.DisplayNickname = player.Nickname;
    }
}