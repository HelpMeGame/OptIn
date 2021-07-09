using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;

using _Server = Exiled.Events.Handlers.Server;
using _Player = Exiled.Events.Handlers.Player;

namespace OptIn
{
    public class OptIn : Plugin<Config>
    {
        public override string Author => "HelpMeGame";
        public override Version Version => new Version(1, 0, 2);
        public override Version RequiredExiledVersion => new Version(2,8,0);

        public static OptIn singleton { get; } = new OptIn();

        public override PluginPriority Priority { get; } = PluginPriority.Default;
        
        private EventHandlers handlers;


        private OptIn(){}

        public override void OnEnabled()
        {
            RegisterEvents();
            Preferences.GetPreferencesFromFile();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
        }

        public void RegisterEvents()
        {
            handlers = new EventHandlers();

            _Server.RoundStarted += handlers.OnRoundStarted;
            _Server.RestartingRound += Preferences.SavePreferencesToFile;
        }

        public void UnregisterEvents()
        {
            _Server.RoundStarted -= handlers.OnRoundStarted;
            _Server.RestartingRound -= Preferences.SavePreferencesToFile;

            handlers = null;
        }

    }
}
