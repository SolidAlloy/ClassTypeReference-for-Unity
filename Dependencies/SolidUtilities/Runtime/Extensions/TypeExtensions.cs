namespace SolidUtilities.Extensions
{
    using System;
    using System.Reflection;

    /// <summary>Different useful extensions for <see cref="System.Type"/>.</summary>
    public static class TypeExtensions
    {
        /// <summary>Finds a field recursively in fields of a class.</summary>
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
    }
}