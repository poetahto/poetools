using System;
using UnityEngine;

namespace poetools.Core
{
    public class UnityPlayerPreferences : Preferences
    {
        public override event Action<PreferenceChangeData> ValueChanged;
        public override event Action LoadFinished;
        public override event Action SaveStarted;

        public override void SetValue(string key, string value)
        {
            ValueChanged?.Invoke(GenerateChangeData(key, value));
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
            SaveStarted?.Invoke();
        }

        public override void Load(string saveName)
        {
            // No action needed.
            LoadFinished?.Invoke();
        }
    }
}
