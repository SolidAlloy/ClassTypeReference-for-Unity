namespace TypeReferences.Editor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// A class that holds static values related to ClassTypeReference and its custom inspector.
    /// </summary>
    internal static class CachedTypeReference
    {
        public const string ReferenceUpdatedCommandName = "TypeReferenceUpdated";
        public static readonly int ControlHint = typeof(ClassTypeReferencePropertyDrawer).GetHashCode();
        public static readonly GUIContent FieldContent = new GUIContent();
        public static readonly GenericMenu.MenuFunction2 SelectedTypeName = OnSelectedTypeName;
        public static int SelectionControlID;
        public static string SelectedTypeNameAndAssembly;

        /// <summary>
        /// Improves performance by avoiding a large number of <see cref="M:Type.GetType"/> calls.
        /// </summary>
        private static readonly Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();

        /// <summary>
        /// Get type from TypeCache if it is cached.
        /// Otherwise, find the type, cache it, and return it to the caller.
        /// </summary>
        /// <param name="typeName">Type name, followed by a comma and assembly name.</param>
        /// <returns>Cached class type.</returns>
        public static Type GetType(string typeName)
        {
            if (TypeCache.TryGetValue(typeName, out Type type))
                return type;

            type = ! string.IsNullOrEmpty(typeName) ? Type.GetType(typeName) : null;
            TypeCache[typeName] = type;
            return type;
        }

        private static void OnSelectedTypeName(object userData)
        {
            var selectedType = userData as Type;

            SelectedTypeNameAndAssembly = ClassTypeReference.GetTypeNameAndAssembly(selectedType);
            Event typeReferenceUpdated = EditorGUIUtility.CommandEvent(ReferenceUpdatedCommandName);
            EditorWindow.focusedWindow.SendEvent(typeReferenceUpdated);
        }
    }
}