using System;
using TriInspector;
using UnityEngine;

namespace poetools.player.Player.Rotation
{
    [HideMonoScript]
    [CreateAssetMenu]
    public class RotationSettingsAsset : ScriptableObject
    {
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        public RotationSettings settings;
    }

    [Serializable]
    public class RotationSettings
    {
        [Header("Standard Rotation Settings")]

        [PropertyTooltip("How fast the transform should rotate when the mouse is moved.")]
        [OnValueChanged(nameof(EnsurePositiveSensitivity))]
        [SerializeField]
        public float sensitivity = 1;

        [PropertyTooltip("Should Y movements be inverted when rotating?")]
        [SerializeField]
        public bool invertY;

        private void EnsurePositiveSensitivity()
        {
            sensitivity = Mathf.Max(0, sensitivity);
        }
    }
}
