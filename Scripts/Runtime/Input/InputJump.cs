using System;
using Listeners;
using UnityEngine;
using UnityEngine.Events;

namespace Input
{
    [Serializable]
    public class InputJumpEvents
    {
        [Tooltip("Passes the collider that was jumped from.")]
        public UnityEvent<Collider> onGroundJump = new UnityEvent<Collider>();
        
        [Tooltip("Passes current speed and remaining air jumps.")]
        public UnityEvent<float, int> onAirJump = new UnityEvent<float, int>();
    }
    
    public class InputJump : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private GroundedCollisionListener groundedCheck;

        [Header("Settings")] 
        [SerializeField] private JumpSettings settings;

        [Space]
        [SerializeField] private InputJumpEvents events = new InputJumpEvents(); 

        // ---- Jumping State ----
        private bool _coyoteAvailable;
        private bool _jumpQueued;
        private bool _holdingJump;
        private float _currentSpeed;
        private float _timeSinceJumpQueued;
        private float _timeSpentFalling;
        private int _remainingAirJumps;
    
        // ---- Custom Gravity State----
        private bool Rising => _currentSpeed > 0;
        private float StandardGravity => Rising ? settings.StandardGravityRising : settings.StandardGravityFalling;
        private float FastFallGravity => Rising ? settings.FastFallGravityRising : settings.FastFallGravityFalling;
        private float Gravity => settings.enableFastFall && _holdingJump == false ? FastFallGravity : StandardGravity;

        private void Start()
        {
            groundedCheck.CollisionEvents.onEnterCollision.AddListener(OnLand);
        }

        private void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER && !ENABLE_INPUT_SYSTEM
            ProvideJumpInputLegacy();
#endif
            UpdateJumpBuffer();
            ApplyGravity();
            
            if (_jumpQueued)
                TryToJump();
            
            ApplySpeed();
        }

        private void UpdateJumpBuffer()
        {
            if (_jumpQueued)
            {
                if (_timeSinceJumpQueued > settings.jumpBufferTime)
                    _jumpQueued = false;

                _timeSinceJumpQueued += Time.deltaTime;
            }
        }

        private void OnLand(Collider _)
        {
            _remainingAirJumps = settings.airJumps;
            _coyoteAvailable = true;
            _timeSpentFalling = 0f;
        }

        private void ApplyGravity()
        {
            if (groundedCheck.IsGroundedIgnoreSlope)
            {
                _currentSpeed = Mathf.Max(_currentSpeed, Gravity);
            }
            else
            {
                float deltaTime = Time.deltaTime;
                _currentSpeed += Gravity * deltaTime;
                _timeSpentFalling += deltaTime;
            }
        }

        private void ApplySpeed()
        {
            Vector3 targetVelocity = Vector3.up * _currentSpeed;

            if (groundedCheck.IsGrounded == false)
            {
                targetVelocity.x += (1f - groundedCheck.ContactNormal.y) * groundedCheck.ContactNormal.x;
                targetVelocity.z += (1f - groundedCheck.ContactNormal.y) * groundedCheck.ContactNormal.z;
            }
            
            controller.Move(targetVelocity * Time.deltaTime);
        }

        // Checks for conditions like coyote jumps, air jumps, ect.
        private void TryToJump()
        {
            if (groundedCheck.IsGrounded)
                Jump();

            else if (_coyoteAvailable && _timeSpentFalling < settings.coyoteTime)
                Jump();

            else if (_remainingAirJumps > 0)
            {
                Jump();
                --_remainingAirJumps;
            }
        }

        private void Jump()
        {
            CallJumpEvent();
        
            _currentSpeed = settings.JumpSpeed;
            _coyoteAvailable = false;
            _jumpQueued = false;
        }

        private void CallJumpEvent()
        {
            if (groundedCheck.IsGrounded)
                events.onGroundJump.Invoke(groundedCheck.ConnectedCollider);

            else events.onAirJump.Invoke(_currentSpeed, _remainingAirJumps);
        }

        #region Input Systems

#if ENABLE_INPUT_SYSTEM
        [JetBrains.Annotations.PublicAPI]
        public void ProvideJumpInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _holdingJump = !context.canceled;

            if (context.started)
            {
                _jumpQueued = true;
                _timeSinceJumpQueued = 0;
            }
        }
#endif
    
#if ENABLE_LEGACY_INPUT_MANAGER && !ENABLE_INPUT_SYSTEM
        private void ProvideJumpInputLegacy()
        {
            _holdingJump = UnityEngine.Input.GetButton("Jump") || 
                           settings.scrollToJump && UnityEngine.Input.GetAxisRaw("Mouse ScrollWheel") != 0;

            if (UnityEngine.Input.GetButtonDown("Jump") || 
                settings.scrollToJump && UnityEngine.Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            {
                _jumpQueued = true;
                _timeSinceJumpQueued = 0;
            }
        }
#endif

        #endregion
    }
}