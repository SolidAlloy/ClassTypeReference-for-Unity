namespace TypeReferences.Editor.Util
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using UnityEditor;

    /// <summary>
    /// A class that gives access to serialized properties inside <see cref="TypeReference"/>.
    /// </summary>
    internal readonly struct SerializedTypeReference
    {
        public readonly SerializedProperty TypeNameProperty;
        private readonly SerializedObject _parentObject;
        private readonly SerializedProperty _guidProperty;
        private readonly SerializedProperty _guidAssignmentFailedProperty;

        public SerializedTypeReference(SerializedProperty typeReferenceProperty)
        {
            _parentObject = typeReferenceProperty.serializedObject;
            TypeNameProperty = typeReferenceProperty.FindPropertyRelative(nameof(TypeReference._typeNameAndAssembly));
            _guidProperty = typeReferenceProperty.FindPropertyRelative(nameof(TypeReference.GUID));
            _guidAssignmentFailedProperty = typeReferenceProperty.FindPropertyRelative(nameof(TypeReference.GuidAssignmentFailed));

            FindGuidIfAssignmentFailed();
        }

        public string TypeNameAndAssembly
        {
            get => TypeNameProperty.stringValue;
            set => SetTypeNameAndAssembly(value);
        }

        public bool TypeNameHasMultipleDifferentValues => TypeNameProperty.hasMultipleDifferentValues;

        private bool GuidAssignmentFailed
        {
            get => _guidAssignmentFailedProperty.boolValue;
            // Used in C# 8
            [UsedImplicitly] set => SetGUIDAssignmentFailed(value);
        }

        // Used in C# 8
        [UsedImplicitly] private string GUID { set => SetGUID(value); }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global",
            Justification = "The method is used by TypeFieldDrawer in C# 7")]
        public void SetTypeNameAndAssembly(string value)
        {
            TypeNameProperty.stringValue = value;
            _guidProperty.stringValue = GetClassGuidFromTypeName(value);
            _parentObject.ApplyModifiedProperties();
        }

        public void SetType(Type type)
        {
            TypeNameProperty.stringValue = TypeReference.GetTypeNameAndAssembly(type);
            _guidProperty.stringValue = TypeReference.GetClassGUID(type);
            _parentObject.ApplyModifiedProperties();
        }

        private void SetGUIDAssignmentFailed(bool value)
        {
            _guidAssignmentFailedProperty.boolValue = value;
            _parentObject.ApplyModifiedProperties();
        }

        private void SetGUID(string value)
        {
            _guidProperty.stringValue = value;
            _parentObject.ApplyModifiedProperties();
        }

        private static string GetClassGuidFromTypeName(string typeName)
        {
            var type = Type.GetType(typeName);
            return TypeReference.GetClassGUID(type);
        }

        private void FindGuidIfAssignmentFailed()
        {
            if ( ! GuidAssignmentFailed || string.IsNullOrEmpty(TypeNameAndAssembly))
                return;

            // C# 7 is dumb and doesn't know that we don't change member variables in the property setter

#if UNITY_2020_2_OR_NEWER
            GuidAssignmentFailed = false;
            GUID = GetClassGuidFromTypeName(TypeNameAndAssembly);
#else
            SetGUIDAssignmentFailed(false);
            SetGUID(GetClassGuidFromTypeName(TypeNameAndAssembly));
#endif
        }
    }
}