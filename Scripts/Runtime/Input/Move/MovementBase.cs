using UnityEngine;

public abstract class MovementBase : MonoBehaviour
{
    [Header("Base Movement Settings")]
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private MovementSettings movement;
    [SerializeField] protected bool showDebug;
    
    private Transform _transform;
    private Vector3 _directionalInput;

    public float CurrentMaxSpeed { get; set; }
    public Vector3 MoveDirectionMultiplier { get; set; } = Vector3.one;
    
    protected abstract Vector3 CurrentVelocity { get; }

    public void MoveTowards(Vector3 direction)
    {
        _directionalInput = direction;
    }
    
    private void Awake()
    {
        _transform = transform;
        movement.Initialize(groundCheck);
        CurrentMaxSpeed = movement.BaseMovementSpeed;
    }

    protected virtual void Update()
    {
        Vector3 targetVelocity = movement.CalculateVelocity(CurrentVelocity, GetMovementDirection(), CurrentMaxSpeed);
        ApplyVelocity(targetVelocity);
    }

    protected abstract void ApplyVelocity(Vector3 velocity);

    private Vector3 GetMovementDirection()
    {
        Vector3 rightVelocity = _transform.right * _directionalInput.x;
        Vector3 forwardVelocity = _transform.forward * _directionalInput.y;
        Vector3 upwardVelocity = _transform.up * _directionalInput.z;
        Vector3 totalVelocity = Vector3.ClampMagnitude(forwardVelocity + rightVelocity + upwardVelocity, 1);

        return Vector3.Scale(totalVelocity, MoveDirectionMultiplier);
    }

    protected virtual void OnGUI()
    {
        if (showDebug)
        {
            GUILayout.Label($"Current Velocity: {CurrentVelocity}");
            GUILayout.Label($"Directional Input: {_directionalInput}");
            GUILayout.Label($"Current Max Speed: {CurrentMaxSpeed}");
            GUILayout.Label($"Move Direction Multiplier: {MoveDirectionMultiplier}");
        }
    }
}