#if TMP_ENABLED

using System;
using TMPro;
using UnityEngine;

namespace poetools.Core.Abstraction.TextMeshPro
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMPTextWrapper : TextDisplay
    {
        public override event Action<string, string> OnValueChange;

        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
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

#endif