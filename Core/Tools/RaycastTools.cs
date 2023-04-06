using UnityEngine;

namespace poetools.Core.Tools
{
    public static class RaycastTools
    {
        public static void SortByNearest(this RaycastHit[] hits, int end)
        {
            for (int i = 0; i < end; i++)
            {
                int minIndex = i;

                for (int j = i + 1; j < end; j++)
                {
                    if (hits[j].distance < hits[minIndex].distance)
                        minIndex = j;
                }

                (hits[minIndex], hits[i]) = (hits[i], hits[minIndex]);
            }
        }
        
        public static void SortByFurthest(this RaycastHit[] hits, int end)
        {
            for (int i = 0; i < end; i++)
            {
                int minIndex = i;

                for (int j = i + 1; j < end; j++)
                {
                    if (hits[j].distance > hits[minIndex].distance)
                        minIndex = j;
                }

                (hits[minIndex], hits[i]) = (hits[i], hits[minIndex]);
            }
        }
        
        public static bool Raycast3D(Ray ray, out RaycastHit hit, float maxDistance, int layerMask, 
            QueryTriggerInteraction interaction, float time = 1.0f)
        {
            bool result = Physics.Raycast(ray, out hit, maxDistance, layerMask, interaction);
            #if UNITY_EDITOR
                DrawDebugHandle(ray, result, hit, time, maxDistance);
            #endif
            return result;
        }
        
        public static bool Raycast3D(Ray ray, out RaycastHit hit, float maxDistance, float time = 1.0f)
        {
            bool result = Physics.Raycast(ray, out hit, maxDistance);
            #if UNITY_EDITOR
                DrawDebugHandle(ray, result, hit, time, maxDistance);
            #endif
            return result;
        }
        
        public static bool Raycast3D(Ray ray, out RaycastHit hit, float time = 1.0f)
        {
            bool result = Physics.Raycast(ray, out hit);
            #if UNITY_EDITOR
                DrawDebugHandle(ray, result, hit, time);
            #endif
            return result;
        }

        private static void DrawDebugHandle(Ray ray, bool success, RaycastHit hit, float time, float maxDistance = 1.0f)
        {
            Vector3 end = success ? hit.point : ray.origin + ray.direction * maxDistance;
            Color color = success ? Color.blue : Color.red;
            
            DebugDrawTools.Arrow(ray.origin, end, color, time);
        }
    }
}