using UnityEngine;

namespace poetools.player.Player.Crouching.Colliders
{
    public abstract class CrouchingCollider : MonoBehaviour, ICrouchingCollider
    {
        public abstract Vector3 Center { get; set; }
        public abstract float Height { get; set; }
        public abstract bool CanStand { get; }
        public abstract bool Cast(Vector3 direction, float distance, out RaycastHit info);
    }
}