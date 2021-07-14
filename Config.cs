using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
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
        public string preferencesLocation { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $"Exiled\\Configs\\{Server.Port}-playerPreferences.json";

        [Description("Changes the way roles are chosen to make it more random.")]
        public bool useAlgorithmMode { get; set; }  = false;

        [Description("The priority to swap roles. SCP -> 1, DClass -> 2, Guard -> 3, Scientist -> 4. Leave as \"null\" for random. Only occurs when using the algorithm mode.")]
        public int[] swapPriority { get; set; } = { 1, 2, 3, 4 };
    }
}