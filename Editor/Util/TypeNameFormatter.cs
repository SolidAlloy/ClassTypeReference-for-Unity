namespace TypeReferences.Editor.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TypeReferences;
    using UnityEngine;

    /// <summary>
    /// Generates paths for dropdown items based on <see cref="System.Type.FullName"/> and formatted according to
    /// <see cref="Grouping"/>.
    /// </summary>
    internal static class TypeNameFormatter
    {
        private const string BuiltInTypesPrefix = "Built-in.";

        private static readonly Dictionary<string, string> BuiltInTypes = new Dictionary<string, string>
        {
            { "System.Boolean", "bool" },
            { "System.Byte", "byte" },
            { "System.SByte", "sbyte" },
            { "System.Char", "char" },
            { "System.Decimal", "decimal" },
            { "System.Double", "double" },
            { "System.Single", "float" },
            { "System.Int32", "int" },
            { "System.UInt32", "uint" },
            { "System.Int64", "long" },
            { "System.UInt64", "ulong" },
            { "System.Int16", "short" },
            { "System.UInt16", "ushort" },
            { "System.Object", "object" },
            { "System.String", "string" }
        };

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

        /// <summary>
        /// Replaces <paramref name="fullTypeName"/> with a built-in name if the built-in analogue exists.
        /// </summary>
        /// <param name="fullTypeName">Full name of the type.</param>
        /// <param name="withoutFolder">Whether to append a folder to the built-in type.</param>
        /// <returns>Name of the built-in type.</returns>
        /// <example><code>
        /// string intName = typeof(System.Int32).FullName;
        /// if (TryReplaceWithBuiltInName(intName))
        ///     Debug.Log(intName); // prints "Built-in.int"
        ///
        /// string intName = typeof(System.Int32).FullName;
        /// if (TryReplaceWithBuiltInName(intName), true)
        ///     Debug.Log(intName); // prints "int"
        /// </code></example>
        public static bool TryReplaceWithBuiltInName(ref string fullTypeName, bool withoutFolder = false)
        {
            if ( ! BuiltInTypes.TryGetValue(fullTypeName, out string builtInName))
                return false;

            fullTypeName = withoutFolder ? builtInName : BuiltInTypesPrefix + builtInName;
            return true;
        }

        /// <summary>Gets the name of the type without its namespace.</summary>
        /// <param name="fullTypeName">Full name of the type including its namespace.</param>
        /// <returns>The type name without namespaces.</returns>
        public static string GetShortName(string fullTypeName) => fullTypeName.Split('.').Last();

        private static string FormatByNamespace(string name)
        {
            return name.Replace('.', '/');
        }

        private static string FormatByNamespaceFlat(string name)
        {
            int lastPeriodIndex = name.LastIndexOf('.');
            if (lastPeriodIndex != -1)
                name = name.Substring(0, lastPeriodIndex) + "/" + name.Substring(lastPeriodIndex + 1);

            return name;
        }

        private static string FormatByAddComponentMenu(Type type, string name)
        {
            var addComponentMenuAttributes = type.GetCustomAttributes(typeof(AddComponentMenu), false);
            if (addComponentMenuAttributes.Length == 1)
                return ((AddComponentMenu) addComponentMenuAttributes[0]).componentMenu;

            return "Scripts/" + FormatByNamespace(name);
        }
    }
}