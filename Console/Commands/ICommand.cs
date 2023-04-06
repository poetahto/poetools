using System;
using System.Collections.Generic;
using poetools.Core;
using UnityEngine;

namespace poetools.Console.Commands
{
    public interface ICommand : IDisposable
    {
        string Name { get; }
        string Help { get; }
        IEnumerable<string> AutoCompletions { get; }

        void Execute(string[] args, RuntimeConsole console);
    }

    public abstract class Command : ScriptableObject, ICommand
    {
        public abstract string Name { get; }
        public virtual string Help { get; } = "No Help Available".Italic().Grey();
        public virtual IEnumerable<string> AutoCompletions { get; } = Array.Empty<string>();
        public virtual void Initialize(RuntimeConsole console) {}
        public abstract void Execute(string[] args, RuntimeConsole console);
        public virtual void Dispose() {}
    }
}
