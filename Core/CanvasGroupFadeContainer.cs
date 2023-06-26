using poetools.Core.Abstraction;
using UnityEngine;

namespace poetools.Core
{
    public class CanvasGroupFadeContainer : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private CanvasGroupFadeSettings fadeSettings;

        private CanvasGroupFadeEffect _fadeEffect;

        public CanvasGroupFadeEffect FadeEffect => _fadeEffect ??= new CanvasGroupFadeEffect(canvasGroup, fadeSettings);

        private void Update()
        {
            FadeEffect.Tick(Time.deltaTime);
        }

        public void Show()
        {
            _fadeEffect.Show();
        }

        public void Hide()
        {
            _fadeEffect.Hide();
        }
    }
}
