using System;
using poetools.Core.Abstraction;
using UnityEngine;

namespace poetools.Console
{
    public class RuntimeConsoleView : MonoBehaviour, IVisibleComponent
    {
        [SerializeField] private TextDisplay textDisplay;
        [SerializeField] private TextDisplay autoCompleteDisplay;
        [SerializeField] private InputFieldDisplay inputFieldDisplay;
        
        private ITextDisplay TextDisplay => textDisplay;
        public ITextDisplay AutoCompleteDisplay => autoCompleteDisplay;
        public IInputField InputFieldDisplay => inputFieldDisplay;

        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                    _textChanged = true;

                _text = value;
            }
        }

        private string _text;
        private bool _textChanged;

        private void Update()
        {
            if (_textChanged)
            {
                TextDisplay.SetText(Text);
                _textChanged = false;
            }
        }

        public void SetVisible(bool isVisible)
        {
            bool wasVisible = gameObject.activeSelf;
            gameObject.SetActive(isVisible);
            OnVisibilityChanged?.Invoke(wasVisible, isVisible);
        }

        public bool IsVisible()
        {
            return gameObject.activeSelf;
        }

        public event Action<bool, bool> OnVisibilityChanged;
    }
}