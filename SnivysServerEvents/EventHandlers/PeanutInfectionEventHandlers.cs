using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;
using SnivysServerEvents.Configs;

namespace SnivysServerEvents.EventHandlers
{
    public class PeanutInfectionEventHandlers
    {
        private static PeanutInfectionConfig _config;
        private static bool _pieStarted;
        public PeanutInfectionEventHandlers()
        {
            Log.Debug("Checking if Peanut Infection Event has already started");
            if (_pieStarted) return;
            _config = Plugin.Instance.Config.PeanutInfectionConfig;
            Plugin.ActiveEvent += 1;
            Log.Debug("Adding Player Died Event PIE Handler");
            Player.Died += Plugin.Instance.EventHandlers.OnKillingPIE;
            _pieStarted = true;
            Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        }

        public static void EndEvent()
        {
            if (!_pieStarted) return;
            Cassie.MessageTranslated(_config.EndEventCassieMessage, _config.EndEventCassieText);
            Log.Debug("Removing Player Died Event PIE Handler");
            Player.Died -= Plugin.Instance.EventHandlers.OnKillingPIE;
            _pieStarted = false;
            Plugin.ActiveEvent -= 1;
        }
    }
}