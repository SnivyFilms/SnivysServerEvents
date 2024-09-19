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
        Log.Debug("Checking if Name Redacted Event has already started");
        if (_nreStarted) return;
        _config = Plugin.Instance.Config.NameRedactedConfig;
        Plugin.ActiveEvent += 1;
        Log.Debug("Adding On Verified event handler");
        PlayerEvent.Verified += OnVerified;
        _nreStarted = true;
        Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        foreach (PlayerAPI player in PlayerAPI.List)
        {
            Log.Debug($"Setting {player} name to {_config.NameRedactedName}");
            player.DisplayNickname = _config.NameRedactedName;
        }
    }

    private static void OnVerified(VerifiedEventArgs ev)
    {
        Log.Debug($"Removing {ev.Player}'s name and giving them the name of {_config.NameRedactedName}");
        ev.Player.DisplayNickname = _config.NameRedactedName;
    }
    
    public static void EndEvent()
    {
        if (!_nreStarted) return;
        _nreStarted = false;
        Plugin.ActiveEvent -= 1;
        Log.Debug("Disabling the On Verified Event Handler");
        PlayerEvent.Verified -= OnVerified;
        foreach (PlayerAPI player in PlayerAPI.List)
        {
            Log.Debug($"Restoring {player} name");
            player.DisplayNickname = player.Nickname;
        }
    }
}