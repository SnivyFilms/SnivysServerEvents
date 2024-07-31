namespace SnivysServerEvents.Configs;

public class PeanutInfectionConfig
{
    public string StartEventCassieMessage { get; set; } = "SCP 1 7 3 plague detected";
    public string StartEventCassieText { get; set; } = "SCP-173 plague detected. (173 Infection Event Started)";
    public string EndEventCassieMessage { get; set; } = "SCP 1 7 3 plague contained";
    public string EndEventCassieText { get; set; } = "SCP-173 plague contained? (173 Infection Event Ended)";
}