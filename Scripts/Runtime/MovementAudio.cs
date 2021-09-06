using JetBrains.Annotations;
using Listeners;
using UnityEngine;

public class MovementAudio : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Listeners.GroundedSphereCaster groundedSphereCaster;
        
    [Header("Settings")]
    [SerializeField] private float stepDistance = 2f;
    [SerializeField] private SurfaceSettings defaultSurfaceSettings;
        
    private Surface _surface;
    private float _displacement;
    private Vector3 _previousPosition;

    private void Awake()
    {
        _displacement = stepDistance;
    }

    private void Update()
    {
        if (groundedSphereCaster.IsGrounded)
            CheckForStep();
            
        _previousPosition = transform.position;
    }

    private void CheckForStep()
    {
        _displacement += Vector3.Distance(transform.position, _previousPosition);
            
        if (_displacement > stepDistance)
            PlayStepSound();
    }

    private void PlayStepSound()
    {
        _displacement = 0;
            
        audioSource.PlayOneShot(groundedSphereCaster.ConnectedCollider.TryGetComponent(out _surface)
            ? _surface.Settings.StepSound
            : defaultSurfaceSettings.StepSound);
    }

    [PublicAPI]
    public void PlayJumpSound(Collider col)
    {
        audioSource.PlayOneShot(col.TryGetComponent(out _surface)
            ? _surface.Settings.JumpSound
            : defaultSurfaceSettings.JumpSound);
    }

    [PublicAPI]
    public void PlayLandSound(Collider col)
    {
        audioSource.PlayOneShot(col.TryGetComponent(out _surface)
            ? _surface.Settings.LandSound
            : defaultSurfaceSettings.LandSound);
    }
}