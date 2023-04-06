using UnityEngine;
using UnityEngine.Events;

namespace poetools.Console
{
    public class ConsoleEventHooks : MonoBehaviour
    {
        [SerializeField] 
        private UnityEvent<bool, bool> onVisibilityChange;
        
        [SerializeField] 
        private UnityEvent<string, string> onInputChange;
        
        [SerializeField] 
        private UnityEvent<string> onInputSubmit;

        private RuntimeConsoleView _view;

        private void Awake()
        {
            _view = RuntimeConsole.Singleton.View;
        }

        private void OnEnable()
        {
            _view.OnVisibilityChanged += HandleVisibilityChange;
            _view.InputFieldDisplay.OnValueChange += HandleInputChange;
            _view.InputFieldDisplay.OnSubmit += HandleSubmit;
        }
        
        private void OnDisable()
        {
            _view.OnVisibilityChanged -= HandleVisibilityChange;
            _view.InputFieldDisplay.OnValueChange -= HandleInputChange;
            _view.InputFieldDisplay.OnSubmit -= HandleSubmit;
        }
        
        private void HandleSubmit(string obj)
        {
            onInputSubmit.Invoke(obj);
        }

        private void HandleInputChange(string arg1, string arg2)
        {
            onInputChange.Invoke(arg1, arg2);
        }

        private void HandleVisibilityChange(bool arg1, bool arg2)
        {
            onVisibilityChange.Invoke(arg1, arg2);
        }
    }
}