namespace TypeReferences.Editor
{
    using TypeReferences;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Custom property drawer for <see cref="TypeReference"/> properties.
    /// </summary>
    [CustomPropertyDrawer(typeof(TypeReference))]
    [CustomPropertyDrawer(typeof(TypeOptionsAttribute), true)]
    internal sealed class TypeReferencePropertyDrawer : PropertyDrawer
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
            var constraints = attribute as TypeOptionsAttribute;
            var serializedTypeRef = new SerializedTypeReference(property);

            var dropDown = new TypeDropDownDrawer(
                serializedTypeRef.TypeNameAndAssembly,
                constraints,
                fieldInfo?.DeclaringType);

            var fieldDrawer = new TypeFieldDrawer(serializedTypeRef, position, dropDown);

            fieldDrawer.Draw();
        }
    }
}
