using UnityEngine;

namespace Integrations
{
    [CreateAssetMenu]
    public class GraphicAuthoring : ScriptableObject
    {
        [SerializeField]
        private Color color;

        public Color Color => color;
    }
}