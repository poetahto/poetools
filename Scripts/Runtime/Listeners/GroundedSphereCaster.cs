using UnityEngine;

namespace Listeners
{
    public class GroundedSphereCaster : GroundCheck
    {
        [SerializeField] protected float sphereCastRadius = 0.5f;
        [SerializeField] protected float sphereCastDistance = 1f;

        public override bool IsGrounded => _isGrounded;
        public override Vector3 ContactNormal => _contactNormal;
        public override Collider ConnectedCollider => _connectedCollider;

        private bool _isGrounded;
        private Vector3 _contactNormal;
        private Collider _connectedCollider;
        private RaycastHit _currentHitInfo;

        protected override void Update()
        {
            RaycastCheckIsGrounded();

            base.Update();
        }

        private void RaycastCheckIsGrounded()
        {
            Ray gravity = new Ray(transform.position, gravityDirection);

            if (Physics.SphereCast(gravity, sphereCastRadius, out _currentHitInfo, sphereCastDistance))
            {
                _contactNormal = _currentHitInfo.normal;
                _connectedCollider = _currentHitInfo.collider;
                _isGrounded = Vector3.Angle(Vector3.up, _currentHitInfo.normal) <= slopeLimitDegrees;
                return;
            }

            _isGrounded = false;
        }
    }
}