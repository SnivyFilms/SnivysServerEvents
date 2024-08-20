using System.ComponentModel;

namespace SnivysServerEvents.Configs;

public class FreezingTemperaturesConfig
{
    public string StartEventCassieMessage { get; set; } = "pitch_0.5 .g7 .g7 .g7 X K end of the world event detected";
    public string StartEventCassieText { get; set; } = "XK end-of-the-world event detected. (173 cloning event)";
    
    [Description("How long before the Light Containment Zone Time Warning")]
    public float LightTimeWarning { get; set; } = 300f;
    
    [Description("How long before Light Containment Zone gets completely frozen over in seconds after the Time Warning")]
    public float LightCompleteFreezeTime { get; set; } = 300f;

    [Description("What should the half time remaining warning be for Light Containment Zone?")]
    public string LightHalfTimeRemainingWarningMessage { get; set; } = "5 Minutes before Light Containment Zone Temperature Failure";
    
    [Description("What should the half time remaining warning be for CASSIE's text in Heavy Containment Zone?")]
    public string LightHalfTimeRemainingWarningText { get; set; } = "5 Minutes before Light Containment Zone Temperature Failure";

    [Description("What should CASSIE say when Light gets frozen over?")]
    public string LightFrozenOverMessage { get; set; } = "Light Containment Zone has been locked down";
    
    [Description("What should CASSIE's test say when Light gets frozen over?")]
    public string LightFrozenOverText { get; set; } = "Light Containment Zone has been locked down";
    
    [Description("How long before the Heavy Containment Zone Time Warning")]
    public float HeavyTimeWarning { get; set; } = 150f;
    
    [Description("How long before Heavy Containment Zone gets completely frozen over in seconds AFTER Light Containment Zone's Freeze Over after the Time Warning")]
    public float HeavyCompleteFreezeTime { get; set; } = 150f;
    
    [Description("What should the half time remaining warning be for Heavy Containment Zone?")]
    public string HeavyHalfTimeRemainingWarningMessage { get; set; } = "2 Minutes 30 Seconds before Heavy Containment Zone Temperature Failure";
    
    [Description("What should the half time remaining warning be for CASSIE's text in Heavy Containment Zone?")]
    public string HeavyHalfTimeRemainingWarningText { get; set; } = "2 Minutes 30 Seconds before Heavy Containment Zone Temperature Failure";
    
    [Description("What should CASSIE say when Heavy gets frozen over?")]
    public string HeavyFrozenOverMessage { get; set; } = "Heavy Containment Zone has been locked down";
    
    [Description("What should CASSIE's test say when Heavy gets frozen over?")]
    public string HeavyFrozenOverText { get; set; } = "Heavy Containment Zone has been locked down";
    
    [Description("How long before the Entrance Zone Time Warning")]
    public float EntranceTimeWarning { get; set; } = 150f;
    
    [Description("How long before Entrance Zone gets completely frozen over in seconds AFTER Heavy Containment Zone's Freeze Over after the Time Warning")]
    public float EntranceCompleteFreezeTime { get; set; } = 150f;
    
    [Description("What should the half time remaining warning be for CASSIE in Entrance Zone?")]
    public string EntranceHalfTimeRemainingWarningMessage { get; set; } = "2 Minutes 30 Seconds before Entrance Zone Temperature Failure";
    
    [Description("What should the half time remaining warning be for CASSIE's text in Entrance Zone?")]
    public string EntranceHalfTimeRemainingWarningText { get; set; } = "2 Minutes 30 Seconds before Entrance Zone Temperature Failure";
    
    [Description("What should CASSIE say when Entrance gets frozen over?")]
    public string EntranceFrozenOverMessage { get; set; } = "Entrance Zone has been locked down";
    
    [Description("What should CASSIE's test say when Entrance gets frozen over?")]
    public string EntranceFrozenOverText { get; set; } = "Entrance Zone has been locked down";

    [Description(
        "How long in seconds after the zones lock does it take for the player to die (In the event that they are able to survive for whatever reason in that zone)")]
    public float KillPlayersInZoneAfterTime { get; set; } = 10f;

    [Description("What would the player's death reason be if they get killed by the server?")]
    public string PlayersDeathReason { get; set; } = "You have froze to death";

}