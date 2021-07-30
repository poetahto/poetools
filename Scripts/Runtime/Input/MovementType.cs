using Listeners;
using UnityEngine;

namespace Input
{
    public abstract class MovementType : ScriptableObject
    {
        public abstract Vector3 CalculateVelocity(Vector3 currentVelocity, Vector3 movementDirection,
            GroundedCollisionListener groundCheck);
    }
}