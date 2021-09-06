using UnityEngine;

public class DragRotateInput : MonoBehaviour
{
    // ---- Inspector Variables ----
    
    [Header("Bindings")]
    [SerializeField] private Optional<RotateBase> targetRotationSystem;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 1;
    
    // ---- Non-Inspector Variables ----

    private Vector3 _rotationDelta;
    
    // ---- Methods ----
    
    private void Update()
    {
        if (targetRotationSystem.ShouldBeUsed)
            ApplyRotationInput(targetRotationSystem.Value);
    }

    private void ApplyRotationInput(RotateBase rotationSystem)
    {
        if (Input.GetMouseButton(0))
        {
            _rotationDelta.x = - Input.GetAxis("Mouse X");
            _rotationDelta.z = Input.GetAxis("Mouse Y");
            
            rotationSystem.ApplyRotationDelta(_rotationDelta * rotationSpeed);
        }
    }
}