using System.ComponentModel;

namespace SnivysServerEvents.Configs;

public class PeanutHydraConfig
{
    public string StartEventCassieMessage { get; set; } = "pitch_0.5 .g7 .g7 .g7 X K end of the world event detected";
    public string StartEventCassieText { get; set; } = "XK end-of-the-world event detected. (173 cloning event)";
    
    [Description("If no spectators can be found, use the player that killed 173 instead")]
    public bool UseAttackersIfNeeded { get; set; } = true;
}