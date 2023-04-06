using System;
using System.Diagnostics;

namespace poetools.Core
{
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public class ScriptableAssetReferenceAttribute : Attribute
    {
    }
}