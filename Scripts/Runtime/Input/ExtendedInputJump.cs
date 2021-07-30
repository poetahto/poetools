using Listeners;
using UnityEngine;

namespace Input
{
    public class ExtendedInputJump : MonoBehaviour
    {
        [SerializeField] private GroundedCollisionListener groundCheck;
        [SerializeField] private JumpSettings settings;

        // ---- Private State ----
        private bool _coyoteAvailable;
        private bool _jumpQueued;
        private bool _holdingJump;
        private float _currentSpeed;
        private float _timeSinceJumpQueued;
        private float _timeSpentFalling;
        private int _remainingAirJumps;
        private bool Rising => _currentSpeed > 0;
        private float StandardGravity => Rising ? settings.StandardGravityRising : settings.StandardGravityFalling;
        private float FastFallGravity => Rising ? settings.FastFallGravityRising : settings.FastFallGravityFalling;
        
        // ---- Public State ----
        public float Gravity => settings.enableFastFall && _holdingJump == false ? FastFallGravity : StandardGravity;
        public JumpSettings Settings => settings;
        public GroundedCollisionListener GroundCheck => groundCheck;
        
        private IJump JumpType { get; set; }

        private void Start()
        {
            if (TryGetComponent<CharacterController>(out var controller))
                JumpType = new CharacterControllerJump(this, controller);
            
            groundCheck.CollisionEvents.onEnterCollision.AddListener(OnLand);
        }

        private void Update()
        {
            UpdateInput();
            UpdateJumpState();
            
            if (ShouldJump())
                Jump();
            
            JumpType.Update();
        }
        
        private void UpdateInput()
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
        
        private void UpdateJumpState()
        {
            if (GroundCheck.IsGrounded)
                _timeSpentFalling += Time.deltaTime;
            
            // Manage Jump Buffer
            if (_jumpQueued)
            {
                if (_timeSinceJumpQueued > settings.jumpBufferTime)
                    _jumpQueued = false;

                _timeSinceJumpQueued += Time.deltaTime;
            }
        }

        private void OnLand(Collider col)
        {
            _remainingAirJumps = settings.airJumps;
            _coyoteAvailable = true;
            _timeSpentFalling = 0f;
        }
        
        // Checks for conditions like coyote jumps, air jumps, ect.
        private bool ShouldJump()
        {
            if (!_jumpQueued)
                return false;
                
            if (groundCheck.IsGrounded)
                return true;

            if (_coyoteAvailable && _timeSpentFalling < settings.coyoteTime)
                return true;

            return _remainingAirJumps > 0;
        }

        private void Jump()
        {
            _coyoteAvailable = false;
            _jumpQueued = false;
            JumpType.Jump();
        }
    }

    public interface IJump
    {
        void Jump();
        void Update();
    }

    public class CharacterControllerJump : IJump
    {
        private ExtendedInputJump _inputJump;
        private CharacterController _controller;
        private float _currentSpeed;

        public CharacterControllerJump(ExtendedInputJump inputJump, CharacterController controller)
        {
            _inputJump = inputJump;
            _controller = controller;
        }

        public void Jump()
        {
            _currentSpeed = _inputJump.Settings.JumpSpeed;
        }

        public void Update()
        {
            ApplyGravity();
            ApplySpeed();
        }
        
        private void ApplyGravity()
        {
            if (_inputJump.GroundCheck.IsGroundedIgnoreSlope)
            {
                _currentSpeed = Mathf.Max(_currentSpeed, _inputJump.Gravity);
            }
            else
            {
                _currentSpeed += _inputJump.Gravity * Time.deltaTime;
            }
        }
        
        private void ApplySpeed()
        {
            Vector3 targetVelocity = Vector3.up * _currentSpeed;

            if (_inputJump.GroundCheck.IsGrounded == false)
            {
                targetVelocity.x += (1f - _inputJump.GroundCheck.ContactNormal.y) * _inputJump.GroundCheck.ContactNormal.x;
                targetVelocity.z += (1f - _inputJump.GroundCheck.ContactNormal.y) * _inputJump.GroundCheck.ContactNormal.z;
            }
            
            _controller.Move(targetVelocity * Time.deltaTime);
        }
    }
}