using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace poetools.Core.Abstraction.Unity
{
    [RequireComponent(typeof(InputField))]
    public class InputFieldWrapper : InputFieldDisplay
    {
        public override event Action<string> OnSubmit;
        public override event Action<string, string> OnValueChange;
        
        private InputField _inputField;
        private string _previousValue = "";
        
        private void Awake()
        {
            _inputField = GetComponent<InputField>();
        }

        private void OnEnable()
        {
            _inputField.onValueChanged.AddListener(SendValueChangedEvent);
        }

        private void OnDisable()
        {
            _inputField.onValueChanged.RemoveListener(SendValueChangedEvent);
        }

        private void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == _inputField.gameObject && Input.GetKeyDown(KeyCode.Return))
            {
                OnSubmit?.Invoke(_inputField.text);
            }
        }

        private void SendValueChangedEvent(string newValue)
        {
            OnValueChange?.Invoke(_previousValue, newValue);
            _previousValue = newValue;
        }
        
        public override void Focus()
        {
            _inputField.ActivateInputField();
        }

        public override string GetText()
        {
            return _inputField.text;
        }

        public override void SetText(string value)
        {
            _inputField.text = value;
            _inputField.caretPosition = value.Length;
        }
    }
}