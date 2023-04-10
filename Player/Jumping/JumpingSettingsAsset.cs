using System;
using TriInspector;
using UnityEngine;

namespace poetools.player.Player.Jumping
{
    [HideMonoScript]
    [CreateAssetMenu(menuName = PlayerNaming.AssetMenuName + "/Jumping Settings")]
    public class JumpingSettingsAsset : ScriptableObject
    {
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private JumpingSettings settings;

        public JumpingSettings Generate() => new JumpingSettings
        {
            airJumps = settings.airJumps,
            coyoteTime = settings.coyoteTime,
            jumpDistance = settings.jumpDistance,
            jumpHeight = settings.jumpHeight,
            minDistance = settings.minDistance,
            minHeight = settings.minHeight,
            standardSkew = settings.standardSkew,
            assumedInitialSpeed = settings.assumedInitialSpeed,
            enableFastFall = settings.enableFastFall,
            fastFallSkew = settings.fastFallSkew,
            jumpBufferTime = settings.jumpBufferTime,
        };
    }

    // todo: more decoration and cleanup of these settings

    [Serializable]
    public class JumpingSettings
    {
        [Header("Standard Jumping Settings")]

        [SerializeField]
        public float jumpDistance = 4.5f;

        [SerializeField]
        public float jumpHeight = 1.1f;

        [SerializeField]
        public float assumedInitialSpeed = 5f;

        [Range(0.1f, 1)]
        [SerializeField]
        public float standardSkew = 0.5f;

        [Header("Fast Fall Settings")]

        [SerializeField]
        public bool enableFastFall;

        [SerializeField]
        public float minDistance = 2.5f;

        [SerializeField]
        public float minHeight = 0.55f;

        [Range(0.1f, 1)]
        [SerializeField]
        public float fastFallSkew = 0.5f;

        [Header("Other Settings")]

        [SerializeField]
        public int airJumps;

        [SerializeField]
        public float coyoteTime = 0.15f;

        [SerializeField]
        public float jumpBufferTime = 0.15f;
    }
}
