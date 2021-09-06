using UnityEngine;

public class CharacterControllerMovement : MovementBase
{
    [Header("Character Controller Settings")]
    [SerializeField] private CharacterController controller;

    private Vector3 _currentVelocity;
    protected override Vector3 CurrentVelocity => _currentVelocity;

    protected override void ApplyVelocity(Vector3 velocity)
    {
        controller.Move(velocity * Time.deltaTime);
        
        Vector3 resultantVelocity = controller.velocity;
        _currentVelocity.x = resultantVelocity.x;
        _currentVelocity.z = resultantVelocity.z;
    }
}
