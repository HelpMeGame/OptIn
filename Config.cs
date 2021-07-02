using System;
using Exiled.API.Interfaces;
using Exiled.API.Features;
using Exiled.API.Enums;
using System.ComponentModel;

namespace OptIn
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        [Description("Enable/Disable basic alerts to actions by OptIn")]
        public bool DebugLogs { get; set; } = false;

        [Description("The place where player preferences will be saved.")]
        public string preferencesLocation { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\EXILED\\Configs\\{Server.Port}-playerPreferences.json";

    }
}
