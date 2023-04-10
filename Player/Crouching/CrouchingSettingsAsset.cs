using System;
using TriInspector;
using UnityEngine;

namespace poetools.player.Player.Crouching
{
    [HideMonoScript]
    [CreateAssetMenu(menuName = PlayerNaming.AssetMenuName + "/Crouching Settings")]
    public class CrouchingSettingsAsset : ScriptableObject
    {
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private CrouchingSettings settings;

        public CrouchingSettings Generate() => new CrouchingSettings
        {
            cameraPercent = settings.cameraPercent,
            crouchHeight = settings.crouchHeight,
            crouchingSpeed = settings.crouchingSpeed,
            standingHeight = settings.standingHeight,
        };
    }

    [Serializable]
    public class CrouchingSettings
    {
        [Header("Standard Crouching Settings")]

        [Tooltip("The percent of the height will the crouch transform be positioned at.")]
        public float cameraPercent = 0.9f;

        [Tooltip("How tall the collider will be when crouching.")]
        public float crouchHeight = 1;

        [Tooltip("How tall the collider will be when standing.")]
        public float standingHeight = 2;

        [Tooltip("How quickly the crouch transform animates between standing and crouching.")]
        public float crouchingSpeed = 10;
    }
}
