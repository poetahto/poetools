using poetools.Core;
using UnityEngine;

namespace poetools.Console.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/Pref")]
    public class PrefCommand : Command
    {
        public override string Name => "pref";

        public override void Execute(string[] args, RuntimeConsole console)
        {
            var preferences = Preferences.Default;

            if (args.Length == 1)
            {
                string prefName = args[0];

                console.Log("pref", preferences.TryGetValue(prefName, out string value)
                        ? value
                        : $"Preference \"{prefName}\" does not exist!");
            }

            if (args.Length >= 2)
            {
                string prefName = args[0];
                string value = args[1];
                preferences.SetValue(prefName, value);
                console.Log("pref", $"Updated {prefName} to {preferences.GetValue(prefName)}");
            }
        }
    }
}
