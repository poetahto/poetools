using System;
using JetBrains.Annotations;
using UnityEngine;

[PublicAPI]
public class Checkpoint : MonoBehaviour
{
    public static event Action<Checkpoint> ActivatedCheckpoint;
        
    public static Checkpoint CurrentCheckpoint { get; private set; }
        
    public void ActivateCheckpoint()
    {
        CurrentCheckpoint = this;
        ActivatedCheckpoint?.Invoke(this);
    }
}