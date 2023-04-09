using poetools.Core;
using poetools.player.Player.Crouching.Colliders;
using poetools.player.Player.Crouching.States;
using UnityEngine;

namespace poetools.player.Player.Crouching
{
    public class FPSCrouchingLogic
    {
        public readonly StandingState Standing;
        public readonly CrouchingGroundState CrouchingGround;
        public readonly CrouchingAirState CrouchingAir;

        public readonly Transform RawCameraTransform;
        public readonly TriggerEvents HeadRoom;
        public readonly CrouchingSettings Settings;
        public readonly Transform Parent;

        private ICrouchingCollider _crouchingCollider;
        private Transform _steadyBase;
        private GroundCheck _groundCheck;
        private Transform _smoothedCrouchTransform;

        private bool _hasCrouchingCollider;
        private bool _hasSteadyBase;
        private bool _hasGroundCheck;
        private bool _hasSmoothedCrouchTransform;

        private CrouchState _currentCrouchState;

        public FPSCrouchingLogic(CrouchingSettings settings, Transform parent)
        {
            Settings = settings;
            Parent = parent;

            // Creating the camera follow target.
            RawCameraTransform = new GameObject($"Camera Target (Added by {nameof(FPSCrouchingLogic)})").transform;
            RawCameraTransform.SetParent(Parent);
            RawCameraTransform.position = SmoothedCrouchPosition;

            // Creating the head-room tracker.
            var headRoomObj = new GameObject("Head Room", typeof(BoxCollider), typeof(TriggerEvents), typeof(Rigidbody));
            headRoomObj.transform.SetParent(Parent);
            headRoomObj.layer = LayerMask.NameToLayer("Ignore Raycast");
            headRoomObj.transform.position = Parent.position;
            HeadRoom = headRoomObj.GetComponent<TriggerEvents>();
            var headRoomCollider = headRoomObj.GetComponent<BoxCollider>();
            headRoomCollider.isTrigger = true;
            headRoomCollider.size = new Vector3(0.9f, (settings.standingHeight - settings.crouchHeight) * 1.1f, 0.9f);
            headRoomCollider.center = Vector3.down * ((settings.standingHeight - settings.crouchHeight) * 1.1f / 2);
            var rigidbody = headRoomObj.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            var trigger = headRoomObj.GetComponent<TriggerEvents>();
            trigger.excludeColliders.AddRange(parent.GetComponents<Collider>());

            // Initializing the states.
            Standing = new StandingState(this);
            CrouchingGround = new CrouchingGroundState(this);
            CrouchingAir = new CrouchingAirState(this);
            TransitionTo(Standing);
        }

        public ICrouchingCollider CrouchingCollider
        {
            get => _crouchingCollider;
            set
            {
                _crouchingCollider = value;
                _hasCrouchingCollider = _crouchingCollider != null;
            }
        }

        public Transform SmoothedCrouchTransform
        {
            get => _smoothedCrouchTransform;
            set
            {
                _smoothedCrouchTransform = value;
                _hasSmoothedCrouchTransform = _smoothedCrouchTransform != null;
            }
        }

        public Transform SteadyBase
        {
            get => _steadyBase;
            set
            {
                _steadyBase = value;
                _hasSteadyBase = _steadyBase != null;
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

        public Vector3 SmoothedCrouchPosition
        {
            get => _hasSmoothedCrouchTransform ? _smoothedCrouchTransform.position : Vector3.zero;
            set
            {
                if (_hasSmoothedCrouchTransform)
                    _smoothedCrouchTransform.position = value;
            }
        }

        public Vector3 SteadyBasePosition
        {
            get => _hasSteadyBase ? _steadyBase.position : Vector3.zero;
            set
            {
                if (_hasSteadyBase)
                    _steadyBase.position = value;
            }
        }

        public float ColliderHeight
        {
            get => _hasCrouchingCollider ? _crouchingCollider.Height : 0;
            set
            {
                if (_hasCrouchingCollider)
                    _crouchingCollider.Height = value;
            }
        }

        public Vector3 ColliderCenter
        {
            get => _hasCrouchingCollider ? _crouchingCollider.Center : Vector3.zero;
            set
            {
                if (_hasCrouchingCollider)
                    _crouchingCollider.Center = value;
            }
        }

        public bool CanStand => !_hasCrouchingCollider || _crouchingCollider.CanStand;

        public bool WantsToCrouch { get; set; }
        public bool IsCrouching => _currentCrouchState != Standing;
        public bool IsGrounded => !_hasGroundCheck || _groundCheck.IsGrounded;

        public bool Cast(Vector3 direction, float distance, out RaycastHit info)
        {
            if (_hasCrouchingCollider)
                return _crouchingCollider.Cast(direction, distance, out info);

            info = default;
            return false;
        }

        public void PhysicsTick()
        {
            _currentCrouchState?.PhysicsTick();
        }

        public void Tick()
        {
            // Lerp the camera to follow the target. The target has no smoothing applied.
            var cur = SmoothedCrouchPosition;
            cur.y = Mathf.Lerp(cur.y, RawCameraTransform.position.y, Settings.crouchingSpeed * Time.deltaTime);
            SmoothedCrouchPosition = cur;

            _currentCrouchState?.Tick();
        }

        public void TransitionTo(CrouchState crouchState)
        {
            _currentCrouchState?.Exit();
            _currentCrouchState = crouchState;
            _currentCrouchState?.Enter();
        }

        public void StandUp()
        {
            ColliderHeight = Settings.standingHeight;
            ColliderCenter = Vector3.zero;
        }

        public void CrouchDown()
        {
            ColliderHeight = Settings.crouchHeight;
            ColliderCenter = Vector3.down * ((Settings.standingHeight - Settings.crouchHeight) / 2);
        }
    }
}
