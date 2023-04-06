using UnityEngine;

namespace poetools.Console.Commands
{
    [CreateAssetMenu(menuName = RuntimeConsoleNaming.AssetMenuName + "/Commands/Mouse")]
    public class MouseCommand : Command
    {
        public override string Name => "mouse";

        private RuntimeConsole _console;
        
        public override void Execute(string[] args, RuntimeConsole console)
        {
            if (args.Length < 1)
                return;

            _console = console;
            
            switch (args[0])
            {
                case "show": HandleShow(); break;
                case "hide": HandleHide(); break;
            }
        }

        private void HandleShow()
        {
            _console.Log(Name, "Mouse has been made visible.");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void HandleHide()
        {
            _console.Log(Name, "Mouse has been made invisible.");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}