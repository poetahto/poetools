using Listeners;
using UnityEngine;

namespace Input
{
    public abstract class InputMoveBase : MonoBehaviour
    {
        [Header("Base Settings")]
        [SerializeField] private GroundedCollisionListener groundCheck;
        [SerializeField] private MovementType movement;
        [SerializeField] private bool showDebug;
        
        private Transform _transform;
        private Vector2 _directionalInput;
        private Vector3 _targetVelocity;
        private bool _isSprinting;
        
        protected Vector3 CurrentVelocity { get; set; }

        private void Awake()
        {
            _transform = transform;
        }

        protected virtual void Update()
        {
            UpdateInputs();
            UpdateCurrentVelocity();
        }

        private void UpdateCurrentVelocity()
        {
            Vector3 targetVelocity = 
                movement.CalculateVelocity(CurrentVelocity, GetMovementDirection(), groundCheck);

            if (groundCheck.IsGrounded == false)
                targetVelocity.y = 0;

            CurrentVelocity = targetVelocity;
        }

        private Vector3 GetMovementDirection()
        {
            Vector3 rightVelocity = _transform.right * _directionalInput.x;
            Vector3 forwardVelocity = _transform.forward * _directionalInput.y;
            Vector3 totalVelocity = Vector3.ClampMagnitude(forwardVelocity + rightVelocity, 1);
            return ProjectOnContactPlane(totalVelocity);
        }
        
        private Vector3 ProjectOnContactPlane(Vector3 vector)
        {
            return vector - groundCheck.ContactNormal * Vector3.Dot(vector, groundCheck.ContactNormal);
        }
       
        private void UpdateInputs()
        {
            float horizontal = UnityEngine.Input.GetAxisRaw("Horizontal");
            float vertical = UnityEngine.Input.GetAxisRaw("Vertical");
            _directionalInput = new Vector2(horizontal, vertical);

            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
                _isSprinting = true;

            else if (UnityEngine.Input.GetKeyUp(KeyCode.LeftShift))
                _isSprinting = false;
        }
        
        private void OnGUI()
        {
            if (showDebug)
            {
                GUILayout.Label($"Current Velocity: {CurrentVelocity}");
                GUILayout.Label($"Directional Input: {_directionalInput}");
                GUILayout.Label($"Sprinting: {_isSprinting}");
            }
        }
    }
}