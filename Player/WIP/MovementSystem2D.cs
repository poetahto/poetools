// using poetools.Abstraction;
// using UnityEngine;
//
// namespace BeastWhisperer.Prototype.Platformer
// {
//     public class MovementSystem2D : MonoBehaviour
//     {
//         public MovementSettings groundedSettings;
//         public MovementSettings airborneSettings;
//         public PhysicsComponent physics;
//         
//         public bool ignoreY;
//         
//         private Rigidbody2D _rigidbody;
//         private Vector2 _targetDirection;
//
//         private void Awake()
//         {
//             _rigidbody = GetComponentInChildren<Rigidbody2D>();
//         }
//
//         private void Update()
//         {
//             UpdateMovement(physics.IsGrounded ? groundedSettings : airborneSettings);
//         }
//
//         public void UpdateMovement(MovementSettings settings)
//         {
//             Vector2 targetVelocity = _targetDirection * settings.speed;
//             
//             if (ignoreY)
//                 targetVelocity.y = _rigidbody.velocity.y;
//             
//             _rigidbody.velocity = Vector2.MoveTowards(_rigidbody.velocity, targetVelocity, settings.acceleration * Time.deltaTime);
//         }
//
//         public Vector2 TargetDirection
//         {
//             get => _targetDirection;
//             set => _targetDirection = value;
//         }
//     }
// }