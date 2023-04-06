using poetools.Core.Tools;
using UnityEngine;

namespace poetools.player.Player.Crouching.Colliders
{
    public class CharacterCrouchingCollider : CrouchingCollider
    {
        [SerializeField]
        public CharacterController capsuleCollider;

        private Vector3 _originalCenter;
        private Vector3 _customCenter;
        private bool _centerInitialized;

        private void Awake()
        {
            EnsureIsInitialized();
        }

        private void EnsureIsInitialized()
        {
            if (!_centerInitialized)
            {
                _originalCenter = capsuleCollider.center;
                _customCenter = Vector3.zero;
                _centerInitialized = true;
            }
        }

        public override Vector3 Center
        {
            get
            {
                EnsureIsInitialized();
                return _customCenter;
            }
            set
            {
                EnsureIsInitialized();
                _customCenter = value;
                capsuleCollider.center = _originalCenter + _customCenter;
            }
        }

        public override float Height
        {
            get => capsuleCollider.height;
            set => capsuleCollider.height = value;
        }

        private RaycastHit[] _hitBuffer = new RaycastHit[50];

        public override bool Cast(Vector3 direction, float distance, out RaycastHit info)
        {
            EnsureIsInitialized();
            int hits = Physics.SphereCastNonAlloc(transform.position + transform.up * 0.5f, capsuleCollider.radius * 0.9f, direction, _hitBuffer, distance, ~LayerMask.GetMask("Ignore Raycast"));
            _hitBuffer.SortByNearest(hits);

            for (int i = 0; i < hits; i++)
            {
                if (_hitBuffer[i].collider != capsuleCollider)
                {
                    info = _hitBuffer[i];
                    return true;
                }
            }

            info = default;
            return false;
        }

        public override bool CanStand => !Cast(transform.up, Height * 0.9f, out _);
    }
}
