using System;

namespace poetools.Core.Abstraction
{
    /// <summary>
    /// An object that can receive input. 
    /// <remarks> Useful for abstracting default Unity input field from TMP input field. </remarks>
    /// </summary>
    public interface IInputField : ITextDisplay
    {
        void Focus();
        event Action<string> OnSubmit;
    }
    
    /// <summary>
    /// Abstract implementation of <see cref="IInputField"/>. 
    /// <remarks> Useful for referencing in the Unity Inspector. </remarks>
    /// </summary>
    public abstract class InputFieldDisplay : TextDisplay, IInputField
    {
        public abstract event Action<string> OnSubmit;
        public abstract void Focus();
    }
}