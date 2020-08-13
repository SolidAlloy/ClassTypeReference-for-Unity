namespace TypeReferences.Editor
{
    using System;
    using UnityEngine;

    internal static class TypeNameFormatter
    {
        public static string Format(Type type, ClassGrouping grouping)
        {
            string name = type.FullName ?? string.Empty;

            switch (grouping)
            {
                default:
                    return name;

                case ClassGrouping.ByNamespace:
                    return FormatByNamespace(name);

                case ClassGrouping.ByNamespaceFlat:
                    return FormatByNamespaceFlat(name);

                case ClassGrouping.ByAddComponentMenu:
                    return FormatByAddComponentMenu(type, name);
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