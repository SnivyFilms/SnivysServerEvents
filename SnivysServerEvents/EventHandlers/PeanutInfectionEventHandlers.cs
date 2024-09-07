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
            if (_pieStarted) return;
            _config = Plugin.Instance.Config.PeanutInfectionConfig;
            Plugin.ActiveEvent += 1;
            Player.Died += Plugin.Instance.EventHandlers.OnKillingPIE;
            Start();
        }

        private static void Start()
        {
            _pieStarted = true;
            Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        }

        public static void EndEvent()
        {
            if (!_pieStarted) return;
            Cassie.MessageTranslated(_config.EndEventCassieMessage, _config.EndEventCassieText);
            Player.Died -= Plugin.Instance.EventHandlers.OnKillingPIE;
            _pieStarted = false;
            Plugin.ActiveEvent -= 1;
        }
    }
}