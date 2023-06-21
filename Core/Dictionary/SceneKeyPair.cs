using System;
using poetools.Core.Dictionary;
using TriInspector;
using UnityEngine;

namespace Listeners
{
    [Serializable]
    public class SceneKeyPair<TValue> : SerializedKeyValuePair<string, TValue>
    {
        [Scene]
        [SerializeField]
        private string key;
        
        [SerializeField]
        private TValue value;

        public override string Key => key;
        public override TValue Value => value;
    }
}