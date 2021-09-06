using UnityEngine;

public abstract class MovementSettings : ScriptableObject
{
    [SerializeField] private float baseMovementSpeed = 5f;

    public float BaseMovementSpeed => baseMovementSpeed;
    protected GroundCheck GroundCheck;
    
    public void Initialize(GroundCheck groundCheck)
    {
        GroundCheck = groundCheck;
    }
    
    public abstract Vector3 CalculateVelocity(Vector3 currentVelocity, Vector3 movementDirection, float currentMaxSpeed);
}