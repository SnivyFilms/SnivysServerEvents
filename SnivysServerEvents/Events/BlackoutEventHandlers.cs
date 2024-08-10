using Exiled.API.Features;
using Maps = Exiled.Events.Handlers.Map;
using SnivysServerEvents.Configs;

namespace SnivysServerEvents.Events
{
    public class BlackoutEventHandlers
    {
        private static BlackoutConfig _config;
        private static bool _boeStarted;
        public BlackoutEventHandlers()
        {
            _config = Plugin.Instance.Config.BlackoutConfig;
            Plugin.ActiveEvent += 1;
            Maps.GeneratorActivating += Plugin.Instance.eventHandlers.OnGeneratorEngagedBOE;
            Start();
        }

        public static void Start()
        {
            _boeStarted = true;
            Map.TurnOffAllLights(3600);
            foreach (var player in Player.List)
            {
                player.AddItem(ItemType.Lantern);
            }

            Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        }

        public static void EndEvent()
        {
            if (_boeStarted)
            {
                Cassie.MessageTranslated(_config.EndEventCassieMessage, _config.EndEventCassieText);
                Maps.GeneratorActivating -= Plugin.Instance.eventHandlers.OnGeneratorEngagedBOE;
                Map.TurnOffAllLights(1);
                _boeStarted = false;
            }
        }
    }
}