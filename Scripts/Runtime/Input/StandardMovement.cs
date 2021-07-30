using Listeners;
using UnityEngine;

namespace Input
{
    [CreateAssetMenu]
    public class StandardMovement : MovementType
    {
        [Header("Settings")]
        [SerializeField] public float maxMovementSpeed = 5f;
        [SerializeField] public float groundedRampTime = 0.125f;
        [SerializeField] public float airborneRampTime = 0.3f;

        private Vector3 _targetVelocity;

        public override Vector3 CalculateVelocity(Vector3 currentVelocity, Vector3 movementDirection,
            GroundedCollisionListener groundCheck)
        {
            float acceleration = groundCheck.IsGrounded 
                ? StandardGroundedAcceleration 
                : StandardAirborneAcceleration;
            
            _targetVelocity = movementDirection * maxMovementSpeed;
            return Vector3.MoveTowards(currentVelocity, _targetVelocity, acceleration);
        }
        
        private float StandardGroundedAcceleration => groundedRampTime > 0 
                ? 1 / groundedRampTime * maxMovementSpeed * Time.deltaTime 
                : maxMovementSpeed;

        private float StandardAirborneAcceleration => airborneRampTime > 0 
            ? 1 / airborneRampTime * maxMovementSpeed * Time.deltaTime 
            : maxMovementSpeed;
    }
}