﻿using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;

namespace SnivysServerEvents.Configs
{
    public class ShortConfig
    {
        public string StartEventCassieMessage { get; set; } = "Small Humans Test Activation";
        public string StartEventCassieText { get; set; } = "Small Humans Test Activation (Short Event Started)";

        public string EndEventCassieMessage { get; set; } = "Small Humans Test Deactivation";
        public string EndEventCassieText { get; set; } = "Small Humans Test Deactivation (Short Event Ended)";

        [Description("Determines how small each player is")]
        public float PlayerSize { get; set; } = 0.25f;

        /*[Description("What item does the players start with?")]
        public List<string> StartingItem { get; set; } = new()
        {
            $"{ItemType.KeycardJanitor}"
        };*/
    }
}
