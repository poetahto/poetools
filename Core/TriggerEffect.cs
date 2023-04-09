using UnityEngine;

namespace poetools.Core
{
    /// <summary>
    /// Provides easy logic for performing behaviors when an object enters a trigger.
    /// </summary>
    [RequireComponent(typeof(Trigger))]
    public abstract class TriggerEffect : MonoBehaviour
    {
        /// <summary>
        /// Called when an object enters the attached trigger.
        /// </summary>
        /// <param name="obj">The object that just entered.</param>
        protected virtual void HandleCollisionEnter(GameObject obj)
        {
        }

        /// <summary>
        /// Called when an object exits the attached trigger.
        /// </summary>
        /// <param name="obj">The object that just left.</param>
        protected virtual void HandleCollisionExit(GameObject obj)
        {
        }

        private void OnEnable()
        {
            var trigger = GetComponent<Trigger>();
            trigger.CollisionEnter += HandleCollisionEnter;
            trigger.CollisionExit += HandleCollisionExit;
        }

        private void OnDisable()
        {
            var trigger = GetComponent<Trigger>();
            trigger.CollisionEnter -= HandleCollisionEnter;
            trigger.CollisionExit -= HandleCollisionExit;
        }
    }
}
