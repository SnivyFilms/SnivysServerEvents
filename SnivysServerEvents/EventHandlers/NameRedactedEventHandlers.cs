using SnivysServerEvents.Configs;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerAPI = Exiled.API.Features.Player;
using PlayerEvent = Exiled.Events.Handlers.Player;

namespace SnivysServerEvents.EventHandlers;
public class NameRedactedEventHandlers
{
    private static NameRedactedConfig _config;
    private static bool _nreStarted;
    public NameRedactedEventHandlers()
    {
        if (_nreStarted) return;
        _config = Plugin.Instance.Config.NameRedactedConfig;
        Plugin.ActiveEvent += 1;
        PlayerEvent.Verified += OnVerified;
        Start();
    }

    private static void Start()
    {
        _nreStarted = true;
        Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        foreach (PlayerAPI player in PlayerAPI.List)
            player.DisplayNickname = _config.NameRedactedName;
    }

    private static void OnVerified(VerifiedEventArgs ev)
    {
        ev.Player.DisplayNickname = _config.NameRedactedName;
    }
    
    public static void EndEvent()
    {
        if (!_nreStarted) return;
        _nreStarted = false;
        Plugin.ActiveEvent -= 1;
        PlayerEvent.Verified -= OnVerified;
        foreach (PlayerAPI player in PlayerAPI.List)
            player.DisplayNickname = player.Nickname;
    }
}