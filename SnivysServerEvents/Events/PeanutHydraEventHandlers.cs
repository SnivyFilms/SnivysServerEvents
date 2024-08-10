using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;
using SnivysServerEvents.Configs;

namespace SnivysServerEvents.Events
{
    
    public class PeanutHydraEventHandlers
    {
        public static PeanutHydraConfig Config;
        private static bool _pheStarted;
        public PeanutHydraEventHandlers()
        {
            Config = Plugin.Instance.Config.PeanutHydraConfig;
            Plugin.ActiveEvent += 1;
            Player.Dying += Plugin.Instance.eventHandlers.OnDyingPHE;
            Player.Died += Plugin.Instance.eventHandlers.OnDiedPHE;
            Start();
        }

        public static void Start()
        {
            _pheStarted = true;
            Cassie.MessageTranslated(Config.StartEventCassieMessage, Config.StartEventCassieText);
        }

        public static void EndEvent()
        {
            if (!_pheStarted) return;
            Player.Dying += Plugin.Instance.eventHandlers.OnDyingPHE;
            Player.Died -= Plugin.Instance.eventHandlers.OnDiedPHE;
            _pheStarted = false;
        }
    }
}