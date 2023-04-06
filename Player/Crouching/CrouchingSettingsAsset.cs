﻿using System;
using TriInspector;
using UnityEngine;

namespace poetools.player.Player.Crouching
{
    [HideMonoScript]
    [CreateAssetMenu]
    public class CrouchingSettingsAsset : ScriptableObject
    {
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        public CrouchingSettings settings;
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