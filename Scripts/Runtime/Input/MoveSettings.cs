using UnityEngine;

namespace Input
{
    [CreateAssetMenu]
    public class MoveSettings : ScriptableObject
    {
        // ---- Inspector Variables ----
        
        [Header("Movement Settings")]
        [SerializeField] public float maxMovementSpeed = 5f;
        [SerializeField] public float groundedRampTime = 0.125f;
        [SerializeField] public float airborneRampTime = 0.3f;
        
        [Header("Sprinting")]
        [SerializeField] public bool useSprinting;
        [SerializeField] public float maxSprintingSpeed = 6f;
    
        [Header("Source Movement")] 
        [SerializeField] public bool useSourceMovement;
        [SerializeField] public float maxGroundSpeed = 5f;
        [SerializeField] public float maxAirSpeed  = 1f;
        [SerializeField] public float groundAcceleration = 50f;
        [SerializeField] public float airAcceleration = 40f;
        [SerializeField] public int noFrictionFrameCount = 1;
        [SerializeField] public float friction = 5.5f;
        
        // ---- Properties ----
        
        public float StandardGroundedAcceleration => 
            groundedRampTime > 0 ? 1 / groundedRampTime * maxMovementSpeed * Time.deltaTime : maxMovementSpeed;

        public float StandardAirborneAcceleration => 
            airborneRampTime > 0 ? 1 / airborneRampTime * maxMovementSpeed * Time.deltaTime : maxMovementSpeed;
    }
}