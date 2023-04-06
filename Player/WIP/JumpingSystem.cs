// using System;
// using poetools.Abstraction;
// using UnityEngine;
// using UnityEngine.Events;
//
// namespace poetools.Runtime.Systems
// {
//     /// <summary>
//     /// Allows a Rigidbody to use advanced jumping mechanics like coyote time and air jumping.
//     /// </summary>
//
//     public class JumpingSystem : MonoBehaviour
//     {
//         [Header("Settings")]
//     
//         [SerializeField] 
//         private JumpSettings jumpSettings;
//     
//         [SerializeField]
//         [Tooltip("Writes debugging info to the console.")]
//         private bool showDebug;
//     
//         [Header("Dependencies")] 
//     
//         [SerializeField] 
//         [Tooltip("The rigidbody to apply our jumping force to.")]
//         private PhysicsComponent physics;
//     
//         [SerializeField]
//         [Tooltip("Controlled at runtime for special jumping effects, like holding to jump longer.")]
//         private Gravity gravity;
//     
//         [Space(20)]
//     
//         [SerializeField] 
//         [Tooltip("Invoked when we jump.")]
//         public UnityEvent onJump;
//         
//         // Internal State
//     
//         private bool _coyoteAvailable;
//         private bool _groundJumpAvailable;
//         private int _remainingAirJumps;
//
//         private bool CoyoteAvailable => _coyoteAvailable && physics.AirTime < jumpSettings.CoyoteTime;
//         public JumpSettings JumpSettings => jumpSettings;
//     
//         private bool _wasGrounded;
//
//         private void Start()
//         {
//             if (physics.IsGrounded)
//                 RefreshJumps();
//         }
//
//         private void Update()
//         {
//             UpdateGravity(Input.GetKey(KeyCode.Space));
//             
//             if (physics.IsGrounded && !_wasGrounded)
//                 RefreshJumps();
//
//             if (physics.IsGrounded && Time.time - _bufferRequestTime <= jumpSettings.JumpBufferTime)
//             {
//                 Jump();
//                 _bufferRequestTime = float.NegativeInfinity;
//             }
//             
//             _wasGrounded = physics.IsGrounded;
//         }
//
//         private void RefreshJumps()
//         {
//             if (showDebug) 
//                 print("Jumps were refreshed!");
//         
//             _remainingAirJumps = jumpSettings.AirJumps;
//             _coyoteAvailable = true;
//             _groundJumpAvailable = true;
//         }
//
//         public void Jump()
//         {
//             if (ShouldJump())
//                 ApplyJump();
//
//             else _bufferRequestTime = Time.time;
//         }
//
//         private float _bufferRequestTime = float.NegativeInfinity;
//
//         private bool ShouldJump()
//         {
//             if ((physics.IsGrounded || CoyoteAvailable) && _groundJumpAvailable)
//             {
//                 if (showDebug) 
//                     print(physics.IsGrounded ? "Jumped: Normal" : "Jumped: Coyote");
//
//                 _groundJumpAvailable = false;
//                 return true;
//             }
//
//             if (!physics.IsGrounded && _remainingAirJumps > 0)
//             {
//                 if (showDebug) 
//                     print("Jumped: Air");
//             
//                 _remainingAirJumps--;
//                 return true;
//             }
//
//             return false;
//         }
//
//         private void ApplyJump()
//         {
//             Vector3 currentVelocity = physics.Velocity;
//             currentVelocity.y = jumpSettings.JumpSpeed;
//             physics.Velocity = currentVelocity;
//         
//             _coyoteAvailable = false;
//             onJump.Invoke();
//         }
//
//         public void UpdateGravity(bool holdingJump)
//         {
//             bool rising = physics.Velocity.y > 0;
//             float currentGravity = jumpSettings.GetCurrentGravity(rising, holdingJump);
//             
//             gravity.amount = physics.IsGrounded 
//                 ? 0 
//                 : currentGravity;
//         }
//     }
// }