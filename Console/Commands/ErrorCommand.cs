using System;
using System.Collections.Generic;
using poetools.Core;

namespace poetools.Console.Commands
{
    public class ErrorCommand : ICommand
    {
        public string Name => string.Empty;
        public string Help => string.Empty;
        public IEnumerable<string> AutoCompletions { get; } = Array.Empty<string>();

        public void Execute(string[] args, RuntimeConsole console)
        {
            string message = "Invalid command! Try using \"help [command]\" for proper usage."
                .Format(Rtf.Italic, Rtf.GreyColor);

            console.View.Text += $"\n{message}";
        }

        public void Dispose()
        {
            // Do nothing.
        }
    }
}
