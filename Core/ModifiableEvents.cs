using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace poetools.Core
{
    public interface IModifiableEvent
    {
        /// <summary>
        /// All member variables must be reset in this method.
        /// Otherwise, old data may be carried over when an instance of this is reused.
        /// </summary>
        void ResetValues();
    }

    public static class ModifiableEventFactory 
    {
        private static Dictionary<Type, ObjectPool<object>> _eventPool;

        public static T Get<T>() where T : IModifiableEvent, new()
        {
            Type type = typeof(T);
        
            // lookup or create new entry for the eventPool
            if (_eventPool.ContainsKey(type) == false)
            {
                _eventPool.Add(type, new ObjectPool<object>(() => new T()));
            }

            object eventData = _eventPool[type].Get();
            return UnsafeUtility.As<object, T>(ref eventData);
        }

        public static void Return<T>(T value) where T : IModifiableEvent, new()
        {
            // mark value as free in the event pool
            Type type = typeof(T);
            _eventPool[type].Release(value);
        
            // reset it's values
            value.ResetValues();
        }
    }
}