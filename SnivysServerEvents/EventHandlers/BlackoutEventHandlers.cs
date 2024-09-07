﻿using Exiled.API.Features;
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
            if (_boeStarted) return;
            _config = Plugin.Instance.Config.BlackoutConfig;
            Plugin.ActiveEvent += 1;
            if (_config.GeneratorEndsEvent)
            {
                Maps.GeneratorActivating += Plugin.Instance.EventHandlers.OnGeneratorEngagedBOE;
            }
            Start();
        }

        private static void Start()
        {
            _boeStarted = true;
            Map.TurnOffAllLights(432000);
            foreach (var player in Player.List)
            {
                player.AddItem(ItemType.Lantern);
            }

            Cassie.MessageTranslated(_config.StartEventCassieMessage, _config.StartEventCassieText);
        }

        public static void EndEvent()
        {
            if (!_boeStarted) return;
            Cassie.MessageTranslated(_config.EndEventCassieMessage, _config.EndEventCassieText);
            if (_config.GeneratorEndsEvent)
            {
                Maps.GeneratorActivating -= Plugin.Instance.EventHandlers.OnGeneratorEngagedBOE;
            }
            Map.TurnOffAllLights(1);
            _boeStarted = false;
            Plugin.ActiveEvent -= 1;
        }
    }
}