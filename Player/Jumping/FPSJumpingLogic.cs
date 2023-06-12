using poetools.Core;
using poetools.Core.Abstraction;
using UnityEngine;

namespace poetools.player.Player.Jumping
{
    public class FPSJumpingLogic
    {
        private Buffer _jumpBuffer = new Buffer();

        public float GetCurrentGravity(bool rising, bool holdingJump)
        {
            if (_settings.enableFastFall && holdingJump == false)
                return rising ? FastFallGravityRising : FastFallGravityFalling;

            return rising ? StandardGravityRising : StandardGravityFalling;
        }

        // Internal State

        private bool _coyoteAvailable;
        private bool _groundJumpAvailable;
        private int _remainingAirJumps;
        private bool _wasGrounded;

        private Vector3 CurrentVelocity
        {
            get => _hasPhysics ? _physics.Velocity : Vector3.zero;
            set
            {
                if (_hasPhysics)
                    _physics.Velocity = value;
            }
        }
        private bool IsGrounded => !_hasGroundCheck || _groundCheck.IsGrounded;
        private bool CoyoteAvailable => _coyoteAvailable && _groundCheck.AirTime < _settings.coyoteTime;
        public bool HoldingJump { get; set; }

        private IPhysicsComponent _physics;
        private JumpingSettings _settings;

        private Gravity _gravity;
        private GroundCheck _groundCheck;

        private bool _hasGravity;
        private bool _hasGroundCheck;
        private bool _hasPhysics;

        public FPSJumpingLogic(JumpingSettings settings)
        {
            _settings = settings;
            CalculateGravityAndSpeed();
        }

        public IPhysicsComponent Physics
        {
            get => _physics;
            set
            {
                _physics = value;
                _hasPhysics = _physics != null;
            }
        }

        public Gravity Gravity
        {
            get => _gravity;
            set
            {
                _gravity = value;
                _hasGravity = _gravity != null;
            }
        }

        public GroundCheck GroundCheck
        {
            get => _groundCheck;
            set
            {
                _groundCheck = value;
                _hasGroundCheck = _groundCheck != null;
            }
        }

        public float JumpSpeed { get; private set; }

        public float StandardGravityRising  {get; private set;}
        public float StandardGravityFalling {get; private set;}
        public float FastFallGravityRising  {get; private set;}
        public float FastFallGravityFalling {get; private set;}

        public void CalculateGravityAndSpeed()
        {
            // Gravity math based on the GDC talk found here:
            // https://youtu.be/hG9SzQxaCm8?t=794

            JumpSpeed = 2 * _settings.jumpHeight * _settings.assumedInitialSpeed / (_settings.jumpDistance * _settings.standardSkew);

            StandardGravityRising = 2 * _settings.jumpHeight * Mathf.Pow(_settings.assumedInitialSpeed, 2)
                                    / Mathf.Pow(_settings.jumpDistance * _settings.standardSkew, 2);

            StandardGravityFalling = 2 * _settings.jumpHeight * Mathf.Pow(_settings.assumedInitialSpeed, 2)
                                     / Mathf.Pow(_settings.jumpDistance * (1 - _settings.standardSkew), 2);

            FastFallGravityRising = 2 * _settings.minHeight * Mathf.Pow(_settings.assumedInitialSpeed, 2)
                                    / Mathf.Pow(_settings.minDistance * _settings.fastFallSkew, 2);

            FastFallGravityFalling = 2 * _settings.minHeight * Mathf.Pow(_settings.assumedInitialSpeed, 2)
                                     / Mathf.Pow(_settings.minDistance * (1 - _settings.fastFallSkew), 2);
        }

        private void RefreshJumps()
        {
            _remainingAirJumps = _settings.airJumps;
            _coyoteAvailable = true;
            _groundJumpAvailable = true;
        }

        private void TryToJump()
        {
            if (ShouldJump())
                ApplyJump();
        }

        private bool ShouldJump()
        {
            if ((IsGrounded || CoyoteAvailable) && _groundJumpAvailable)
            {
                _groundJumpAvailable = false;
                return true;
            }

            if (!IsGrounded && _remainingAirJumps > 0)
            {
                _remainingAirJumps--;
                return true;
            }

            return false;
        }

        private void ApplyJump()
        {
            Vector3 v = CurrentVelocity;
            v.y = -_gravity.downDirection.y * JumpSpeed;
            CurrentVelocity = v;

            _coyoteAvailable = false;
            _jumpBuffer.Clear();
        }

        public void Tick()
        {
            if (_jumpBuffer.IsQueued(_settings.jumpBufferTime))
                TryToJump();
        }

        public void Jump()
        {
            _jumpBuffer.Queue();
        }

        public void PhysicsTick()
        {
            if (_hasGravity)
                UpdateGravity(HoldingJump);
        }

        private void UpdateGravity(bool holdingJump)
        {
            bool rising = CurrentVelocity.y > 0;
            _gravity.amount = GetCurrentGravity(rising, holdingJump);

            if (!_wasGrounded && IsGrounded)
                RefreshJumps();

            _wasGrounded = IsGrounded;
        }
    }
}
