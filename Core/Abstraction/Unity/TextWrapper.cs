using System;
using UnityEngine;
using UnityEngine.UI;

namespace poetools.Core.Abstraction.Unity
{
    [RequireComponent(typeof(Text))]
    public class TextWrapper : TextDisplay
    {
        public override event Action<string, string> OnValueChange;

        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
        }

        public override string GetText()
        {
            return _text.text;
        }

        public override void SetText(string value)
        {
            string oldValue = _text.text;
            _text.text = value;
            OnValueChange?.Invoke(oldValue, value);
        }
    }
}