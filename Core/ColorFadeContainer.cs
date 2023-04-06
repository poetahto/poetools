using UnityEngine;

namespace poetools.Core
{
    public class ColorFadeContainer : MonoBehaviour
    {
        [SerializeField]
        private ColorFadeView fadeView;

        [SerializeField]
        private ColorFadeSettings fadeSettings;

        private ColorFadeEffect _fadeEffect;

        public ColorFadeEffect FadeEffect => _fadeEffect ??= new ColorFadeEffect(fadeView, fadeSettings);

        private void Update()
        {
            FadeEffect.Tick(Time.deltaTime);
        }
    }
}
