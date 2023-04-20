using UnityEngine;
using UnityEngine.UI;

namespace Integrations
{
    public class GraphicApplier : MonoBehaviour
    {
        [SerializeField] private Graphic[] targets;
        public GraphicAuthoring authoring;

        private void Awake()
        {
            foreach (var target in targets)
                target.color = authoring.Color;
        }
    }
}
