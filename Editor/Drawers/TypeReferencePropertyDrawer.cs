namespace TypeReferences.Editor.Drawers
{
    using TypeReferences;
    using UnityEditor;
    using UnityEngine;
    using Util;
    using TypeCache = Util.TypeCache;

    /// <summary>
    /// Custom property drawer for <see cref="TypeReference"/> properties.
    /// </summary>
    [CustomPropertyDrawer(typeof(TypeReference))]
    [CustomPropertyDrawer(typeof(TypeOptionsAttribute), true)]
    public class TypeReferencePropertyDrawer : PropertyDrawer
    {
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

            var fieldRectWithoutLabel = EditorGUI.PrefixLabel(position, label);
            return fieldRectWithoutLabel;
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

            var dropdownDrawer = new TypeDropdownDrawer(selectedType, typeOptionsAttribute, fieldInfo?.DeclaringType);

            var fieldDrawer = new TypeFieldDrawer(
                serializedTypeRef,
                position,
                dropdownDrawer,
                typeOptionsAttribute.ShortName,
                typeOptionsAttribute.UseBuiltInNames);

            fieldDrawer.Draw();
        }
    }
}
