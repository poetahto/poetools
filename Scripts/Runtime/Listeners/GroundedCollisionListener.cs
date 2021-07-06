using System;
using UnityEngine;
using UnityEngine.Events;

namespace Listeners
{
    [Serializable]
    public class SurfaceCollisionEvents
    {
        [Tooltip("Passes the collider which was collided with.")]
        public UnityEvent<Collider> onEnterCollision = new UnityEvent<Collider>();
        
        [Tooltip("Passes the collider which was exited.")]
        public UnityEvent<Collider> onExitCollision = new UnityEvent<Collider>();
    }
    
    public class GroundedCollisionListener : MonoBehaviour
    {
        [Header("Ground Check Settings")]
        
        [SerializeField] 
        private Vector3 gravityDirection = Vector3.down;
        
        [SerializeField] 
        private float slopeLimitDegrees = 45f;
        
        [SerializeField] 
        private float sphereCastRadius = 0.5f;
        
        [SerializeField] 
        private float sphereCastDistance = 1f;
        
        [Space]
        [SerializeField] 
        private SurfaceCollisionEvents collisionEvents = new SurfaceCollisionEvents();

        public bool IsGrounded { get; private set; }
        public bool IsGroundedIgnoreSlope { get; private set; }
        public Collider ConnectedCollider { get; private set; }
        public Vector3 ContactNormal { get; private set; }
        public SurfaceCollisionEvents CollisionEvents => collisionEvents;
        
        private RaycastHit _currentHitInfo;
        private bool _wasGroundedLastFrame;

        private void Update()
        {
            _wasGroundedLastFrame = IsGrounded;
            IsGrounded = CheckGrounded();
            
            CheckForCollisionEnter();
            CheckForCollisionExit();
        }

        private bool CheckGrounded()
        {
            Ray gravity = new Ray(transform.position, gravityDirection);

            if (Physics.SphereCast(gravity, sphereCastRadius, out _currentHitInfo, sphereCastDistance))
            {
                ContactNormal = _currentHitInfo.normal;
                IsGroundedIgnoreSlope = true;
                ConnectedCollider = _currentHitInfo.collider;
                return Vector3.Angle(Vector3.up, _currentHitInfo.normal) <= slopeLimitDegrees;
            }

            IsGroundedIgnoreSlope = false;
            return false;
        }

        private void CheckForCollisionEnter()
        {
            if (JustEntered)
                collisionEvents.onEnterCollision.Invoke(ConnectedCollider);
        }

        private void CheckForCollisionExit()
        {
            if (JustExited)
                collisionEvents.onExitCollision.Invoke(ConnectedCollider);
        }
        
        private bool JustEntered => IsGrounded && !_wasGroundedLastFrame;
        private bool JustExited => !IsGrounded && _wasGroundedLastFrame;
    }
}