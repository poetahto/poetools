using System;
using UnityEngine;

namespace poetools.Core.Abstraction.Unity
{
    public class CanvasGroupVisibleComponent : VisibleComponent
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private bool _isVisible;

        public override void SetVisible(bool isVisible)
        {
            if (_isVisible == isVisible)
                return;

            OnVisibilityChanged?.Invoke(_isVisible, isVisible);
            _isVisible = isVisible;

            if (_isVisible)
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }

        public override bool IsVisible()
        {
            return _isVisible;
        }

        public override event Action<bool, bool> OnVisibilityChanged;
    }
}
