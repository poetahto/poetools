using poetools.Core.Tools;
using UnityEngine;

namespace poetools.player.Player.Crouching.Colliders
{
    public class BoxCrouchingCollider : CrouchingCollider
    {
        [SerializeField]
        private BoxCollider boxCollider;

        private Vector3 _originalCenter;
        private Vector3 _customCenter;
        private bool _centerInitialized;
        private RaycastHit[] _hitBuffer = new RaycastHit[50];

        private void EnsureIsInitialized()
        {
            if (!_centerInitialized)
            {
                _originalCenter = boxCollider.center;
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
                boxCollider.center = _originalCenter + _customCenter;
            }
        }

        public override float Height
        {
            get => boxCollider.size.y;
            set
            {
                Vector3 size = boxCollider.size;
                size.y = value;
                boxCollider.size = size;
            }
        }

        public override bool CanStand => !Cast(transform.up, Height * 0.9f, out _);

        public override bool Cast(Vector3 direction, float distance, out RaycastHit info)
        {
            EnsureIsInitialized();
            int hits = Physics.BoxCastNonAlloc(transform.position + transform.up * 0.5f, boxCollider.size * (0.5f * 0.9f), direction, _hitBuffer, Quaternion.identity, distance, ~LayerMask.GetMask("Ignore Raycast"));
            _hitBuffer.SortByNearest(hits);

            for (int i = 0; i < hits; i++)
            {
                if (_hitBuffer[i].collider != boxCollider)
                {
                    info = _hitBuffer[i];
                    return true;
                }
            }

            info = default;
            return false;
        }
    }
}
