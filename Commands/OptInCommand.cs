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

        private List<string> SCPArgs = new List<string>(new string[] {"scp", "monster"});
        private List<string> DClassArgs = new List<string>(new string[] { "d-class","dclass","dboi","d-boi","d-boy","dboy" });
        private List<string> GaurdArgs = new List<string>(new string[] { "gaurd","facility" });
        private List<string> ScientistArgs = new List<string>(new string[] { "scientist" });


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender player)
            {
                PlayerPreference playerOptIns = Preferences.GetPlayerPreference(player.SenderId);

                if (arguments.Array.Length <= 1)
                {
                    response = $"\nQueue for SCP: {playerOptIns.wantsSCP}\nQueue for DClass: {playerOptIns.wantsDClass}\nQueue for Gaurd: {playerOptIns.wantsGaurd}\nQueue for Scientist: {playerOptIns.wantsScientist}";
                    return true;
                }

                response = "\n";

                for (int i = 1; i < arguments.Array.Length; i++)
                {
                    string arg = arguments.Array[i].ToLower();
                    if (SCPArgs.Contains(arg))
                    {
                        // Change SCP Stuff
                        if (playerOptIns.wantsSCP) { playerOptIns.wantsSCP = false; } else { playerOptIns.wantsSCP = true; }
                        response = String.Concat(response, $"Queue for a SCP is now {playerOptIns.wantsSCP}.\n");
                    }
                    else if (DClassArgs.Contains(arg))
                    {
                        // Change DClass Stuff
                        if (playerOptIns.wantsDClass) { playerOptIns.wantsDClass = false; } else { playerOptIns.wantsDClass = true; }
                        response = String.Concat(response, $"Queue for DClass is now {playerOptIns.wantsDClass}.\n");

                    }
                    else if (GaurdArgs.Contains(arg))
                    {
                        // Change Gaurd Stuff
                        if (playerOptIns.wantsGaurd) { playerOptIns.wantsGaurd = false; } else { playerOptIns.wantsGaurd = true; }
                        response = String.Concat(response, $"Queue for Gaurd role is now {playerOptIns.wantsGaurd}.\n");

                    }
                    else if (ScientistArgs.Contains(arg))
                    {
                        // Change Scientist Stuff
                        if (playerOptIns.wantsScientist) { playerOptIns.wantsScientist = false; } else { playerOptIns.wantsScientist = true; }
                        response = String.Concat(response, $"Queue for Scientist role is now {playerOptIns.wantsScientist}.\n");

                    }
                    else
                    {
                        response = String.Concat(response,$"Argument \"{arg}\" is not a recognized role.\n");
                    }
                }
                Preferences.ChangePreference(playerOptIns);
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
