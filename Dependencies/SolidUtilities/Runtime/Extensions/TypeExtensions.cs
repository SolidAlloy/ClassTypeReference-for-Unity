namespace SolidUtilities.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;

    /// <summary>Different useful extensions for <see cref="System.Type"/>.</summary>
    public static class TypeExtensions
    {
        /// <summary>Finds a field recursively in the fields of a class.</summary>
        /// <param name="parentType">The class type to start the search from.</param>
        /// <param name="path">The path to a field, separated by dot.</param>
        /// <returns>Field info if the field is found, and null if not.</returns>
        /// <example><code>
        /// FieldInfo nestedField = targetType.GetFieldAtPath("parentField.nestedField");
        /// Debug.Log((string)nestedField.GetValue(obj));
        /// </code></example>
        public static FieldInfo GetFieldAtPath(this Type parentType, string path)
        {
            FieldInfo field = null;
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            foreach (string part in path.Split('.'))
            {
                field = parentType.GetField(part, flags);
                if (field == null)
                    return null;

                parentType = field.FieldType;
            }

            return field;
        }

        /// <summary>
        /// Collects all the serializable fields of a class: private ones with SerializeField attribute and public ones.
        /// </summary>
        /// <param name="type">Class type to collect the fields from.</param>
        /// <returns>Collection of the serializable fields of a class.</returns>
        /// <example><code>
        /// var fields = objectType.GetSerializedFields();
        /// foreach (var field in fields)
        /// {
        ///     string fieldLabel = ObjectNames.NicifyVariableName(field.Name);
        ///     object fieldValue = field.GetValue(serializedObject);
        ///     object newValue = DrawField(fieldLabel, fieldValue);
        ///     field.SetValue(serializedObject, newValue);
        /// }
        /// </code></example>
        public static IEnumerable<FieldInfo> GetSerializedFields(this Type type)
        {
            const BindingFlags instanceFilter = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var instanceFields = type.GetFields(instanceFilter);
            return instanceFields.Where(field => field.IsPublic || field.GetCustomAttribute<SerializeField>() != null);
        }

        /// <summary>Checks whether the type is nullable.</summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is nullable.</returns>
        public static bool IsNullable(this Type type)
        {
            return ! type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// Checks whether the type is derivative of a generic class without specifying its type parameter.
        /// </summary>
        /// <param name="typeToCheck">The type to check.</param>
        /// <param name="generic">The generic class without type parameter.</param>
        /// <returns>True if the type is subclass of the generic class.</returns>
        /// <example><code>
        /// class Base&lt;T> { }
        /// class IntDerivative : Base&lt;int> { }
        /// class StringDerivative : Base&lt;string> { }
        ///
        /// bool intIsSubclass = typeof(IntDerivative).IsSubclassOfRawGeneric(typeof(Base&lt;>)); // true
        /// bool stringIsSubclass = typeof(StringDerivative).IsSubclassOfRawGeneric(typeof(Base&lt;>)); // true
        /// </code></example>
        public static bool IsSubclassOfRawGeneric(this Type typeToCheck, Type generic)
        {
            while (typeToCheck != null && typeToCheck != typeof(object))
            {
                Type cur = typeToCheck.IsGenericType ? typeToCheck.GetGenericTypeDefinition() : typeToCheck;

                if (generic == cur)
                    return true;

                typeToCheck = typeToCheck.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Checks whether the type inherits from the base type.
        /// </summary>
        /// <param name="typeToCheck">The type to check.</param>
        /// <param name="baseType">
        /// The base type to check inheritance from. It can be a generic type without the type parameter.
        /// </param>
        /// <returns>Whether <paramref name="typeToCheck"/>> inherits <paramref name="baseType"/>.</returns>
        /// <example><code>
        /// class Base&lt;T> { }
        /// class IntDerivative : Base&lt;int> { }
        ///
        /// bool isAssignableWithTypeParam = typeof(typeof(Base&lt;int>).IsAssignableFrom(IntDerivative)); // true
        /// bool isAssignableWithoutTypeParam = typeof(typeof(Base&lt;>)).IsAssignableFrom(IntDerivative); // false
        /// bool inherits = typeof(IntDerivative).Inherits(typeof(Base&lt;>)); // true
        /// </code></example>
        public static bool InheritsFrom(this Type typeToCheck, Type baseType)
        {
            bool subClassOfRawGeneric = false;
            if (baseType.IsGenericType)
                subClassOfRawGeneric = typeToCheck.IsSubclassOfRawGeneric(baseType);

            return baseType.IsAssignableFrom(typeToCheck) || subClassOfRawGeneric;
        }
    }
}