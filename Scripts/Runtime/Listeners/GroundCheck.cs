﻿using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SurfaceCollisionEvents
{
    [Tooltip("Passes the collider which was collided with.")]
    public UnityEvent<Collider> onEnterCollision = new UnityEvent<Collider>();
        
    [Tooltip("Passes the collider which was exited.")]
    public UnityEvent<Collider> onExitCollision = new UnityEvent<Collider>();
}

public abstract class GroundCheck : MonoBehaviour
{
    [Header("Ground Check Settings")]
    [SerializeField] protected Vector3 gravityDirection = Vector3.down;
    [SerializeField] protected float slopeLimitDegrees = 45f;
    [SerializeField] private bool showDebug;   

    [Space]
    [SerializeField] protected SurfaceCollisionEvents collisionEvents = new SurfaceCollisionEvents();

    public bool WasGroundedLastFrame { get; private set; }
    public int FramesSpentOnGround { get; private set; }
    public float TimeSpentFalling { get; private set; }
    public abstract bool IsGrounded { get; }
    public abstract Vector3 ContactNormal { get; }
    public abstract Collider ConnectedCollider { get; }
    
    public SurfaceCollisionEvents CollisionEvents => collisionEvents;
    public bool JustEntered => IsGrounded && !WasGroundedLastFrame;
    public bool JustExited => !IsGrounded && WasGroundedLastFrame;
    
    protected virtual void Update()
    {
        UpdateGroundedFrames();
        UpdateFallingDuration();
        
        CheckForCollisionEnter();
        CheckForCollisionExit();
    }

    private void LateUpdate()
    {
        WasGroundedLastFrame = IsGrounded;
    }

    private void UpdateGroundedFrames()
    {
        if (IsGrounded)
            FramesSpentOnGround = JustEntered ? 0 : FramesSpentOnGround + 1;
    }
    
    private void UpdateFallingDuration()
    {
        if (JustEntered)
            TimeSpentFalling = 0;

        if (IsGrounded == false)
            TimeSpentFalling += Time.deltaTime;
    }
    
    private void CheckForCollisionEnter()
    {
        if (JustEntered)
            collisionEvents.onEnterCollision.Invoke(ConnectedCollider);
    }

    private void CheckForCollisionExit()
    {
        if (JustExited)
            collisionEvents.onExitCollision.Invoke(ConnectedCollider);
    }
    
    private void OnGUI()
    {
        if (showDebug)
        {
            string connectedCollider = ConnectedCollider ? ConnectedCollider.name : "None";
                
            GUILayout.Label($"IsGrounded: {IsGrounded}");
            GUILayout.Label($"Was Grounded Last Frame: {WasGroundedLastFrame}");
            GUILayout.Label($"Connected Collider: {connectedCollider}");
            GUILayout.Label($"Contact Normal: {ContactNormal}");
            GUILayout.Label($"Frames spent on ground: {FramesSpentOnGround}");
            GUILayout.Label($"Time spent falling: {TimeSpentFalling}");
        }
    }
}