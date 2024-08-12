
namespace SnivysServerEvents.Configs
{
    public class ShortConfig
    {
        public string StartEventCassieMessage { get; set; } = "Small Humans Test Activation";
        public string StartEventCassieText { get; set; } = "Small Humans Test Activation (Short Event Started)";

        public string EndEventCassieMessage { get; set; } = "Small Humans Test Deactivation";
        public string EndEventCassieText { get; set; } = "Small Humans Test Deactivation (Short Event Ended)";
    }
}
