using System;
using System.Collections.Generic;
using CommandSystem;
using RemoteAdmin;

using Exiled.API.Features;

namespace OptIn.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    class PriorityCommand : ICommand
    {
        public string Command { get; } = "Priority";

        public string[] Aliases { get; } = { "pr" };

        public string Description { get; } = "Change the priority of certain roles";

        private List<string> SCPArgs = new List<string>(new string[] { "scp", "monster" });
        private List<string> DClassArgs = new List<string>(new string[] { "d-class", "dclass", "dboi", "d-boi", "d-boy", "dboy" });
        private List<string> GuardArgs = new List<string>(new string[] { "guard", "facility" });
        private List<string> ScientistArgs = new List<string>(new string[] { "scientist" });
        private string[] numToRole = { "SCP", "DClass", "Guard", "Scientist" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender player)
            {
                PlayerPreference playerPreference = Preferences.GetPlayerPreference(player.SenderId);

                if (arguments.Array.Length == 1)
                {
                    List<int> order = playerPreference.priorityOrder;
                    response = $"\nPriority 1 - {numToRole[order[0]-1]}\nPriority 2 - {numToRole[order[1]-1]}\nPriority 3 - {numToRole[order[2]-1]}\nPriority 4 - {numToRole[order[3]-1]}\n";
                    return true;
                }
                else if (arguments.Array.Length == 2)
                {
                    response = $"You missed an argument. The command should look like '.priority <role> <priority (1-4)>'";
                    return false;
                }

                int priority;
                try
                {
                    priority = int.Parse(arguments.Array[2]) - 1;
                }
                catch (FormatException)
                {
                    response = $"The argument \"{arguments.Array[2]}\" is not a number.";
                    return false;
                }


                response = "\n";
                string arg = arguments.Array[1].ToLower();
                int newNum;
                if (SCPArgs.Contains(arg)) { newNum = 1; response = $"SCP is now priority {priority + 1}"; }
                else if (DClassArgs.Contains(arg)) { newNum = 2; response = $"DClass is now priority {priority + 1}"; }
                else if (GuardArgs.Contains(arg)) { newNum = 3; response = $"Guard is now priority {priority + 1}"; }
                else if (ScientistArgs.Contains(arg)) { newNum = 4; response = $"Scientist is now priority {priority + 1}"; }
                else
                {
                    response = String.Concat(response, $"Argument \"{arg}\" is not a recognized role.\n");
                    return false;
                }

                int oldNum = playerPreference.priorityOrder[priority];

                int oldPriority = playerPreference.priorityOrder.IndexOf(newNum);

                playerPreference.priorityOrder[priority] = newNum;
                playerPreference.priorityOrder[oldPriority] = oldNum;

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
