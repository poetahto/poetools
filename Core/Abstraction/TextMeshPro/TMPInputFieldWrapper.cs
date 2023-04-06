#if TMP_ENABLED

using System;
using TMPro;
using UnityEngine;

namespace poetools.Core.Abstraction.TextMeshPro
{
    [RequireComponent(typeof(TMP_InputField))]
    public class TMPInputFieldWrapper : InputFieldDisplay
    {
        public override event Action<string> OnSubmit;
        public override event Action<string, string> OnValueChange;
        
        private TMP_InputField _inputField;
        private string _previousValue = "";

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        private void OnEnable()
        {
            _inputField.onSubmit.AddListener(SendSubmitEvent);
            _inputField.onValueChanged.AddListener(SendValueChangedEvent);
        }

        private void OnDisable()
        {
            _inputField.onSubmit.RemoveListener(SendSubmitEvent);
            _inputField.onValueChanged.AddListener(SendValueChangedEvent);
        }
        
        private void SendValueChangedEvent(string newValue)
        {
            OnValueChange?.Invoke(_previousValue, newValue);
            _previousValue = newValue;
        }

        private void SendSubmitEvent(string submittedValue)
        {
            OnSubmit?.Invoke(submittedValue);
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
        }
    }
}

#endif