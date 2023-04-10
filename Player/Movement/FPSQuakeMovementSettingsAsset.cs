using System;
using TriInspector;
using UnityEngine;

namespace poetools.player.Player.Movement
{
    [HideMonoScript]
    [CreateAssetMenu(menuName = PlayerNaming.AssetMenuName + "/Quake Movement Settings")]
    public class FPSQuakeMovementSettingsAsset : ScriptableObject
    {
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private FPSQuakeMovementSettings settings;

        public FPSQuakeMovementSettings Generate() => new FPSQuakeMovementSettings
        {
            noFrictionJumpWindow = settings.noFrictionJumpWindow,
            friction = settings.friction,
            airAcceleration = settings.airAcceleration,
            groundAcceleration = settings.groundAcceleration,
            trueMax = settings.trueMax,
            maxAirSpeed = settings.maxAirSpeed,
            maxGroundSpeed = settings.maxGroundSpeed,
        };
    }

    [Serializable]
    public class FPSQuakeMovementSettings
    {
        [Header("Standard Quake Movement Settings")]

        [SerializeField]
        public float noFrictionJumpWindow = 0.1f;

        [SerializeField]
        public float friction = 10f;

        [SerializeField]
        public float airAcceleration = 40f;

        [SerializeField]
        public float groundAcceleration = 50f;

        [SerializeField]
        public float maxAirSpeed = 1f;

        [SerializeField]
        public float maxGroundSpeed = 5f;

        [SerializeField]
        public float trueMax = 10f;
    }
}
