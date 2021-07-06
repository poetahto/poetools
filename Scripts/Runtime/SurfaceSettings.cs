using UnityEngine;

[CreateAssetMenu]
public class SurfaceSettings : ScriptableObject
{
    [Header("Movement Settings")]
    [SerializeField] private float friction = 4.5f;
        
    [Header("Audio Settings")]
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;
        
    // ---- Properties ----

    public float Friction => friction;
    public AudioClip StepSound => stepSound;
    public AudioClip JumpSound => jumpSound;
    public AudioClip LandSound => landSound;
}