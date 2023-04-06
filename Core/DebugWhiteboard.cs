using System;
using System.Collections.Generic;
using UnityEngine;

namespace poetools.Core
{
    public class DebugWhiteboard : LazySingleton<DebugWhiteboard>
    {
        public delegate void GUIData();

        public delegate string LabelData();

        private readonly struct WhiteboardDataDisposable : IDisposable
        {
            private readonly List<GUIData> _list;
            private readonly GUIData _data;

            public WhiteboardDataDisposable(List<GUIData> list, GUIData data)
            {
                _list = list;
                _data = data;
            }
            
            public void Dispose()
            {
                _list.Remove(_data);
            }
        }
        
        private List<GUIData> _guiData = new List<GUIData>();

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetActive(Debug.isDebugBuild);
        }

        public IDisposable Add(GUIData guiData)
        {
            _guiData.Add(guiData);
            return new WhiteboardDataDisposable(_guiData, guiData);
        }

        public IDisposable AddLabel(LabelData labelData)
        {
            void GUIData() => GUILayout.Label(labelData.Invoke());
            return Add(GUIData);
        }
        
        private void OnGUI()
        {
            foreach (var drawCall in _guiData)
                drawCall.Invoke();
        }
    }
}