namespace poetools.Console.Commands
{
    public abstract class Preferences
    {
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
    }
}
