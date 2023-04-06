using UnityEngine;
using UnityEngine.UI;

namespace poetools.player.Player.Interaction
{
    public class InteractableCrosshairIndicator : MonoBehaviour
    {
        [SerializeField] private CanvasGroup crosshairCanvasGroup;
        [SerializeField] private CanvasGroup textCanvasGroup;
        [SerializeField] private Text targetNameText;
        [SerializeField] private float animationSpeed = 15;
        [SerializeField] private float scaleAmount = 0.5f;
        [SerializeField] private float boostRestoreSpeed = 10;
        [SerializeField] private float boostAmount = 1.5f;

        private Vector3 _originalScale;
        private float _targetScale;
        private float _targetAlpha;
        private float _interactedBoost;

        private void Awake()
        {
            _originalScale = crosshairCanvasGroup.transform.localScale;
            _targetAlpha = 0;
            _targetScale = 1;
        }

        private void Update()
        {
            crosshairCanvasGroup.alpha = Mathf.Lerp(crosshairCanvasGroup.alpha, Mathf.Max(0.5f, _targetAlpha), animationSpeed * Time.deltaTime);
            textCanvasGroup.alpha = Mathf.Lerp(textCanvasGroup.alpha, _targetAlpha, animationSpeed * Time.deltaTime);
            crosshairCanvasGroup.transform.localScale = Vector3.Lerp(crosshairCanvasGroup.transform.localScale, _originalScale * (_targetScale + _interactedBoost), animationSpeed * Time.deltaTime);
            _interactedBoost  = Mathf.Max(0, _interactedBoost - Time.deltaTime * boostRestoreSpeed);
        }

        public void FaceObjectStarted(GameObject interactable)
        {
            _targetAlpha = 1;
            _targetScale = 1 + scaleAmount;
            targetNameText.text = interactable.name;
        }

        public void FaceObjectStopped()
        {
            _targetAlpha = 0;
            _targetScale = 1;
        }

        public void Interacted()
        {
            _interactedBoost = boostAmount;
        }
    }
}
