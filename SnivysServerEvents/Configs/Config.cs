using System.ComponentModel;
using System.IO;
using Exiled.API.Interfaces;

namespace SnivysServerEvents.Configs
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Debug Printouts")] 
        public bool Debug { get; set; } = false;
        
        //Independent Event Configs
        public BlackoutConfig BlackoutConfig { get; set; } = new();
        public ChaoticConfig ChaoticConfig { get; set; } = new();
        public FreezingTemperaturesConfig FreezingTemperaturesConfig { get; set; } = new();
        public NameRedactedConfig NameRedactedConfig { get; set; } = new();
        public PeanutInfectionConfig PeanutInfectionConfig { get; set; } = new();
        public PeanutHydraConfig PeanutHydraConfig { get; set; } = new();
        public ShortConfig ShortConfig { get; set; } = new();
        public VariableLightsConfig VariableLightsConfig { get; set; } = new();
    }
}