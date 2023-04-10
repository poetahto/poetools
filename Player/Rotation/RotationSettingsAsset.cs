using System;
using TriInspector;
using UnityEngine;

namespace poetools.player.Player.Rotation
{
    [HideMonoScript]
    [CreateAssetMenu(menuName = PlayerNaming.AssetMenuName + "/Rotation Settings")]
    public class RotationSettingsAsset : ScriptableObject
    {
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private RotationSettings settings;

        public RotationSettings Generate() => new RotationSettings
        {
            sensitivity = settings.sensitivity,
            invertY = settings.invertY,
        };
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
