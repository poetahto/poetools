using UnityEngine;

[CreateAssetMenu]
public class StandardMovementSettings : MovementSettings
{
    [Header("Settings")]
    [SerializeField] public float groundedAcceleration = 0.125f;
    [SerializeField] public float groundedDeceleration = 0.125f;
    [SerializeField] public float airborneAcceleration = 0.3f;
    [SerializeField] public float airborneDeceleration = 0.3f;

    private Vector3 _targetVelocity;
    private float _currentMaxSpeed;

    public override Vector3 CalculateVelocity(Vector3 currentVelocity, Vector3 movementDirection, float currentMaxSpeed)
    {
        _currentMaxSpeed = currentMaxSpeed;
        _targetVelocity = movementDirection * _currentMaxSpeed;
        _targetVelocity.y = currentVelocity.y;

        float acceleration = GroundCheck.IsGrounded 
            ? IsAccelerating ? CalculateAcceleration(groundedAcceleration) : CalculateAcceleration(groundedDeceleration)
            : IsAccelerating ? CalculateAcceleration(airborneAcceleration) : CalculateAcceleration(airborneDeceleration);
        
        return Vector3.MoveTowards(currentVelocity, _targetVelocity, acceleration);
    }

    private float CalculateAcceleration(float acceleration)
    {
        return 1 / acceleration * _currentMaxSpeed * Time.deltaTime;
    }

    private bool IsAccelerating => _targetVelocity != Vector3.zero;
}
