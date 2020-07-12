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
    public sealed class ClassTypeReferencePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorStyles.popup.CalcHeight(GUIContent.none, 0);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var constraintAttribute = attribute as ClassTypeConstraintAttribute;

            if (label != null && label != GUIContent.none)
                position = EditorGUI.PrefixLabel(position, label);

            var classRefProperty = property.FindPropertyRelative("_classRef");
            var typeField = new TypeField(classRefProperty, position, constraintAttribute, fieldInfo.DeclaringType);
            typeField.Draw();
        }
    }
}
