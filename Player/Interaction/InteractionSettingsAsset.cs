using System;
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
        private InteractionSettings settings;

        public InteractionSettings Generate() => new InteractionSettings
        {
            range = settings.range,
        };
    }

    [Serializable]
    public class InteractionSettings
    {
        [Header("Standard Interaction Settings")]

        [SerializeField]
        public float range = 4f;
    }
}
