using UnityEngine;

public abstract class JumpBase : MonoBehaviour
{
    [Header("Base Jump Settings")]
    [SerializeField] protected GroundCheck groundCheck;
    [SerializeField] protected JumpSettings settings;

    protected bool CoyoteAvailable;
    protected bool JumpQueued;
    protected bool HoldingJump = true;
    protected float TimeSinceJumpQueued;
    protected int RemainingAirJumps;
    protected Vector3 TargetVelocity;
    protected bool Rising => CurrentVelocity.y > 0;
    
    protected float StandardGravity => Rising ? settings.StandardGravityRising : settings.StandardGravityFalling;
    protected float FastFallGravity => Rising ? settings.FastFallGravityRising : settings.FastFallGravityFalling;
    
    protected Vector3 Gravity => settings.enableFastFall && HoldingJump == false 
        ? Vector3.down * FastFallGravity 
        : Vector3.down * StandardGravity;

    protected abstract Vector3 CurrentVelocity { get; }

    // ---- Event Subscription ----
    
    private void OnEnable()  { groundCheck.CollisionEvents.onEnterCollision.AddListener(OnLand); }
    private void OnDisable() { groundCheck.CollisionEvents.onEnterCollision.RemoveListener(OnLand); }
    
    private void OnLand(Collider col)
    {
        RemainingAirJumps = settings.airJumps;
        CoyoteAvailable = true;
    }

    // ---- Exposed Methods ----
    
    public void QueueJump()
    {
        JumpQueued = true;
        TimeSinceJumpQueued = 0;
    }
    
    public void SetHoldingJump(bool holdingJump)
    {
        HoldingJump = holdingJump;
    }
    
    protected abstract void ApplyVelocity(Vector3 velocity);

    // ---- Private Methods ----
    
    private void FixedUpdate()
    {
        TargetVelocity = CurrentVelocity;
        
        UpdateJumpBuffer();
        ApplyGravity();

        if (ShouldJump())
            ApplyJump();
        
        ApplyVelocity(TargetVelocity);
    }
    
    private void UpdateJumpBuffer()
    {
        if (JumpQueued)
        {
            if (TimeSinceJumpQueued > settings.jumpBufferTime)
                JumpQueued = false;

            TimeSinceJumpQueued += Time.fixedDeltaTime;
        }
    }
    
    // Checks for conditions like coyote jumps, air jumps, ect.
    private bool ShouldJump()
    {
        if (!JumpQueued)
            return false;
            
        if (groundCheck.IsGrounded)
            return true;

        if (CoyoteAvailable && groundCheck.TimeSpentFalling < settings.coyoteTime)
            return true;

        return RemainingAirJumps > 0;
    }

    private void ApplyJump()
    {
        CoyoteAvailable = false;
        JumpQueued = false;
        TargetVelocity.y = settings.JumpSpeed;
    }

    private void ApplyGravity()
    {
        if (groundCheck.IsGrounded)
            TargetVelocity.y = Mathf.Max(Gravity.y, CurrentVelocity.y);
        
        else TargetVelocity += Gravity * Time.fixedDeltaTime;
    }
}