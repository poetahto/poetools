using UnityEngine;

public abstract class RotateBase : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] public Vector3 rotationSpeed = Vector3.one;
    [SerializeField] private Vector3 rotationConstraints = new Vector3(-1f, -1f, -1f);
    
    private Vector3 _currentRotation;
    public Vector3 CurrentRotation => _currentRotation;

    private void Update()
    {
        ClampCurrentRotation();
        ApplyRotation();
    }

    public void ApplyRotationDelta(Vector3 rotationDelta)
    {
        _currentRotation.x += rotationDelta.y * rotationSpeed.x;
        _currentRotation.y += rotationDelta.x * rotationSpeed.y;
        _currentRotation.z += rotationDelta.z * rotationSpeed.z;
    }
    
    private void ClampCurrentRotation()
    {
        if (rotationConstraints.x > 0)
            _currentRotation.x = Mathf.Clamp(_currentRotation.x, -rotationConstraints.x, rotationConstraints.x);
    
        if (rotationConstraints.y > 0)
            _currentRotation.y = Mathf.Clamp(_currentRotation.y, -rotationConstraints.y, rotationConstraints.y);
        
        if (rotationConstraints.z > 0)
            _currentRotation.z = Mathf.Clamp(_currentRotation.z, -rotationConstraints.z, rotationConstraints.z);
    }

    protected abstract void ApplyRotation();
}
