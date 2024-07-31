namespace SnivysServerEvents.Configs
{
    public class BlackoutConfig
    {
        public string StartEventCassieMessage { get; set; } = "Power System Failure";
        public string StartEventCassieText { get; set; } = "Power System Failure. (Blackout event started)";

        public string EndEventCassieMessage { get; set; } = "Power System Restored";
        public string EndEventCassieText { get; set; } = "Power System Restored. (Blackout event ended)";
    }
}