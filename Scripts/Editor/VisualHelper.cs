using JetBrains.Annotations;
using UnityEngine;

namespace Editor
{
    // Provides tools for quick debugging by displaying important information to the screen.
    [PublicAPI]
    public static class VisualHelper
    {
        public static void Visualize(this Vector3 vector, Vector3 position, Color color)
        {
            Debug.DrawRay(position, vector, color);
        }

        public static void Visualize(this Collision collision, Color color)
        {
            foreach (var contact in collision.contacts)
                contact.Visualize(color);
        }

        public static void Visualize(this ContactPoint point, Color color)
        {
            point.normal.Visualize(point.point, color);
        }
    }
}
