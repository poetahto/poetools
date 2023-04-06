using poetools.Core;
using poetools.Core.Abstraction;
using UnityEngine;

namespace poetools.player.Player.Movement
{
    public class FPSQuakeMovementLogic
    {
        private FPSQuakeMovementSettings _settings;
        private float _speed;
        private Vector3 _velocity;
        private GroundCheck _groundCheck;
        private IPhysicsComponent _physicsComponent;
        private Transform _yawTransform;

        private bool _hasGroundCheck;
        private bool _hasPhysics;
        private bool _hasViewDirection;

        public FPSQuakeMovementLogic(FPSQuakeMovementSettings settings)
        {
            _settings = settings;
        }

        public FPSQuakeMovementSettings Settings => _settings;

        public float SpeedMultiplier { get; set; } = 1;
        public Vector3 TargetDirection { get; set; } = Vector3.zero;

        public GroundCheck GroundCheck
        {
            get => _groundCheck;
            set
            {
                _groundCheck = value;
                _hasGroundCheck = _groundCheck != null;
            }
        }

        public Transform YawTransform
        {
            get => _yawTransform;
            set
            {
                _yawTransform = value;
                _hasViewDirection = _yawTransform != null;
            }
        }

        public IPhysicsComponent PhysicsComponent
        {
            get => _physicsComponent;
            set
            {
                _physicsComponent = value;
                _hasPhysics = _physicsComponent != null;
            }
        }

        public float CurrentRunningSpeed
        {
            get
            {
                Vector3 v = Velocity;
                v.y = 0;
                return v.magnitude;
            }
        }

        private float GroundedTime => _hasGroundCheck ? _groundCheck.GroundTime : 0;
        private Vector3 Forward => _hasViewDirection ? _yawTransform.forward : Vector3.forward;
        private Vector3 Right => _hasViewDirection ? _yawTransform.right : Vector3.right;
        private bool IsGrounded => !_hasGroundCheck || _groundCheck.IsGrounded;

        private Vector3 Velocity
        {
            get => _hasPhysics ? _physicsComponent.Velocity : Vector3.zero;
            set
            {
                if (_hasPhysics)
                    _physicsComponent.Velocity = value;
            }
        }

        public void PhysicsTick()
        {
            _speed = CurrentRunningSpeed;
            _velocity = Velocity;
            var newVel = IsGrounded ? MoveGround() : MoveAir();
            newVel.y = 0;
            newVel = Vector3.ClampMagnitude(newVel, _settings.trueMax);
            newVel.y = Velocity.y;
            Velocity = newVel;
        }

        private Vector3 MoveGround()
        {
            if (_speed != 0 && GroundedTime > _settings.noFrictionJumpWindow)
            {
                float drop = _speed * _settings.friction * Time.deltaTime;
                // float originalY = _velocity.y;
                _velocity *= Mathf.Max(_speed - drop, 0) / _speed;
                // _velocity.y = originalY;
            }

            return Accelerate(_settings.groundAcceleration, _settings.maxGroundSpeed * SpeedMultiplier);
        }

        private Vector3 MoveAir()
        {
            return Accelerate(_settings.airAcceleration, _settings.maxAirSpeed * SpeedMultiplier);
        }

        private Vector3 Accelerate(float acceleration, float maxVelocity)
        {
            var targetForward = Forward * TargetDirection.y;
            var targetStrafe = Right * TargetDirection.x;
            var targetDir = (targetForward + targetStrafe).normalized;

            float projVel = Vector3.Dot(_velocity, targetDir);
            float accelVel = acceleration * Time.deltaTime;

            if (projVel + accelVel > maxVelocity)
                accelVel = Mathf.Max(maxVelocity - projVel, 0);
            // accelVel = maxVelocity - projVel;

            return _velocity + targetDir * accelVel;
        }
    }
}
