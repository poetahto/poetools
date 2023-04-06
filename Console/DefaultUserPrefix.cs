using poetools.Core;
using UnityEngine;

namespace poetools.Console
{
    [CreateAssetMenu]
    public class DefaultUserPrefix : UserPrefix
    {
        [SerializeField]
        private Color prefixColor;

        public override string GenerateMessage(string input)
        {
            return $"{">".Bold().Color(prefixColor)} {input}";
        }
    }
}
