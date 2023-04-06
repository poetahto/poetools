using System;
using poetools.Core;
using poetools.Core.Abstraction;
using UnityEngine;

namespace poetools.player.Player.Movement
{
    [RequireComponent(typeof(GroundCheck))]
    public class FPSStandardMovementSystem : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            public float acceleration = 15;
            public float deceleration = 7;
            public float reactivity = 3;
            public float sprintMultiplier = 1.3f;
        }

        public float fovSmoothing = 5f;
        public float fovMultiplier = 1.2f;
        public float speed = 3;
        public Settings airborneSettings;
        public Settings groundedSettings;
        public bool useSmoothStop = true;

        public bool IsSprinting { get; set; }
        public Vector2 InputDirection { get; set; }
        public float SprintMultiplier { get; set; } = 1;
        public Transform yawTransform;
        public CameraComponent playerCamera; // todo: make this into a seperate script, for sprinting FX
        public float DefaultFov { get; set; }
        
        [SerializeField] private PhysicsComponent physics;
        private GroundCheck _groundCheck;

        public Settings CurrentSettings => _groundCheck.IsGrounded ? groundedSettings : airborneSettings;
        public float GroundSpeed => new Vector3(physics.Velocity.x, 0, physics.Velocity.z).magnitude;
        
        private Vector3 _targetVelocity;
        private Vector3 _currentVelocity;
        private bool _isAccelerating;

        public bool ReallyIsSprinting()
        {
            return IsSprinting && InputDirection.y > 0f;
        }
        
        public float GetCameraFOV(float currentFov)
        {
            float targetFov = DefaultFov * (ReallyIsSprinting() ? fovMultiplier : 1);
            return Mathf.Lerp(currentFov, targetFov, fovSmoothing * Time.deltaTime);
        }

        private void Awake()
        {
            _groundCheck = GetComponent<GroundCheck>();

            if (physics == null)
                physics = GetComponent<PhysicsComponent>();
            
            DefaultFov = playerCamera.Fov;
            // DebugWhiteboard.Instance.AddLabel(() => $"Ground speed: {GroundSpeed:F}");
        }

        public void Update()
        {
            playerCamera.Fov = GetCameraFOV(playerCamera.Fov);
            
            SprintMultiplier = ReallyIsSprinting() ? CurrentSettings.sprintMultiplier : 1;

            _isAccelerating = InputDirection != Vector2.zero;
            _currentVelocity = physics.Velocity;
        
            CalculateTargetVelocity();
        
            if (_isAccelerating || useSmoothStop == false || !_groundCheck.IsGrounded)
                ApplyConstantAcceleration(CurrentSettings);

            else if (useSmoothStop)
                ApplySmoothStop(CurrentSettings);
        }

        private void CalculateTargetVelocity()
        {
            Vector3 forwardVelocity = yawTransform.forward * InputDirection.y;
            Vector3 rightVelocity = yawTransform.right * InputDirection.x;
            Vector3 targetDirection = (forwardVelocity + rightVelocity).normalized;
            targetDirection += CalculateForwardSpeedMultiplier(targetDirection);
            
            _targetVelocity = targetDirection * speed;
            _targetVelocity.y = _currentVelocity.y;
        }
        
        private Vector3 CalculateForwardSpeedMultiplier(Vector3 targetDirection)
        {
            Vector3 forward = yawTransform.forward;
            float forwardSpeed = Vector3.Dot(targetDirection, forward);
                
            if (forwardSpeed > 0.1f)
                return forward * (forwardSpeed * (SprintMultiplier - 1));

            return Vector3.zero;
        }

        private void ApplyConstantAcceleration(Settings settings)
        {
            bool didReverse = _targetVelocity.x != 0 && _currentVelocity.x != 0 &&
                              // ReSharper disable once CompareOfFloatsByEqualityOperator
                              Mathf.Sign(_targetVelocity.x) != Mathf.Sign(_currentVelocity.x);
        
            float currentAcceleration = _isAccelerating ? settings.acceleration : settings.deceleration;

            float reverseMultiplier = didReverse ? settings.reactivity : 1;
            float maxDelta = currentAcceleration * reverseMultiplier * Time.deltaTime;
        
            physics.Velocity = Vector3.MoveTowards(_currentVelocity, _targetVelocity, maxDelta);
        }

        private void ApplySmoothStop(Settings settings)
        {
            Vector3 smoothedVelocity = _currentVelocity;

            float currentDeceleration = settings.deceleration * Time.deltaTime;
            smoothedVelocity.x -= smoothedVelocity.x * currentDeceleration;
            smoothedVelocity.z -= smoothedVelocity.z * currentDeceleration;

            physics.Velocity = smoothedVelocity;
        }
    }
}