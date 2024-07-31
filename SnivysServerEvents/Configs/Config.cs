using System.ComponentModel;
using Exiled.API.Interfaces;

namespace SnivysServerEvents.Configs
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Debug Printouts")] 
        public bool Debug { get; set; } = false;
        //To do indepentent events configs
        public BlackoutConfig BlackoutConfig { get; set; } = new();
        public PeanutInfectionConfig PeanutInfectionConfig { get; set; } = new();
        public PeanutHydraConfig PeanutHydraConfig { get; set; } = new();
    }
}