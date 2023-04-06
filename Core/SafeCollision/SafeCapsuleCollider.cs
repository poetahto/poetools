using UnityEngine;

namespace poetools.Core.SafeCollision
{
    public class SafeCapsuleCollider : SafeColliderGeneric<CapsuleCollider>
    {
        [SerializeField] private float padding = 0.05f;
        
        protected override void CopyTo(CapsuleCollider target, CapsuleCollider original)
        {
            target.center = original.center;
            target.height = original.height - padding;
            target.radius = original.radius - padding;
            target.direction = original.direction;
        }
    }
}