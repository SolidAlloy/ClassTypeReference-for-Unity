namespace TypeReferences.Editor.Drawers
{
    using Editor.Util;
    using TypeReferences;
    using UnityEditor;
    using UnityEngine;
    using TypeCache = Editor.Util.TypeCache;

    /// <summary>
    /// Custom property drawer for <see cref="TypeReference"/> properties.
    /// </summary>
    [CustomPropertyDrawer(typeof(TypeReference))]
    [CustomPropertyDrawer(typeof(TypeOptionsAttribute), true)]
    public sealed class TypeReferencePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorStyles.popup.CalcHeight(GUIContent.none, 0);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = ExcludeLabelFromPositionIfNecessary(position, label);
            DrawTypeReferenceField(position, property);
        }

        private static Rect ExcludeLabelFromPositionIfNecessary(Rect position, GUIContent label)
        {
            if (label == null || label == GUIContent.none)
                return position;

            var positionExcludingLabel = EditorGUI.PrefixLabel(position, label);
            return positionExcludingLabel;
        }

        private void DrawTypeReferenceField(Rect position, SerializedProperty property)
        {
            var typeOptionsAttribute = attribute as TypeOptionsAttribute ?? new TypeOptionsAttribute();
            var serializedTypeRef = new SerializedTypeReference(property);

            var selectedType = TypeCache.GetType(serializedTypeRef.TypeNameAndAssembly);

            if (selectedType != null && !typeOptionsAttribute.MatchesRequirements(selectedType))
            {
                Debug.Log($"{property.name} had the {selectedType} value but the type does not match " +
                          $"constraints set in the attribute, so it was set to null.");
                selectedType = null;
                serializedTypeRef.TypeNameAndAssembly = string.Empty;
            }

            var dropDown = new TypeDropdownDrawer(
                selectedType,
                typeOptionsAttribute,
                fieldInfo?.DeclaringType);

            var fieldDrawer = new TypeFieldDrawer(serializedTypeRef, position, dropDown);

            fieldDrawer.Draw();
        }
    }
}
