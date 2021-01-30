namespace TypeReferences.Demo.Editor
{
    using System.Reflection;
    using SolidUtilities.UnityEditorInternals;
    using TypeReferences.Editor.Drawers;
    using UnityEditor;
    using UnityEngine;

    internal class TypeReferenceField
    {
        private readonly TypeReferencePropertyDrawer _fieldDrawer;
        private readonly GUIContent _label;
        private readonly SerializedProperty _property;

        public TypeReferenceField(SerializedProperty property, FieldInfo field)
        {
            _fieldDrawer = new TypeReferencePropertyDrawer();
            _label = new GUIContent(property.displayName);
            _property = property;

            _fieldDrawer.SetFieldInfo(field);

            SetInheritsAttributeIfFound(field);
            SetTypeOptionsAttributeIfFound(field);
        }

        public void Draw()
        {
            Rect typeRefArea = EditorGUILayout.GetControlRect(true, _fieldDrawer.GetPropertyHeight(_property, _label));
            _fieldDrawer.OnGUI(typeRefArea, _property, _label);
        }

        private void SetInheritsAttributeIfFound(MemberInfo field)
        {
            var inheritsAttribute = field.GetCustomAttribute<InheritsAttribute>();
            if (inheritsAttribute != null)
                _fieldDrawer.SetAttribute(inheritsAttribute);
        }

        private void SetTypeOptionsAttributeIfFound(MemberInfo field)
        {
            var typeOptionsAttribute = field.GetCustomAttribute<TypeOptionsAttribute>();
            if (typeOptionsAttribute != null)
                _fieldDrawer.SetAttribute(typeOptionsAttribute);
        }
    }
}