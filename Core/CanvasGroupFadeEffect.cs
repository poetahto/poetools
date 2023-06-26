using System;
using System.Collections;
using System.Threading.Tasks;
using poetools.Core.Abstraction;
using UnityEngine;

namespace poetools.Core
{
    [Serializable]
    public class CanvasGroupFadeSettings
    {
        public float fadeSpeed = 1;
    }

    public class CanvasGroupFadeEffect : IVisibleComponent
    {
        private readonly CanvasGroup _canvasGroup;
        private readonly CanvasGroupFadeSettings _settings;

        private float _targetAlpha;
        private bool _isVisible;

        public CanvasGroupFadeEffect(CanvasGroup canvasGroup, CanvasGroupFadeSettings settings)
        {
            _canvasGroup = canvasGroup;
            _settings = settings;
        }

        public void Tick(float deltaTime)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, _targetAlpha, _settings.fadeSpeed * deltaTime);
        }

        public IEnumerator Yield()
        {
            while (Math.Abs(_canvasGroup.alpha - _targetAlpha) > 0.01f)
                yield return null;
        }

        public async Task Await()
        {
            while (Math.Abs(_canvasGroup.alpha - _targetAlpha) > 0.01f)
                await Task.Yield();
        }

        public void SetVisible(bool isVisible)
        {
            OnVisibilityChanged?.Invoke(_isVisible, isVisible);
            _isVisible = isVisible;
            _targetAlpha = _isVisible ? 1 : 0;
            _canvasGroup.interactable = _isVisible;
            _canvasGroup.blocksRaycasts = _isVisible;
        }

        public bool IsVisible()
        {
            return _isVisible;
        }

        public event Action<bool, bool> OnVisibilityChanged;
    }
}
