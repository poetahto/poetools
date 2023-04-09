using System;
using UnityEngine;

namespace poetools.Core
{
    /// <summary>
    /// An object that tracks when other objects enter its collision.
    /// </summary>
    public abstract class Trigger : MonoBehaviour
    {
        /// <summary>
        /// Called when an object enters this trigger.
        /// </summary>
        public abstract event Action<GameObject> CollisionEnter;

        /// <summary>
        /// Called when an object exits this trigger.
        /// </summary>
        public abstract event Action<GameObject> CollisionExit;
    }
}
