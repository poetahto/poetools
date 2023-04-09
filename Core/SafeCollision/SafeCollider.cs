using UnityEngine;

namespace poetools.Core.SafeCollision
{
    public abstract class SafeCollider : MonoBehaviour
    {
        private Collider _collider;
        private TriggerEvents _trigger;

        public bool IsClear => _trigger.CurrentColliders.Count <= 0;

        private void Awake()
        {
            var triggerObject = new GameObject("Safe Trigger");

            triggerObject.transform.SetParent(transform);
            triggerObject.transform.localPosition = Vector3.zero;
            triggerObject.transform.localRotation = Quaternion.identity;

            _collider = GetCollider();
            _trigger = AddTrigger(triggerObject);
        }

        public void SafeEnable()
        {
            if (IsClear)
                _collider.enabled = true;
        }

        public void UnsafeEnable()
        {
            _collider.enabled = true;
        }

        public void Disable()
        {
            _collider.enabled = false;
        }

        public Collider Collider => _collider;

        protected abstract Collider GetCollider();
        protected abstract TriggerEvents AddTrigger(GameObject target);
    }
}
