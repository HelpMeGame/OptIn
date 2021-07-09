using System;
using System.Collections.Generic;
using CommandSystem;
using RemoteAdmin;

namespace OptIn.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    class OptInCommand : ICommand
    {
        public string Command { get; } = "OptIn";

        public string[] Aliases { get; } = { "optins", "oi" };

        public string Description { get; } = "Opt in or out of certain roles";

        private List<string> SCPArgs = new List<string>(new string[] { "scp", "monster" });
        private List<string> DClassArgs = new List<string>(new string[] { "d-class", "dclass", "dboi", "d-boi", "d-boy", "dboy" });
        private List<string> GuardArgs = new List<string>(new string[] { "guard", "facility" });
        private List<string> ScientistArgs = new List<string>(new string[] { "scientist" });


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender player)
            {
                PlayerPreference playerPreference = Preferences.GetPlayerPreference(player.SenderId);

                if (arguments.Array.Length <= 1)
                {
                    response = $"\nQueue for SCP: {playerPreference.wantsSCP}\nQueue for DClass: {playerPreference.wantsDClass}\nQueue for Guard: {playerPreference.wantsGaurd}\nQueue for Scientist: {playerPreference.wantsScientist}";
                    return true;
                }

                response = "\n";

                for (int i = 1; i < arguments.Array.Length; i++)
                {
                    string arg = arguments.Array[i].ToLower();
                    if (SCPArgs.Contains(arg))
                    {
                        // Change SCP Stuff
                        if (playerPreference.wantsSCP) { playerPreference.wantsSCP = false; } else { playerPreference.wantsSCP = true; }
                        response = String.Concat(response, $"Queue for a SCP is now {playerPreference.wantsSCP}.\n");
                    }
                    else if (DClassArgs.Contains(arg))
                    {
                        // Change DClass Stuff
                        if (playerPreference.wantsDClass) { playerPreference.wantsDClass = false; } else { playerPreference.wantsDClass = true; }
                        response = String.Concat(response, $"Queue for DClass is now {playerPreference.wantsDClass}.\n");

                    }
                    else if (GuardArgs.Contains(arg))
                    {
                        // Change Gaurd Stuff
                        if (playerPreference.wantsGaurd) { playerPreference.wantsGaurd = false; } else { playerPreference.wantsGaurd = true; }
                        response = String.Concat(response, $"Queue for Gaurd role is now {playerPreference.wantsGaurd}.\n");

                    }
                    else if (ScientistArgs.Contains(arg))
                    {
                        // Change Scientist Stuff
                        if (playerPreference.wantsScientist) { playerPreference.wantsScientist = false; } else { playerPreference.wantsScientist = true; }
                        response = String.Concat(response, $"Queue for Scientist role is now {playerPreference.wantsScientist}.\n");

                    }
                    else
                    {
                        response = String.Concat(response, $"Argument \"{arg}\" is not a recognized role.\n");
                        return false;
                    }
                }
                Preferences.ChangePreference(playerPreference);
                return true;
            }
            else
            {
                response = "This command can not be used from the console.";
                return false;
            }
        }
    }
}
