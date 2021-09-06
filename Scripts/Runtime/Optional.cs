using System;
using System.Runtime.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Optional<T>
{
    [SerializeField] private T value;
    [SerializeField] private bool valueEnabled;

    public bool ShouldBeUsed => valueEnabled;
    public T Value => value;
}