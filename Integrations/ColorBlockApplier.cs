using System;
using UnityEngine;
using UnityEngine.UI;

namespace Integrations
{
    public class ColorBlockApplier : MonoBehaviour
    {
        public ColorBlockAuthoring authoring;
        public Selectable[] targets;

        private void UpdateTargets()
        {
            foreach (var target in targets)
                target.colors = authoring.ColorBlock;
        }

        private void Awake()
        {
            UpdateTargets();
        }

#if UNITY_EDITOR
        private void Update()
        {
            UpdateTargets();
        }
#endif
    }
}
