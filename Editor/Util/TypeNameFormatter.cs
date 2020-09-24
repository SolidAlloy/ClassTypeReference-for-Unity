namespace TypeReferences.Editor.Util
{
    using System;
    using TypeReferences;
    using UnityEngine;

    internal static class TypeNameFormatter
    {
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