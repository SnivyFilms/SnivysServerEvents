using SnivysServerEvents.Configs;
using Exiled.API.Features;

namespace SnivysServerEvents.Events
{
    internal class ShortEventHandlers
    {
        private static ShortConfig _config;
        private static bool _seStarted;
        
        public ShortEventHandlers()
        {
            _config = Plugin.Instance.Config.ShortConfig;
            Plugin.ActiveEvent += 1;
            Exiled.Events.Handlers.Player.ChangingRole += Plugin.Instance.eventHandlers.OnRoleSwapSE;
            Start();
        }
        public static void Start()
        {
            _seStarted = true;
            foreach (var player in Player.List)
            {
                player.AddItem(ItemType.KeycardJanitor);
                player.Scale = new UnityEngine.Vector3(0.25f, 0.25f, 0.25f);
            }
            Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        }
        public static void EndEvent()
        {
            if (_seStarted)
            {
                Cassie.MessageTranslated(_config.EndEventCassieMessage, _config.EndEventCassieText);
                Exiled.Events.Handlers.Player.ChangingRole -= Plugin.Instance.eventHandlers.OnRoleSwapSE;
                _seStarted = false;
            }
        }
    }
}
