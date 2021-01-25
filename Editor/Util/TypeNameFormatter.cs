namespace TypeReferences.Editor.Util
{
    using System;
    using System.Linq;
    using TypeReferences;
    using UnityEngine;

    /// <summary>
    /// Generates paths for dropdown items based on <see cref="System.Type.FullName"/> and formatted according to
    /// <see cref="Grouping"/>.
    /// </summary>
    internal static class TypeNameFormatter
    {
        /// <summary>Generates a path for a dropdown item according to <paramref name="grouping"/>.</summary>
        /// <param name="type">Type to generate the path for.</param>
        /// <param name="fullTypeName">Full name of the type.</param>
        /// <param name="grouping">Grouping mode to apply when formatting full type name.</param>
        /// <returns>
        /// Path where some dots in full type name are replaced with slashes. This allows to generate folders for types.
        /// </returns>
        public static string Format(Type type, string fullTypeName, Grouping grouping)
        {
            switch (grouping)
            {
                default:
                    return fullTypeName;

                case Grouping.ByNamespace:
                    return FormatByNamespace(fullTypeName);

                case Grouping.ByNamespaceFlat:
                    return FormatByNamespaceFlat(fullTypeName);

                case Grouping.ByAddComponentMenu:
                    return FormatByAddComponentMenu(type, fullTypeName);
            }
        }

        /// <summary>Gets the name of the type without its namespace.</summary>
        /// <param name="fullTypeName">Full name of the type including its namespace.</param>
        /// <returns>The type name without namespaces.</returns>
        public static string GetShortName(string fullTypeName)
        {
            int afterLastDot = fullTypeName.LastIndexOf('.') + 1;
            return fullTypeName.Substring(afterLastDot, fullTypeName.Length - afterLastDot);
        }

        private static string FormatByNamespace(string name)
        {
            return name.Replace('.', '/');
        }

        private static string FormatByNamespaceFlat(string name)
        {
            int lastDotIndex = name.LastIndexOf('.');

            return lastDotIndex == -1
                ? name
                : $"{name.Substring(0, lastDotIndex)}/{name.Substring(lastDotIndex + 1)}";
        }

        private static string FormatByAddComponentMenu(Type type, string name)
        {
            var addComponentMenuAttributes = type.GetCustomAttributes(typeof(AddComponentMenu), false);

            return addComponentMenuAttributes.Length == 1
                ? ((AddComponentMenu) addComponentMenuAttributes[0]).componentMenu
                : "Scripts/" + FormatByNamespace(name);
        }
    }
}