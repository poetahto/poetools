using System;
using TriInspector;
using UnityEngine;

namespace poetools.Core.Dictionary
{
    /// <summary>
    /// A pair where a key maps to a Unity scene. The value will be the string scene name,
    /// and is decorated with a custom TriInspector property drawer.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    [Serializable]
    public class SceneValuePair<TKey> : SerializedKeyValuePair<TKey, string>
    {
        [SerializeField]
        private TKey key;

        [Scene]
        [SerializeField]
        private string scene;

        /// <inheritdoc/>
        public override TKey Key => key;

        /// <inheritdoc/>
        public override string Value => scene;
    }
}
