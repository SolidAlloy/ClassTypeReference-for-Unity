// Copyright ClassTypeReference Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

namespace TypeReferences.Editor
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Custom property drawer for <see cref="ClassTypeReference"/> properties.
    /// </summary>
    [CustomPropertyDrawer(typeof(ClassTypeReference))]
    [CustomPropertyDrawer(typeof(ClassTypeConstraintAttribute), true)]
    internal sealed class ClassTypeReferencePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorStyles.popup.CalcHeight(GUIContent.none, 0);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = MovePositionToLabelIfPossible(position, label);
            DrawTypeReferenceField(position, property);
        }

        private static Rect MovePositionToLabelIfPossible(Rect position, GUIContent label)
        {
            if (label != null && label != GUIContent.none)
                position = EditorGUI.PrefixLabel(position, label);

            return position;
        }

        private void DrawTypeReferenceField(Rect position, SerializedProperty property)
        {
            var constraints = attribute as ClassTypeConstraintAttribute;
            var serializedTypeRef = new SerializedClassTypeReference(property);

            var dropDown = new TypeDropDownDrawer(
                serializedTypeRef.TypeNameAndAssembly,
                constraints,
                fieldInfo.DeclaringType);

            var fieldDrawer = new TypeFieldDrawer(serializedTypeRef, position, dropDown);

            fieldDrawer.Draw();
        }
    }
}
