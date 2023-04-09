﻿using System;
using TriInspector;
using UnityEngine;

namespace poetools.player.Player.Interaction
{
    [HideMonoScript]
    [CreateAssetMenu(menuName = PlayerNaming.AssetMenuName + "/Interaction Settings")]
    public class InteractionSettingsAsset : ScriptableObject
    {
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        public InteractionSettings settings;
    }

    [Serializable]
    public class InteractionSettings
    {
        [Header("Standard Interaction Settings")]

        [SerializeField]
        public float range = 4f;
    }
}
