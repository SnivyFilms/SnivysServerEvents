using System.Collections.Generic;
using System.ComponentModel;

namespace SnivysServerEvents.Configs
{
    public class BlackoutConfig
    {
        public string StartEventCassieMessage { get; set; } = "Power System Failure";
        public string StartEventCassieText { get; set; } = "Power System Failure. (Blackout event started)";

        public string EndEventCassieMessage { get; set; } = "Power System Restored";
        public string EndEventCassieText { get; set; } = "Power System Restored. (Blackout event ended)";

        [Description("Should the Blackout Event end when all 3 generators activate?")]
        public bool GeneratorEndsEvent { get; set; } = true;

        [Description("What item should the player recieve when the blackout event starts?")]
        public List<ItemType> BlackoutEventStartingItem { get; set; } = [ItemType.Lantern];
    }
}