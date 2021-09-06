using UnityEngine;

public class TransformRotate : RotateBase
{
    [Header("Dependencies")]
    [SerializeField] private Transform targetTransform;
    
    // ---- Abstract Implementation ----
    
    protected override void ApplyRotation()
    {
        targetTransform.localRotation = Quaternion.Euler(CurrentRotation);
    }
}