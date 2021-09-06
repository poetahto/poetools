using System;
using UnityEngine;

[Serializable]
public class ModifiableFloat
{
    [SerializeField] private float baseValue;

    public float CurrentValue => (baseValue + _currentOffset) * _currentMultiplier;

    private float _currentMultiplier = 1;
    private float _currentOffset;
    
    public void AddMultiplier(float multiplier) { _currentMultiplier += multiplier - 1; }
    public void RemoveMultiplier(float multiplier) { _currentMultiplier -= multiplier - 1; }

    public void AddOffset(float offset) { _currentOffset += offset; }
    public void RemoveOffset(float offset) { _currentOffset -= offset; }
}

[Serializable]
public class ModifiableInt
{
    [SerializeField] private int baseValue;

    public int CurrentValue => (baseValue + _currentOffset) * _currentMultiplier;

    private int _currentMultiplier = 1;
    private int _currentOffset;

    public void AddMultiplier(float multiplier) { _currentMultiplier += Mathf.RoundToInt(multiplier) - 1; }
    public void RemoveMultiplier(float multiplier) { _currentMultiplier -= Mathf.RoundToInt(multiplier) - 1; }

    public void AddOffset(float offset) { _currentOffset += Mathf.RoundToInt(offset); }
    public void RemoveOffset(float offset) { _currentOffset -= Mathf.RoundToInt(offset); }
}