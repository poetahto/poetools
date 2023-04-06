using Core.Editor;
using poetools.Core;
using TriInspector;
using UnityEditor;
using UnityEngine;

[assembly: RegisterTriAttributeDrawer(typeof(ScriptableAssetReferenceDrawer), TriDrawerOrder.Inspector)]

namespace Core.Editor
{
    public class ScriptableAssetReferenceDrawer : TriAttributeDrawer<ScriptableAssetReferenceAttribute>
    {
        public override float GetHeight(float width, TriProperty property, TriElement next)
        {
            return next.GetHeight(width) + EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, TriProperty property, TriElement next)
        {
            Rect firstButton = new Rect(position);
            Rect secondButton = new Rect(position);
            Rect content = new Rect(position);

            firstButton.width *= 0.5f;
            firstButton.height = EditorGUIUtility.singleLineHeight;
            secondButton.width *= 0.5f;
            secondButton.height = EditorGUIUtility.singleLineHeight;
            secondButton.x += firstButton.width;
            content.y += EditorGUIUtility.singleLineHeight;

            var settingsAsset = property.Value as ScriptableObject;
            bool wasEnabled = GUI.enabled;

            if (wasEnabled)
                GUI.enabled = settingsAsset == null;

            if (GUI.Button(firstButton, "Create Inline"))
            {
                settingsAsset = ScriptableObject.CreateInstance(property.ValueType);
                property.SetValue(settingsAsset);
            }

            if (wasEnabled)
                GUI.enabled = settingsAsset != null && !AssetDatabase.Contains(settingsAsset);

            if (GUI.Button(secondButton, "Save"))
            {
                string fileName = AssetDatabase.GenerateUniqueAssetPath($"Assets/New {property.ValueType?.Name}.asset");
                AssetDatabase.CreateAsset(settingsAsset, fileName);
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = settingsAsset;
            }

            GUI.enabled = wasEnabled;
            next.OnGUI(content);
        }
    }
}
