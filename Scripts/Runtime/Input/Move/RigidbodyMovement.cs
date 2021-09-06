using UnityEngine;

public class RigidbodyMovement : MovementBase
{
    [Header("Dependencies")] 
    [SerializeField] private Rigidbody targetRigidbody;

    protected override Vector3 CurrentVelocity => targetRigidbody.velocity;

    protected override void ApplyVelocity(Vector3 velocity)
    {
        targetRigidbody.velocity = velocity;
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        if (showDebug)
        {
            if (GUILayout.Button("Boost up"))
                targetRigidbody.AddForce(Vector3.up * 20, ForceMode.Impulse);
        
            if (GUILayout.Button("Boost forward"))
                targetRigidbody.AddForce(transform.forward * 20, ForceMode.Impulse);    
        }
    }
}