using System;
using Exiled.API.Interfaces;
using Exiled.API.Features;
using System.ComponentModel;

namespace OptIn
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("The place where player preferences will be saved.")]
        public string preferencesLocation { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\EXILED\\Configs\\{Server.Port}-playerPreferences.json";

    }
}
