using Exiled.API.Features;
using SnivysServerEvents.Configs;
using Maps = Exiled.Events.Handlers.Map;

namespace SnivysServerEvents.EventHandlers
{
    public class BlackoutEventHandlers
    {
        private static BlackoutConfig _config;
        private static bool _boeStarted;
        public BlackoutEventHandlers()
        {
            Log.Debug("Checking if Blackout Event has already started");
            if (_boeStarted) return;
            _config = Plugin.Instance.Config.BlackoutConfig;
            Plugin.ActiveEvent += 1;
            if (_config.GeneratorEndsEvent)
            {
                Log.Debug("Enabling the Generator Activation Event Handlers");
                Maps.GeneratorActivating += Plugin.Instance.EventHandlers.OnGeneratorEngagedBOE;
            }
            
            _boeStarted = true;
            Log.Debug("Turning off the lights");
            Map.TurnOffAllLights(432000);
            foreach (var player in Player.List)
            {
                Log.Debug($"Adding {_config.BlackoutEventStartingItem} to {player}");
                player.AddItem(_config.BlackoutEventStartingItem);
            }

            Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        }

        public static void EndEvent()
        {
            if (!_boeStarted) return;
            Cassie.MessageTranslated(_config.EndEventCassieMessage, _config.EndEventCassieText);
            if (_config.GeneratorEndsEvent)
            {
                Log.Debug("Disabling Generator Engaged Event Handler");
                Maps.GeneratorActivating -= Plugin.Instance.EventHandlers.OnGeneratorEngagedBOE;
            }
            Log.Debug("Turning on the lights");
            Map.TurnOffAllLights(1);
            _boeStarted = false;
            Plugin.ActiveEvent -= 1;
        }
    }
}