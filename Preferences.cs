using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Exiled.API.Features;
using Utf8Json;

namespace OptIn
{
    class Preferences
    {
        static Config config = OptIn.Instance.Config;
        public static List<string> wantsSCP = new List<string>();
        public static List<string> wantsDClass = new List<string>();
        public static List<string> wantsGuard = new List<string>();
        public static List<string> wantsScientist = new List<string>();


        public static void GetPreferencesFromFile()
        {
            if (File.Exists(config.preferencesLocation))
            {
                // Get values from file
                if (config.DebugLogs){Log.Info("Loading Player Preferneces...");}

                using (StreamReader sr = new StreamReader(config.preferencesLocation)) 
                {
                    wantsSCP = JsonSerializer.Deserialize<List<string>>(Encoding.ASCII.GetBytes(sr.ReadLine()));
                    wantsDClass = JsonSerializer.Deserialize<List<string>>(Encoding.ASCII.GetBytes(sr.ReadLine()));
                    wantsGuard = JsonSerializer.Deserialize<List<string>>(Encoding.ASCII.GetBytes(sr.ReadLine()));
                    wantsScientist = JsonSerializer.Deserialize<List<string>>(Encoding.ASCII.GetBytes(sr.ReadLine()));
                    sr.Close();
                }

                if (config.DebugLogs){Log.Info("Player Preferences Loaded.");}
                
                
            }
            else
            {
                if (config.DebugLogs){Log.Warn("Player Preferences File Not Found.");}
            }
        }

        public static void SavePreferencesToFile()
        {
            // Save values to file
            if (config.DebugLogs){Log.Info("Saving Player Preferences...");}

            string json = Encoding.UTF8.GetString(JsonSerializer.Serialize(wantsSCP)) + "\n";
            json = String.Concat(json, Encoding.UTF8.GetString(JsonSerializer.Serialize(wantsDClass)) + "\n");
            json = String.Concat(json, Encoding.UTF8.GetString(JsonSerializer.Serialize(wantsGuard)) + "\n");
            json = String.Concat(json, Encoding.UTF8.GetString(JsonSerializer.Serialize(wantsScientist)));

            using (StreamWriter sw = new StreamWriter(config.preferencesLocation)){ sw.Write(json); sw.Close(); }
            if (config.DebugLogs){Log.Info("Player Preferences Saved.");}
        }

        public static List<PlayerPreference> GetPlayerPreferences(List<Player> players)
        {
            List<PlayerPreference> preferences = new List<PlayerPreference>();
            foreach (var player in players)
            {
                string ID = player.UserId;
                preferences.Add(new PlayerPreference(ID, player, wantsSCP.Contains(ID), wantsDClass.Contains(ID), wantsGuard.Contains(ID), wantsScientist.Contains(ID)));
            }
            return preferences;
        }

        public static PlayerPreference GetPlayerPreference(String ID)
        {
            return new PlayerPreference(ID, null, wantsSCP.Contains(ID), wantsDClass.Contains(ID), wantsGuard.Contains(ID), wantsScientist.Contains(ID));

        }

        public static void ChangePreference(PlayerPreference preference)
        {
            string ID = preference.userID;

            if (preference.wantsSCP) { if (!wantsSCP.Contains(ID)) { wantsSCP.Add(ID); } }
            else { if (wantsSCP.Contains(ID)) { wantsSCP.Remove(ID); } }

            if (preference.wantsDClass) { if (!wantsDClass.Contains(ID)) { wantsDClass.Add(ID); } }
            else { if (wantsDClass.Contains(ID)) { wantsDClass.Remove(ID); } }

            if (preference.wantsGaurd) { if (!wantsGuard.Contains(ID)) { wantsGuard.Add(ID); } }
            else { if (wantsGuard.Contains(ID)) { wantsGuard.Remove(ID); } }

            if (preference.wantsScientist) { if (!wantsScientist.Contains(ID)) { wantsScientist.Add(ID); } }
            else { if (wantsScientist.Contains(ID)) { wantsScientist.Remove(ID); } }

        }
    }

    class PlayerPreference
    {
        public string userID;
        public Player player;
        public bool wantsSCP, wantsDClass, wantsGaurd, wantsScientist;

        public PlayerPreference(string _userID, Player _player = null, bool _wantsSCP = true, bool _wantsDClass = true, bool _wantsGaurd = true, bool _wantsScientist = true)
        {
            userID = _userID;
            player = _player;
            wantsSCP = _wantsSCP;
            wantsDClass = _wantsDClass;
            wantsGaurd = _wantsGaurd;
            wantsScientist = _wantsScientist;
        }
    }
}
