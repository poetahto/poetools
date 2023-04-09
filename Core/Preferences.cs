using System;

namespace poetools.Core
{
    public struct PreferenceChangeData
    {
        public string Preference;
        public string OldValue;
        public string NewValue;
    }

    public abstract class Preferences
    {
        public abstract event Action<PreferenceChangeData> ValueChanged;
        public abstract event Action LoadFinished;
        public abstract event Action SaveStarted;

        public static Preferences Default { get; set; } = new UnityPlayerPreferences();

        public abstract void SetValue(string key, string value);

        public abstract string GetValue(string key);

        public abstract bool HasValue(string key);

        public abstract void RemoveValue(string key);

        public abstract void Save(string saveName);

        public abstract void Load(string saveName);

        public bool TryGetValue(string key, out string value)
        {
            if (HasValue(key))
            {
                value = GetValue(key);
                return true;
            }

            value = string.Empty;
            return false;
        }

        protected PreferenceChangeData GenerateChangeData(string key, string newValue)
        {
            return new PreferenceChangeData
            {
                Preference = key,
                NewValue = newValue,
                OldValue = GetValue(key),
            };
        }
    }
}
