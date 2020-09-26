namespace SolidUtilities.Editor.Extensions
{
    using System;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    /// <summary>Different useful extensions for <see cref="PropertyDrawer"/>.</summary>
    public static class PropertyDrawerExtensions
    {
        /// <summary>
        /// Set attribute manually for a PropertyDrawer. This makes PropertyDrawer think a field it represents has a
        /// certain attribute.
        /// </summary>
        /// <param name="drawer">The property drawer to set an attribute for.</param>
        /// <param name="attribute">The attribute to set.</param>
        /// <example><code>
        /// var fieldDrawer = new TypeReferencePropertyDrawer();
        /// var typeOptionsAttribute = (TypeOptionsAttribute) Attribute.GetCustomAttribute(field, typeof(TypeOptionsAttribute));
        /// if (typeOptionsAttribute != null)
        ///     fieldDrawer.SetAttribute(typeOptionsAttribute);
        ///
        /// fieldDrawer.OnGUI(typeRefArea, _property, _label);
        /// </code></example>
        public static void SetAttribute(this PropertyDrawer drawer, PropertyAttribute attribute)
        {
            var attributeField = typeof(PropertyDrawer).GetField("m_Attribute", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? throw new NullReferenceException("m_Attribute field was not found in PropertyDrawer class.");

            attributeField.SetValue(drawer, attribute);
        }

        /// <summary>
        /// Set fieldInfo for a PropertyDrawer. Unity fills FieldInfo automatically for the PropertyDrawer instances
        /// it creates, but if you create a PropertyDrawer instance manually, you have to fill the FieldInfo yourself.
        /// </summary>
        /// <param name="drawer">The property drawer to set FieldInfo for.</param>
        /// <param name="fieldInfo">The FieldInfo to set.</param>
        /// <example><code>
        /// var fieldDrawer = new TypeReferencePropertyDrawer();
        /// fieldDrawer.SetFieldInfo(field);
        /// fieldDrawer.OnGUI(typeRefArea, _property, _label);
        /// </code></example>
        public static void SetFieldInfo(this PropertyDrawer drawer, FieldInfo fieldInfo)
        {
            var fieldInfoField = typeof(PropertyDrawer).GetField("m_FieldInfo", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? throw new NullReferenceException("m_FieldInfo field was not found in PropertyDrawer class.");

            fieldInfoField.SetValue(drawer, fieldInfo);
        }
    }
}