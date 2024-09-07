using System.Collections.Generic;
using SnivysServerEvents.Configs;
using Exiled.API.Features;

namespace SnivysServerEvents.EventHandlers
{
    internal class ShortEventHandlers
    {
        private static ShortConfig _config;
        private static bool _seStarted;
        
        public ShortEventHandlers()
        {
            if (_seStarted) return;
            _config = Plugin.Instance.Config.ShortConfig;
            Plugin.ActiveEvent += 1;
            Exiled.Events.Handlers.Player.ChangingRole += Plugin.Instance.EventHandlers.OnRoleSwapSE;
            Start();
        }

        private static void Start()
        {
            _seStarted = true;
            foreach (var player in Player.List)
            {
                var startingItems = GetStartingItems(_config.StartingItems);
                foreach (var item in startingItems)
                {
                    Log.Debug($"Adding {item} to {player}");
                    player.AddItem(item);
                }
                player.Scale = new UnityEngine.Vector3(GetPlayerSize(), GetPlayerSize(), GetPlayerSize());
                Log.Debug($"Set {player} size to {GetPlayerSize()}");
            }
            Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        }

        public static float GetPlayerSize()
        {
            return _config.PlayerSize;
        }

        private static List<ItemType> GetStartingItems(List<ItemType> items)
        {
            return items;
        }
        public static void EndEvent()
        {
            if (!_seStarted) return;
            Cassie.MessageTranslated(_config.EndEventCassieMessage, _config.EndEventCassieText);
            Log.Debug("Unregistering ChangingRole (SE) Event Handlers");
            Exiled.Events.Handlers.Player.ChangingRole -= Plugin.Instance.EventHandlers.OnRoleSwapSE;
            _seStarted = false;
            Plugin.ActiveEvent -= 1;
        }
    }
}
