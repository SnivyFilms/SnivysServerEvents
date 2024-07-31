/*using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;
using SnivysServerEvents.Configs;

namespace SnivysServerEvents.Events
{
    public class PeanutHydraEventHandlers
    {
        private static PeanutHydraConfig _config;
        private static bool _pheStarted;
        public PeanutHydraEventHandlers()
        {
            _config = new PeanutHydraConfig();
            Player.Dying += Plugin.Instance.eventHandlers.OnDyingPHE;
            Player.Died += Plugin.Instance.eventHandlers.OnDiedPHE;
            Start();
        }

        public static void Start()
        {
            _pheStarted = true;
            Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        }

        public static void EndEvent()
        {
            if (_pheStarted)
            {
                Player.Dying += Plugin.Instance.eventHandlers.OnDyingPHE;
                Player.Died -= Plugin.Instance.eventHandlers.OnDiedPHE;
                _pheStarted = false;
            }
        }
    }
}*/