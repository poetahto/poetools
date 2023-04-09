// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;
//
// namespace poetools.Core
// {
//     /// <summary>
//     /// Wraps a trigger Collider and provides UnityEvents for OnTriggerEnter / Exit / Stay
//     /// </summary>
//
//     [RequireComponent(typeof(Collider))]
//     public class Trigger : MonoBehaviour
//     {
//         [Header("Settings")]
//
//         [SerializeField]
//         [Tooltip("Which layers should be ignored when checking collisions.")]
//         public LayerMask excludeLayers;
//
//         public List<GameObject> excludeObjects = new List<GameObject>();
//
//         [SerializeField]
//         [Tooltip("Should this trigger disable itself after one activation?")]
//         private bool oneShot;
//
//         [Space(20f)]
//
//         [SerializeField]
//         [Tooltip("Called when a collider enters this trigger.")]
//         private UnityEvent<Collider> collisionEnter = new UnityEvent<Collider>();
//
//         [SerializeField]
//         [Tooltip("Called when a collider exits this trigger.")]
//         private UnityEvent<Collider> collisionExit = new UnityEvent<Collider>();
//
//         // Internal State
//
//         public IReadOnlyCollection<Rigidbody> Rigidbodies => _rigidbodies;
//         public IReadOnlyCollection<Collider> Colliders => _colliders;
//
//         private HashSet<Rigidbody> _rigidbodies = new HashSet<Rigidbody>();
//         private HashSet<Collider> _colliders = new HashSet<Collider>();
//
//         public Collider Collider => _collider;
//
//         // Methods
//
//         private Collider _collider;
//
//         private void Awake()
//         {
//             _collider = GetComponent<Collider>();
//             _collider.isTrigger = true;
//         }
//
//         private void OnValidate()
//         {
//             // Ensure that the collider is always set to be a trigger.
//             GetComponent<Collider>().isTrigger = true;
//         }
//
//         // Note: enter / exit can sometime miss objects, so we cannot rely on it for updating our lists.
//         // However, its convenient for sending UnityEvents to the editor.
//
//         private void OnTriggerEnter(Collider other)
//         {
//             if (IsValidCollider(other) && enabled)
//             {
//                 if (showDebug)
//                     Debug.Log($"Entered trigger: {other.gameObject.name}", gameObject);
//
//                 collisionEnter?.Invoke(other);
//             }
//         }
//
//         private void OnTriggerExit(Collider other)
//         {
//             if (IsValidCollider(other) && enabled)
//             {
//                 if (showDebug)
//                     Debug.Log($"Exited trigger: {other.gameObject.name}", gameObject);
//
//                 collisionExit.Invoke(other);
//
//                 if (oneShot)
//                     enabled = false;
//             }
//         }
//
//         private void FixedUpdate()
//         {
//             // Every physics update, we clear our state and repopulate with new data from OnTriggerStay.
//
//             _rigidbodies.Clear();
//             _colliders.Clear();
//         }
//
//         private void OnTriggerStay(Collider other)
//         {
//             if (IsValidCollider(other) && enabled)
//             {
//                 _colliders.Add(other);
//
//                 if (other.attachedRigidbody != null)
//                     _rigidbodies.Add(other.attachedRigidbody);
//             }
//         }
//
//         private bool IsValidCollider(Collider other)
//         {
//             // weird bit-mask code for checking if the object is a valid layer
//             bool isExcludeLayer = excludeLayers.value == (excludeLayers.value | (1 << other.gameObject.layer));
//             bool isTrigger = other.isTrigger;
//             bool isExcludeObject = excludeObjects.Contains(other.gameObject);
//
//             return !isTrigger && !isExcludeLayer && !isExcludeObject;
//         }
//
//         #region Debug
//
//         [SerializeField]
//         [Tooltip("Writes debug information to the screen.")]
//         public bool showDebug;
//
//         private void OnGUI()
//         {
//             if (showDebug)
//             {
//                 GUILayout.Label($"Rigidbodies: {Rigidbodies.Count}");
//
//                 foreach (var occupiedRigidbody in Rigidbodies)
//                     GUILayout.Label(occupiedRigidbody.name);
//
//                 GUILayout.Label($"Colliders: {Colliders.Count}");
//
//                 foreach (var occupiedCollider in Colliders)
//                     GUILayout.Label(occupiedCollider.name);
//             }
//         }
//
//         #endregion
//
//     }
// }
