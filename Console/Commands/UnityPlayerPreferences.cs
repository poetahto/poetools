using UnityEngine;

namespace poetools.Console.Commands
{
    public class UnityPlayerPreferences : Preferences
    {
        public override void SetValue(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public override string GetValue(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public override bool HasValue(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public override void RemoveValue(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public override void Save(string saveName)
        {
            // No action needed.
        }

        public override void Load(string saveName)
        {
            // No action needed.
        }
    }
}
