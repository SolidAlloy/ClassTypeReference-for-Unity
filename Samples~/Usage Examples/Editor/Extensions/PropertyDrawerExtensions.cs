namespace TypeReferences.Demo.Editor
{
    using System;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    internal static class PropertyDrawerExtensions
    {
        public static void SetAttribute(this PropertyDrawer drawer, PropertyAttribute attribute)
        {
            var attributeField = typeof(PropertyDrawer).GetField("m_Attribute", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? throw new NullReferenceException("m_Attribute field was not found in PropertyDrawer class.");

            attributeField.SetValue(drawer, attribute);
        }

        public static void SetFieldInfo(this PropertyDrawer drawer, FieldInfo fieldInfo)
        {
            var fieldInfoField = typeof(PropertyDrawer).GetField("m_FieldInfo", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? throw new NullReferenceException("m_FieldInfo field was not found in PropertyDrawer class.");

            fieldInfoField.SetValue(drawer, fieldInfo);
        }
    }
}