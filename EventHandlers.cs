using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using System.Threading;
using Exiled.Events.EventArgs;
using Exiled.API.Features;

namespace OptIn
{
    internal class EventHandlers
    {
        Config config = OptIn.singleton.Config;
        private static Random rng;
        private List<RoleType> allSCPRoles = new List<RoleType>(new RoleType[] { RoleType.Scp049, RoleType.Scp0492, RoleType.Scp079, RoleType.Scp096, RoleType.Scp106, RoleType.Scp173, RoleType.Scp93953, RoleType.Scp93989 });


        public void OnRoundStarted()
        {
            rng = new Random();
            List<Player> players = Player.List.ToList();

            List<PlayerPreference> preferences = Preferences.GetPlayerPreferences(players);

            if (config.useAlgorithmMode)
            {
                preferences.Shuffle(rng);

                int[] preferenceOrder = OptIn.singleton.Config.swapPriority;
                if (preferenceOrder == null)
                {
                    preferenceOrder = new int[] { 1, 2, 3, 4 };
                    preferenceOrder.Shuffle(rng);
                }

                // As long as there is two players in the list, loop through the avaialable options.
                while (preferences.Count > 1)
                {
                    PlayerPreference currentPreference = preferences[0];
                    bool swapRole = false;

                    // Loop through players and choose a replacement
                    for (int selectedPreference = 1; selectedPreference < preferences.Count - 1; selectedPreference++)
                    {
                        // Priority Randomizer/Definer. Used to excute the priority of role swap.
                        for (int i = 0; i < preferenceOrder.Length; i++)
                        {
                            if (preferenceOrder[i] == 1)
                            {
                                if (allSCPRoles.Contains(currentPreference.player.Role) && !currentPreference.wantsSCP)
                                {
                                    if (preferences[selectedPreference].wantsSCP)
                                    {
                                        swapRole = true;
                                    }
                                }
                            }
                            else if (preferenceOrder[i] == 2)
                            {
                                if (currentPreference.player.Role == RoleType.ClassD && !currentPreference.wantsGaurd)
                                {
                                    if (preferences[selectedPreference].wantsGaurd)
                                    {
                                        swapRole = true;
                                    }
                                }
                            }
                            else if (preferenceOrder[i] == 3)
                            {
                                if (currentPreference.player.Role == RoleType.ClassD && !currentPreference.wantsDClass)
                                {
                                    if (preferences[selectedPreference].wantsDClass)
                                    {
                                        swapRole = true;
                                    }
                                }
                            }
                            else if (preferenceOrder[i] == 4)
                            {
                                if (currentPreference.player.Role == RoleType.ClassD && !currentPreference.wantsScientist)
                                {
                                    if (preferences[selectedPreference].wantsScientist)
                                    {
                                        swapRole = true;
                                    }
                                }
                            }

                            if (swapRole) { break; }
                        }


                        if (swapRole)
                        {
                            RoleType temp = preferences[selectedPreference].player.Role;
                            preferences[selectedPreference].player.SetRole(currentPreference.player.Role);
                            currentPreference.player.SetRole(temp);
                            preferences[selectedPreference].player.Health = preferences[selectedPreference].player.MaxHealth;
                            currentPreference.player.Health = currentPreference.player.MaxHealth;
                            preferences.RemoveAt(selectedPreference);
                            break;
                        }
                    }
                    preferences.RemoveAt(0);
                }
            }
            else
            {
                int scpCount = 0, dclassCount = 0, guardCount = 0, scientistCount = 0;
                List<RoleType> activeSCPRoles = new List<RoleType>();
                foreach (Player player in players)
                {
                    if (allSCPRoles.Contains(player.Role)) { scpCount += 1; activeSCPRoles.Add(player.Role); }
                    else if (player.Role == RoleType.ClassD) { dclassCount += 1; }
                    else if (player.Role == RoleType.FacilityGuard) { guardCount += 1; }
                    else if (player.Role == RoleType.Scientist) { scientistCount += 1; }
                }

                for (int i = 1; i < 5; i++)
                {
                    Dictionary<Player, double> roleChance = new Dictionary<Player, double>();
                    int playersPriority1 = 0;
                    int playersPriority2 = 0;
                    int playersPriority3 = 0;
                    int playersPriority4 = 0;


                    //preferences.Where(x => x.priorityOrder[0] == i).Count();

                    foreach (PlayerPreference player in preferences)
                    {
                        if (player.priorityOrder[0] == i) { playersPriority1 += 1; }
                        else if (player.priorityOrder[1] == i) { playersPriority2 += 1; }
                        else if (player.priorityOrder[2] == i) { playersPriority3 += 1; }
                        else if (player.priorityOrder[3] == i) { playersPriority4 += 1; }
                    }
                    foreach (PlayerPreference player in preferences)
                    {
                        if (player.priorityOrder[0] == i) { roleChance.Add(player.player, 0.8 / playersPriority1); }
                        else if (player.priorityOrder[1] == i) { roleChance.Add(player.player, 0.2 / playersPriority2); }
                        else if (player.priorityOrder[2] == i) { roleChance.Add(player.player, 0.07 / playersPriority3); }
                        else if (player.priorityOrder[3] == i) { roleChance.Add(player.player, 0.03 / playersPriority4); }
                    }

                    int count = 0;
                    RoleType roleToAssign = RoleType.None;

                    if (i == 1) { count = scpCount; roleToAssign = RoleType.None; }
                    else if (i == 2) { count = dclassCount; roleToAssign = RoleType.ClassD; }
                    else if (i == 3) { count = guardCount; roleToAssign = RoleType.FacilityGuard; }
                    else if (i == 4) { count = scientistCount; roleToAssign = RoleType.Scientist; }

                    if (config.DebugLogs)
                    {
                        Log.Warn($"Iteration for Role {roleToAssign}.");

                        Log.Warn($"Priorities: 1 ({playersPriority1}) 2 ({playersPriority2}) 3 ({playersPriority3}) 4 ({playersPriority4})");


                        foreach (var val in roleChance)
                        {
                            Log.Warn($" - User: {val.Key} Chance: {val.Value}");
                        }
                    }

                    List<Player> addToRole = new List<Player>();

                    for (int j = 0; j < count; j++)
                    {
                        Player temp = Tools.RandomPlayerWeightedKey(roleChance);
                        if (config.DebugLogs) { Log.Warn($"Role chosen for user: {temp}"); }
                        roleChance.Remove(temp);
                        addToRole.Add(temp);
                    }

                    foreach (Player player in addToRole)
                    {
                        if (roleToAssign != RoleType.None)
                        {
                            player.SetRole(roleToAssign);
                        }
                        else
                        {
                            player.SetRole(activeSCPRoles[0]);
                            activeSCPRoles.RemoveAt(0);
                        }
                        player.Health = player.MaxHealth;
                        preferences.Remove(preferences.Where(y => y.userID == player.UserId).FirstOrDefault());
                    }

                }
            }
        }
    }

    public static class Tools
    {
        public static void Shuffle<T>(this IList<T> list, Random rng)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        static public int NextInt(Random random, double max)
        {
            return random.Next((int)max);
        }

        public static Player RandomPlayerWeightedKey(Dictionary<Player, double> weightedDictionary)
        {
            Random random = new Random();
            Player result = null;
            double totalWeight = 0;

            foreach (var item in weightedDictionary)
                totalWeight += item.Value;

            double randNumber = NextInt(random, totalWeight);

            foreach (var item in weightedDictionary)
            {
                var value = item.Value;

                if (randNumber >= value)
                {
                    randNumber -= value;
                }
                else
                {
                    result = item.Key;
                    break;
                }
            }
            return result;
        }
    }

}


