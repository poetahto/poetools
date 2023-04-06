using UnityEngine;

namespace poetools.Core.SafeCollision
{
    public class SafeSphereCollider : SafeColliderGeneric<SphereCollider>
    {
        [SerializeField] private float padding = 0.05f;
        
        protected override void CopyTo(SphereCollider target, SphereCollider original)
        {
            target.center = original.center;
            target.radius = original.radius - padding;
        }
    }
}