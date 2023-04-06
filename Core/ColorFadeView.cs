using System;
using System.Collections;
using System.Threading.Tasks;
using poetools.Core.Abstraction;
using UnityEngine;
using UnityEngine.UI;

namespace poetools.Core
{
    public class ColorFadeView : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public Image image;
    }

    [Serializable]
    public class ColorFadeSettings
    {
        public float fadeSpeed = 1;
        public Color color = Color.black;
    }

    public class ColorFadeEffect : IVisibleComponent
    {
        private readonly ColorFadeView _view;
        private readonly ColorFadeSettings _settings;

        private float _targetAlpha;
        private bool _isVisible;

        public ColorFadeEffect(ColorFadeView view, ColorFadeSettings settings)
        {
            _view = view;
            _settings = settings;
        }

        public void Tick(float deltaTime)
        {
            _view.canvasGroup.alpha = Mathf.MoveTowards(_view.canvasGroup.alpha, _targetAlpha, _settings.fadeSpeed * deltaTime);
            _view.image.color = Color.Lerp(_view.image.color, _settings.color, _settings.fadeSpeed * deltaTime);
        }

        public IEnumerator Yield()
        {
            while (Math.Abs(_view.canvasGroup.alpha - _targetAlpha) > 0.01f)
                yield return null;
        }

        public async Task Await()
        {
            while (Math.Abs(_view.canvasGroup.alpha - _targetAlpha) > 0.01f)
                await Task.Yield();
        }

        public void SetVisible(bool isVisible)
        {
            OnVisibilityChanged?.Invoke(_isVisible, isVisible);
            _isVisible = isVisible;
            _targetAlpha = _isVisible ? 1 : 0;
            _view.canvasGroup.interactable = _isVisible;
            _view.canvasGroup.blocksRaycasts = _isVisible;
        }

        public bool IsVisible()
        {
            return _isVisible;
        }

        public event Action<bool, bool> OnVisibilityChanged;
    }
}
