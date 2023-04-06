using UnityEngine;

namespace poetools.Core.Tools
{
    public static class GameObjectExtensions
    {
        public static void EnsureHas<T>(this GameObject target) where T : Component
        {
            GetOrAdd<T>(target);
        }

        public static T GetOrAdd<T>(this GameObject target) where T : Component
        {
            if (!target.TryGetComponent(out T result))
            {
                result = target.AddComponent<T>();
            }

            return result;
        }
    }
}
