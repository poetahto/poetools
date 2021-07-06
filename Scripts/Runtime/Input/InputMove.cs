using System;
using Listeners;
using UnityEngine;
using UnityEngine.Events;

//TODO: Add support for footstep sounds, maybe in another class

namespace Input
{
    [Serializable]
    public class InputMoveEvents
    {
        public UnityEvent onSprintStart = new UnityEvent();
        public UnityEvent onSprintCancel = new UnityEvent();
    }
    
    public class InputMove : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private GroundedCollisionListener groundedCheck;

        [Header("Settings")] 
        [SerializeField] private MoveSettings settings;

        [Space] 
        [SerializeField] private InputMoveEvents events;
        
        private Transform _transform;
        private Vector2 _directionalInput;
        private Vector3 _currentVelocity;
        private Vector3 _targetVelocity;
        private bool _isSprinting;
        private bool _wasGroundedLastFrame;
        private int _framesSpentOnGround;

        private float MovementSpeed => _isSprinting && settings.useSprinting
            ? settings.maxSprintingSpeed
            : settings.useSourceMovement ? settings.maxGroundSpeed : settings.maxMovementSpeed;

        private void Awake()
        {
            _transform = transform;
        }

        public void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER && !ENABLE_INPUT_SYSTEM
            ProvideMovementInputLegacy();
#endif

            _currentVelocity = settings.useSourceMovement 
                ? UpdateVelocitySource() 
                : UpdateVelocityStandard();
        
            UpdateGroundedState();

            if (groundedCheck.IsGrounded == false)
                _currentVelocity.y = 0;
            
            controller.Move(_currentVelocity * Time.deltaTime);
            
            Vector3 resultantVelocity = controller.velocity;
            _currentVelocity.x = resultantVelocity.x;
            _currentVelocity.z = resultantVelocity.z;
        }

        private Vector3 UpdateVelocityStandard()
        {
            float acceleration = groundedCheck.IsGrounded 
                ? settings.StandardGroundedAcceleration 
                : settings.StandardAirborneAcceleration;
            
            _targetVelocity = GetMovementDirection() * MovementSpeed;
            return Vector3.MoveTowards(_currentVelocity, _targetVelocity, acceleration);
        }

        private Vector3 GetMovementDirection()
        {
            Vector3 xVelocity = _transform.right * _directionalInput.x;
            Vector3 yVelocity = _transform.forward * _directionalInput.y;
            Vector3 velocity = Vector3.ClampMagnitude(yVelocity + xVelocity, 1);
            return ProjectOnContactPlane(velocity);
        }

        private void UpdateGroundedState()
        {
            _wasGroundedLastFrame = groundedCheck.IsGrounded;
        
            if (groundedCheck.IsGrounded)
                ++_framesSpentOnGround;

            if (groundedCheck.IsGrounded && _wasGroundedLastFrame != groundedCheck.IsGrounded)
                _framesSpentOnGround = 0;
        }

        private void StartSprinting()
        {
            _isSprinting = true;
            events.onSprintStart.Invoke();
        }

        private void StopSprinting()
        {
            _isSprinting = false;
            events.onSprintCancel.Invoke();
        }
        
        private Vector3 ProjectOnContactPlane(Vector3 vector)
        {
            return vector - groundedCheck.ContactNormal * Vector3.Dot(vector, groundedCheck.ContactNormal);
        }

        #region Source Movement Calculations

        private Vector3 UpdateVelocitySource()
        {
            return groundedCheck.IsGrounded
                ? MoveGround()
                : MoveAir();
        }

        private Vector3 MoveGround()
        {
            float speed = _currentVelocity.magnitude;
            if (speed != 0 && _framesSpentOnGround > settings.noFrictionFrameCount)
            {
                float drop = speed * settings.friction * Time.deltaTime;
                _currentVelocity *= Mathf.Max(speed - drop, 0) / speed;
            }

            return Accelerate(_currentVelocity, settings.groundAcceleration, MovementSpeed);
        }

        private Vector3 MoveAir()
        {
            return Accelerate(_currentVelocity, settings.airAcceleration, settings.maxAirSpeed);
        }

        private Vector3 Accelerate(Vector3 oldVelocity, float accelerate, float maxVelocity)
        {
            Vector3 direction = GetMovementDirection();
            float projVel = Vector3.Dot(oldVelocity, direction);
            float accelVel = accelerate * Time.deltaTime;

            if (projVel + accelVel > maxVelocity)
                accelVel = Mathf.Max(maxVelocity - projVel, 0);

            return _currentVelocity + direction * accelVel;
        }

        #endregion
        
        #region Input Systems

#if ENABLE_INPUT_SYSTEM
        [JetBrains.Annotations.PublicAPI]
        public void ProvideMovementInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _directionalInput = context.ReadValue<Vector2>();
        }
        
        [JetBrains.Annotations.PublicAPI]
        public void ProvideSprintInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.started)
                StartSprinting();

            if (context.canceled)
                StopSprinting();
        }
#endif
        
#if ENABLE_LEGACY_INPUT_MANAGER && !ENABLE_INPUT_SYSTEM
        private void ProvideMovementInputLegacy()
        {
            float horizontal = UnityEngine.Input.GetAxisRaw("Horizontal");
            float vertical = UnityEngine.Input.GetAxisRaw("Vertical");
            _directionalInput = new Vector2(horizontal, vertical);
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
                StartSprinting();
            
            else if (UnityEngine.Input.GetKeyUp(KeyCode.LeftShift))
                StopSprinting();
        }
#endif
        
        #endregion
    }
}