using Listeners;
using UnityEngine;

namespace Input
{
    [CreateAssetMenu]
    public class SourceMovement : MovementType
    {
        [Header("Settings")]
        [SerializeField] public int noFrictionFrameCount = 1;
        [SerializeField] public float friction = 5.5f;
        [SerializeField] public float airAcceleration = 40f;
        [SerializeField] public float groundAcceleration = 50f;
        [SerializeField] public float maxAirSpeed = 1f;
        [SerializeField] public float maxGroundSpeed = 5f;
        
        private GroundedCollisionListener _groundCheck;
        private Vector3 _currentVelocity;
        private Vector3 _currentTargetDirection;
        
        public override Vector3 CalculateVelocity(Vector3 currentVelocity, Vector3 currentTargetDirection,
            GroundedCollisionListener groundCheck)
        {
            _groundCheck = groundCheck;
            _currentVelocity = currentVelocity;
            _currentTargetDirection = currentTargetDirection;
            
            return _groundCheck.IsGrounded
                ? MoveGround()
                : MoveAir();
        }
        
        private Vector3 MoveGround()
        {
            float speed = _currentVelocity.magnitude;
            
            if (speed != 0 && _groundCheck.FramesSpentOnGround > noFrictionFrameCount)
            {
                float drop = speed * friction * Time.deltaTime;
                _currentVelocity *= Mathf.Max(speed - drop, 0) / speed;
            }

            return Accelerate(groundAcceleration, maxGroundSpeed);
        }

        private Vector3 MoveAir()
        {
            return Accelerate(airAcceleration, maxAirSpeed);
        }
        
         private Vector3 Accelerate(float acceleration, float maxVelocity)
        {
            float projVel = Vector3.Dot(_currentVelocity, _currentTargetDirection);
            float accelVel = acceleration * Time.deltaTime;

            if (projVel + accelVel > maxVelocity)
                accelVel = Mathf.Max(maxVelocity - projVel, 0);

            return _currentVelocity + _currentTargetDirection * accelVel;
        }
    }
}