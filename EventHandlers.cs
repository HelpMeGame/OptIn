using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Exiled.Events.EventArgs;
using Exiled.API.Features;

namespace OptIn
{
    class EventHandlers
    {
        private static Random rng = new Random();
        private List<RoleType> SCPRoles = new List<RoleType>(new RoleType[] {RoleType.Scp049,RoleType.Scp0492, RoleType.Scp079, RoleType.Scp096, RoleType.Scp106, RoleType.Scp173, RoleType.Scp93953, RoleType.Scp93989 });


        public void OnRoundStarted()
        {
            List<Player> players = Player.List.ToList();

            List<PlayerPreference> preferences = Preferences.GetPlayerPreferences(players);


            int n = preferences.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                PlayerPreference value = preferences[k];
                preferences[k] = preferences[n];
                preferences[n] = value;
            }

            while (preferences.Count > 1)
            {
                PlayerPreference currentPreference = preferences[0];
                bool swapRole = false;
                for (int i = 1; i < preferences.Count -1; i++)
                {
                    if (SCPRoles.Contains(currentPreference.player.Role) && !currentPreference.wantsSCP)
                    {
                        if (preferences[i].wantsSCP)
                        {
                            swapRole = true;
                        }
                    }
                    if (currentPreference.player.Role == RoleType.ClassD && !currentPreference.wantsGaurd && !swapRole)
                    {
                        if (preferences[i].wantsGaurd)
                        {
                            swapRole = true;
                        }
                    }
                    if (currentPreference.player.Role == RoleType.ClassD && !currentPreference.wantsDClass && !swapRole)
                    {
                        if (preferences[i].wantsDClass)
                        {
                            swapRole = true;
                        }
                    }
                    if (currentPreference.player.Role == RoleType.ClassD && !currentPreference.wantsScientist && !swapRole)
                    {
                        if (preferences[i].wantsScientist)
                        {
                            swapRole = true;
                        }
                    }

                    if (swapRole)
                    {
                        RoleType temp = preferences[i].player.Role;
                        preferences[i].player.SetRole(currentPreference.player.Role);
                        currentPreference.player.SetRole(temp);
                        preferences[i].player.Health = preferences[i].player.MaxHealth;
                        currentPreference.player.Health = currentPreference.player.MaxHealth;
                        preferences.RemoveAt(i);
                        break;
                    }
                }
                preferences.RemoveAt(0);
            }
        }
    }

}


