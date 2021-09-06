using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // ---- Inspector Variables ----
    
    [Header("Bindings")]
    [SerializeField] private Optional<MovementBase> targetMovementSystem;
    [SerializeField] private Optional<RotateBase>[] targetRotationSystems;
    [SerializeField] private Optional<JumpBase> targetJumpingSystem;
    
    [Header("Input Settings")]
    [SerializeField] private bool scrollToJump;
    
    // ---- Non-Inspector Variables ----
    
    private Vector3 _targetDirection;
    private Vector3 _rotationDelta;
    
    // ---- Methods ----
    
    private void Update()
    {
        if (targetMovementSystem.ShouldBeUsed)
            ApplyMovementInput(targetMovementSystem.Value);
        
        if (targetJumpingSystem.ShouldBeUsed)
            ApplyJumpInput(targetJumpingSystem.Value);

        foreach (Optional<RotateBase> rotationSystem in targetRotationSystems)
        {
            if (rotationSystem.ShouldBeUsed)
                ApplyRotateInput(rotationSystem.Value);    
        }
    }

    private void ApplyRotateInput(RotateBase rotationSystem)
    {
        _rotationDelta.x = Input.GetAxis("Mouse X");
        _rotationDelta.y = - Input.GetAxis("Mouse Y");
        
        rotationSystem.ApplyRotationDelta(_rotationDelta);
    }

    private void ApplyMovementInput(MovementBase movementSystem)
    {
        _targetDirection.x = Input.GetAxisRaw("Horizontal");
        _targetDirection.y = Input.GetAxisRaw("Vertical");

        movementSystem.MoveTowards(_targetDirection);
    }

    private void ApplyJumpInput(JumpBase jumpingSystem)
    {
        if (Input.GetButtonDown("Jump") || scrollToJump && Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            jumpingSystem.QueueJump();

        jumpingSystem.SetHoldingJump(Input.GetButton("Jump"));
    }
}