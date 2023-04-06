using UnityEngine;

namespace poetools.Core.Tools
{
    public static class DebugDrawTools
    {
        public static void Triangle(Ray ray, float width, float height,
            Color color, float time = 1.0f, bool depthTest = false)
        {
            var matrix = Matrix4x4.identity;
            matrix *= Matrix4x4.Translate(ray.origin);
            matrix *= Matrix4x4.Rotate(Quaternion.FromToRotation(Vector3.up, ray.direction));
            
            var leftCorner = matrix.MultiplyPoint(-Vector3.right * (width / 2));
            var rightCorner = matrix.MultiplyPoint(Vector3.right * (width / 2));
            var topCorner = matrix.MultiplyPoint(Vector3.up * height);
            
            Debug.DrawLine(leftCorner, rightCorner, color, time, depthTest);
            Debug.DrawLine(rightCorner, topCorner, color, time, depthTest);
            Debug.DrawLine(topCorner, leftCorner, color, time, depthTest);
        }
        
        public static void Arrow(Vector3 start, Vector3 end, 
            Color color, float time = 1.0f, bool depthTest = false)
        {
            Vector3 direction = (end - start).normalized;
            Debug.DrawLine(start, end, color, time, false);
            Triangle(new Ray(end, direction), 0.25f, 0.25f, color, time, depthTest);
        }
    }
}