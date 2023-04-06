using System;
using System.Collections;
using System.Collections.Generic;
using poetools.Core.Tools;
using UnityEngine;

namespace poetools.Core
{
    /// <summary>
    /// Routes events throughout the codebase.
    /// Allows separate systems to communicate with each other.
    /// </summary>
    public class EventBus
    {
        private Dictionary<Type, SortedDataStore> _listenerDictionary = new Dictionary<Type, SortedDataStore>();

        // A collection of data, sorted by priority and counted.
        private class SortedDataStore
        {
            // Unregisters itself when disposed. Struct to avoid allocations.
            private struct UnregisterDisposable : IDisposable
            {
                public SortedDataStore Parent;
                public object Data;
                public int Priority;
                public string ListenerId;
            
                private bool _valid; // ensure that we can only be disposed once

                public void Dispose()
                {
                    if (!_valid)
                        return;

                    _valid = false;
                
                    Parent._data[Priority].Remove(Data);
                    Parent._dataIds.Remove(ListenerId);
                    Parent.Count--;
                    Parent.UpdateDebugString();
                }
            }
        
            /// <summary>
            /// How many listeners are registered in this group.
            /// </summary>
            public int Count { get; private set; }
        
            /// <summary>
            /// All of the Listeners in this group, sorted by priority.
            /// </summary>
            public IEnumerable Data => _data.Values;
        
            private SortedList<int, List<object>> _data = new SortedList<int, List<object>>();
            private List<string> _dataIds = new List<string>();
            private string _debugString;

            public override string ToString()
            {
                return _debugString;
            }
        
            private void UpdateDebugString()
            {
                _debugString = FormatTools.PrettyList(_dataIds);
            }

            /// <summary>
            /// Adds new data to this collection.
            /// </summary>
            /// <param name="data">The data to store.</param>
            /// <param name="priority">Order of storage for data: lower is early, higher is late</param>
            /// <param name="debugId">A human-readable name for the object calling this function.</param>
            /// <returns>A handle for this data: dispose it to remove from the collection.</returns>
            public IDisposable Add(object data, int priority, string debugId)
            {
                if (!_data.ContainsKey(priority))
                    _data.Add(priority, new List<object>());
            
                _dataIds.Add(debugId);
                _data[priority].Add(data);
                Count++;
            
                UpdateDebugString();
                return new UnregisterDisposable { Data = data, Priority = priority, ListenerId = debugId, Parent = this};
            }
        }
    
        // Color formatting information for debug messages.
        private IRichTextData _listenerFormat = Rtf.Composite(Rtf.Color(Color.red));
        private IRichTextData _eventFormat = Rtf.Composite(Rtf.Color(Color.cyan));

        public bool VerboseLogging { get; set; } = true;

        private void Log(string message)
        {
            if (VerboseLogging)
                Debug.Log(message);
        }
    
        /// <summary>
        /// Registers a new listener for an event of type T.
        /// </summary>
        /// <param name="listener">The action to take when the event occurs.</param>
        /// <param name="debugId">A human-readable name for the object calling this function.</param>
        /// <param name="priority">Order of execution for listeners: lower is early, higher is late.</param>
        /// <typeparam name="T">The type of event you are listening for.</typeparam>
        /// <returns>A handle for your listener: call dispose on it to stop listening for events.</returns>
        public IDisposable AddListener<T>(Action<T> listener, string debugId, int priority = 0)
        {
            Log($"{debugId.Format(_listenerFormat)} started listening to " +
                $"{typeof(T).Name.Format(_eventFormat)} with priority {priority.ToString().Format(_eventFormat)}.");
        
            Type type = typeof(T);

            if (!_listenerDictionary.ContainsKey(type))
                _listenerDictionary.Add(type, new SortedDataStore());
        
            return _listenerDictionary[type].Add(listener, priority, debugId);
        }

        /// <summary>
        /// Calls an event of type T.
        /// </summary>
        /// <param name="data">The data to pass along with the event.</param>
        /// <param name="debugId">A human-readable name for the object calling this function.</param>
        /// <typeparam name="T">The type of data to send with the event.</typeparam>
        public void Invoke<T>(T data, string debugId)
        {
            Type type = typeof(T);
        
            if (!_listenerDictionary.ContainsKey(type))
                _listenerDictionary.Add(type, new SortedDataStore());

            var listenerData = _listenerDictionary[type];
            int listenerCount = listenerData.Count;
            
            Log($"{debugId.Format(_listenerFormat)} invoked " +
                $"{typeof(T).Name.Format(_eventFormat)} ({listenerCount} {"Listener".Plural(listenerCount)}: {listenerData}).");
            
            if (listenerCount <= 0)
                return;

            foreach (List<object> listeners in listenerData.Data)
            {
                foreach (Action<T> listener in listeners)
                    listener.Invoke(data);
            }
        }
    }
}