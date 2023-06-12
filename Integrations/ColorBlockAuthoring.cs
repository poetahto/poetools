using UnityEngine;
using UnityEngine.UI;

namespace Integrations
{
    [CreateAssetMenu]
    public class ColorBlockAuthoring : ScriptableObject
    {
        [SerializeField]
        private ColorBlock colorBlock;

        public ColorBlock ColorBlock => colorBlock;
    }
}