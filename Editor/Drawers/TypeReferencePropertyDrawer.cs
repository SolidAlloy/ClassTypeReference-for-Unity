namespace TypeReferences.Editor.Drawers
{
    using System.Collections.Generic;
    using TypeReferences;
    using UnityEditor;
    using UnityEngine;
    using Util;
    using TypeCache = Util.TypeCache;

    /// <summary>
    /// Custom property drawer for <see cref="TypeReference"/> properties.
    /// </summary>
    /// <remarks>The class is public because it is used in the Samples package.</remarks>
    [CustomPropertyDrawer(typeof(TypeReference))]
    [CustomPropertyDrawer(typeof(TypeOptionsAttribute), true)]
    public class TypeReferencePropertyDrawer : PropertyDrawer
    {
        private static readonly Dictionary<(SerializedObject serializedObject, string path), string> _valuesCache = new Dictionary<(SerializedObject serializedObject, string path), string>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorStyles.popup.CalcHeight(GUIContent.none, 0f);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = GetPositionWithoutLabel(position, label);
            DrawTypeReferenceField(position, property);
        }

        private static Rect GetPositionWithoutLabel(Rect position, GUIContent label)
        {
            if (label == null || label == GUIContent.none)
                return position;

            return EditorGUI.PrefixLabel(position, label);
        }

        private void DrawTypeReferenceField(Rect position, SerializedProperty property)
        {
            var typeOptionsAttribute = attribute as TypeOptionsAttribute ?? new TypeOptionsAttribute();
            var serializedTypeRef = new SerializedTypeReference(property);

            var selectedType = TypeCache.GetType(serializedTypeRef.TypeNameAndAssembly);

            if (selectedType != null && ! typeOptionsAttribute.MatchesRequirements(selectedType))
            {
                Debug.Log($"{property.name} had the {selectedType} value but the type does not match " +
                          "constraints set in the attribute, so it was set to null.");
                selectedType = null;
                serializedTypeRef.TypeNameAndAssembly = string.Empty;
            }

            // This is needed for the 'value changed' event to propagate up, so that if someones subscribes to
            // value changed events through UI Elements, they will receive a notification.
            // We can determine that the value changed only by comparing it with the previous value because the dropdown
            // opens a new window and when the type is changed, the focus is on the window, not on this property drawer.
            if (!PreviousCurrentValuesEqual(serializedTypeRef.TypeNameProperty))
                GUI.changed = true;

            var dropdownDrawer = new TypeDropdownDrawer(selectedType, typeOptionsAttribute, fieldInfo?.DeclaringType);

            var fieldDrawer = new TypeFieldDrawer(
                serializedTypeRef,
                position,
                dropdownDrawer,
                typeOptionsAttribute.ShortName);

            fieldDrawer.Draw();
        }
        
        private static bool PreviousCurrentValuesEqual(SerializedProperty typeNameProperty)
        {
            var key = (typeNameProperty.serializedObject, typeNameProperty.propertyPath);
            
            if (_valuesCache.TryGetValue(key, out string previousValue))
            {
                if (typeNameProperty.stringValue == previousValue)
                {
                    return true;
                }

                _valuesCache[key] = typeNameProperty.stringValue;
                return false;
            }

            _valuesCache.Add(key, typeNameProperty.stringValue);
            return true;
        }
    }
}
