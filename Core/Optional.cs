using System;
using UnityEngine;

namespace poetools.Core
{
    [Serializable]
    public class Optional<T>
    {
        [SerializeField] private T value;
        [SerializeField] private bool valueEnabled;

        public Optional(T initialValue, bool enabled = false)
        {
            value = initialValue;
            valueEnabled = enabled;
        }

        public bool ShouldBeUsed
        {
            get => valueEnabled;
            set => valueEnabled = value;
        }

        public T Value
        {
            get => value;
            set => this.value = value;
        }
    }
}
