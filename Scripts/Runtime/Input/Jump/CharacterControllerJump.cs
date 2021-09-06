using UnityEngine;

public class CharacterControllerJump : JumpBase
{
    [Header("Dependencies")]
    [SerializeField] private CharacterController controller;

    // ---- Abstract Implementation ----

    protected override Vector3 CurrentVelocity => controller.velocity;

    protected override void ApplyVelocity(Vector3 velocity)
    {
        if (groundCheck.IsGrounded == false)
        {
            velocity.x += (1f - groundCheck.ContactNormal.y) * groundCheck.ContactNormal.x;
            velocity.z += (1f - groundCheck.ContactNormal.y) * groundCheck.ContactNormal.z;
        }
        
        controller.Move(velocity * Time.deltaTime);
    }
}