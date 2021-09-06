using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(Optional<>))]
    public class OptionalPropertyDrawer : PropertyDrawer
    {
        private const float ValueEnabledWidth = 20f;
        private const float ValueEnabledPadding = 10f;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("value");
            var valueEnabledProperty = property.FindPropertyRelative("valueEnabled");
            
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginDisabledGroup(valueEnabledProperty.boolValue == false);
            
            // Draw Prefix
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            // Calculate Rects
            const float totalValueEnabledWidth = ValueEnabledWidth + ValueEnabledPadding;

            Rect valueRect = position;
            valueRect.width -= totalValueEnabledWidth;

            Rect valueEnabledRect = position;
            valueEnabledRect.x += position.width - ValueEnabledWidth;
            valueEnabledRect.width = ValueEnabledWidth;
            
            // Draw Fields
            EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
            EditorGUI.EndDisabledGroup();
            EditorGUI.PropertyField(valueEnabledRect, valueEnabledProperty, GUIContent.none);
            
            EditorGUI.EndProperty();
        }
    }
}