using UnityEngine;

public class RigidbodyJump : JumpBase
{
    [SerializeField] private Rigidbody targetRigidbody;
    
    protected override Vector3 CurrentVelocity => targetRigidbody.velocity;
    
    protected override void ApplyVelocity(Vector3 velocity)
    {
        targetRigidbody.velocity = velocity;
    }
}