using System.ComponentModel;

namespace SnivysServerEvents.Configs;

public class VariableLightsConfig
{
    public string StartEventCassieMessage { get; set; } = "Facility Power Surge Detected";
    public string StartEventCassieText { get; set; } = "Facility Power Surge Detected (Lights variable event)";
    
    [Description("Determines how long before the lights change")]
    public float TimeForChange { get; set; } = 20;

    [Description("Should the lights only brighten and dim or allow for color changing too")]
    public bool ColorChanging { get; set; } = true;
}