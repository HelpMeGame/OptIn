using System;
using Exiled.API.Enums;
using Exiled.API.Features;

using _Server = Exiled.Events.Handlers.Server;
using _Player = Exiled.Events.Handlers.Player;

namespace OptIn
{
    public class OptIn : Plugin<Config>
    {
        private static readonly Lazy<OptIn> lazyInstance = new Lazy<OptIn>(() => new OptIn());
        public static OptIn Instance => lazyInstance.Value;

        public override PluginPriority Priority { get; } = PluginPriority.Default;
        
        private EventHandlers handlers;


        private OptIn()
        {
        }

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

            _Player.Joined += handlers.OnPlayerJoin;
            _Server.RoundStarted += handlers.OnRoundStarted;
            _Server.RestartingRound += Preferences.SavePreferencesToFile;
        }

        public void UnregisterEvents()
        {
            _Player.Joined -= handlers.OnPlayerJoin;
            _Server.RoundStarted -= handlers.OnRoundStarted;
            _Server.RestartingRound -= Preferences.SavePreferencesToFile;

            handlers = null;
        }
    }
}
