using System;
using TriInspector;
using UnityEngine;

namespace poetools.Core
{
    [Serializable]
    [DeclareHorizontalGroup("group")]
    public class InterfaceReference<T> where T : class
    {
        [Flags]
        public enum SearchType
        {
            Root = 1 << 0,
            Children = 1 << 1,
            Parent = 1 << 2,
        }

        [Group("group")]
        [ValidateInput(nameof(ValidateTarget))]
        [SerializeField]
        private GameObject target; // todo: require type of interface

        private TriValidationResult ValidateTarget()
        {
            if (target != null && target.TryGetComponent(out T _)) return TriValidationResult.Error($"Target does not contain {typeof(T).Name}");
            return TriValidationResult.Valid;
        }

        [Group("group")]
        [EnumToggleButtons]
        [SerializeField]
        private SearchType type;

        public T ResolveSingle()
        {
            T result = null;

            if (type.HasFlag(SearchType.Root))
                result = target.GetComponent<T>();

            if (result != null && type.HasFlag(SearchType.Children))
                result = target.GetComponentInChildren<T>();

            if (result != null && type.HasFlag(SearchType.Parent))
                result = target.GetComponentInParent<T>();

            return result;
        }

        public T[] ResolveMultiple()
        {
            T[] result = null;

            if (type.HasFlag(SearchType.Root))
                result = target.GetComponents<T>();

            if (result != null && type.HasFlag(SearchType.Children))
                result = target.GetComponentsInChildren<T>();

            if (result != null && type.HasFlag(SearchType.Parent))
                result = target.GetComponentsInParent<T>();

            return result;
        }
    }
}
