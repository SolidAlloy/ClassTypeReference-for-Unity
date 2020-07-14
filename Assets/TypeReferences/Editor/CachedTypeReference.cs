namespace TypeReferences.Editor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    internal static class CachedTypeReference
    {
        public const string ReferenceUpdatedCommandName = "TypeReferenceUpdated";
        public static readonly int ControlHint = typeof(ClassTypeReferencePropertyDrawer).GetHashCode();
        public static readonly GUIContent FieldContent = new GUIContent();
        public static readonly GenericMenu.MenuFunction2 SelectedTypeName = OnSelectedTypeName;
        public static int SelectionControlID;
        public static string SelectedClassRef;

        /// <summary>
        /// Improves performance by avoiding extensive number of <see cref="M:Type.GetType"/> calls.
        /// </summary>
        private static readonly Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();

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

            SelectedClassRef = ClassTypeReference.GetClassRef(selectedType);
            var typeReferenceUpdatedEvent = EditorGUIUtility.CommandEvent(ReferenceUpdatedCommandName);
            EditorWindow.focusedWindow.SendEvent(typeReferenceUpdatedEvent);
        }
    }
}