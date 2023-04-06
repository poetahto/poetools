using UnityEngine;

namespace poetools.Core.Abstraction
{
    public abstract class CameraComponent : MonoBehaviour
    {
        public abstract float Fov { get; set; }
    }
}