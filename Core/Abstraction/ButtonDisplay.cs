using System;
using UnityEngine;

namespace poetools.Core.Abstraction
{
    public interface IButton
    {
        event Action OnClick;
    }
    
    public abstract class ButtonDisplay : MonoBehaviour, IButton
    {
        public abstract event Action OnClick;
    }

    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class UnityButton : ButtonDisplay
    {
        public override event Action OnClick;

        private UnityEngine.UI.Button _button;
        
        private void Awake()
        {
            _button = GetComponent<UnityEngine.UI.Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleOnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleOnClick);
        }

        private void HandleOnClick()
        {
            OnClick?.Invoke();
        }
    }
}