using UnityEngine;

namespace poetools.player.Player.Crouching.Colliders
{
    /// <summary>
    /// A collider that can be manipulated for crouching.
    /// </summary>
    public interface ICrouchingCollider
    {
        public abstract Vector3 Center { get; set; }
        public abstract float Height { get; set; }
        public abstract bool CanStand { get; }
        public abstract bool Cast(Vector3 direction, float distance, out RaycastHit info);
    }
}