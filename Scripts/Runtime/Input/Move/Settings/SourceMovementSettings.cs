using UnityEngine;

[CreateAssetMenu]
public class SourceMovementSettings : MovementSettings
{
    [Header("Settings")]
    [SerializeField] public int noFrictionFrameCount = 1;
    [SerializeField] public float friction = 5.5f;
    [SerializeField] public float airAcceleration = 40f;
    [SerializeField] public float groundAcceleration = 50f;
    [SerializeField] public float maxAirSpeed = 1f;

    private float _currentMaxSpeed;
    private Vector3 _currentVelocity;
    private Vector3 _currentTargetDirection;
    
    public override Vector3 CalculateVelocity(Vector3 currentVelocity, Vector3 currentTargetDirection, float currentMaxSpeed)
    {
        _currentMaxSpeed = currentMaxSpeed;
        _currentVelocity = currentVelocity;
        _currentTargetDirection = currentTargetDirection;
        
        return GroundCheck.IsGrounded
            ? MoveGround()
            : MoveAir();
    }
    
    private Vector3 MoveGround()
    {
        float speed = _currentVelocity.magnitude;
        
        if (speed != 0 && GroundCheck.FramesSpentOnGround > noFrictionFrameCount)
        {
            float drop = speed * friction * Time.deltaTime;
            _currentVelocity *= Mathf.Max(speed - drop, 0) / speed;
        }

        return Accelerate(groundAcceleration, _currentMaxSpeed);
    }

    private Vector3 MoveAir()
    {
        return Accelerate(airAcceleration, maxAirSpeed);
    }
    
     private Vector3 Accelerate(float acceleration, float maxVelocity)
    {
        float projVel = Vector3.Dot(_currentVelocity, _currentTargetDirection);
        float accelVel = acceleration * Time.deltaTime;

        if (projVel + accelVel > maxVelocity)
            accelVel = Mathf.Max(maxVelocity - projVel, 0);

        return _currentVelocity + _currentTargetDirection * accelVel;
    }
}
