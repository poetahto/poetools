using System;
using UnityEngine;

namespace poetools.Core.Abstraction
{
    /// <summary>
    /// An object that can display text. 
    /// <remarks> Useful for abstracting default Unity text from TMP text. </remarks>
    /// </summary>
    public interface ITextDisplay
    {
        string GetText();
        void SetText(string value);

        event Action<string, string> OnValueChange;
    }
    
    /// <summary>
    /// Abstract implementation of <see cref="ITextDisplay"/>. 
    /// <remarks> Useful for referencing in the Unity Inspector. </remarks>
    /// </summary>
    public abstract class TextDisplay : MonoBehaviour, ITextDisplay
    {
        public abstract string GetText();

        public abstract void SetText(string value);
        
        public abstract event Action<string, string> OnValueChange;
    }

    public static class TextDisplayExtensions
    {
        public static void Append(this ITextDisplay textDisplay, string text)
        {
            string existingText = textDisplay.GetText();
            string newText = $"{existingText}{text}";
            textDisplay.SetText(newText);
        }
        
        public static void AppendLine(this ITextDisplay textDisplay, string text)
        {
            Append(textDisplay, $"\n{text}");
        }

        public static void Clear(this ITextDisplay textDisplay)
        {
            textDisplay.SetText("");
        }
    }
}