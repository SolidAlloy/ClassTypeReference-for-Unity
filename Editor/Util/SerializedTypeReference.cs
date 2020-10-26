namespace TypeReferences.Editor.Util
{
    using System;
    using UnityEditor;

    /// <summary>
    /// A class that gives access to serialized properties inside <see cref="TypeReference"/>.
    /// </summary>
    internal class SerializedTypeReference
    {
        private readonly SerializedObject _parentObject;
        private readonly SerializedProperty _typeNameProperty;
        private readonly SerializedProperty _guidProperty;
        private readonly SerializedProperty _guidAssignmentFailedProperty;

        public SerializedTypeReference(SerializedProperty typeReferenceProperty)
        {
            _parentObject = typeReferenceProperty.serializedObject;
            _typeNameProperty = typeReferenceProperty.FindPropertyRelative(nameof(TypeReference.TypeNameAndAssembly));
            _guidProperty = typeReferenceProperty.FindPropertyRelative(nameof(TypeReference.GUID));
            _guidAssignmentFailedProperty = typeReferenceProperty.FindPropertyRelative(nameof(TypeReference.GuidAssignmentFailed));

            SetGuidIfAssignmentFailed();
        }

        public string TypeNameAndAssembly
        {
            get => _typeNameProperty.stringValue;
            set
            {
                _typeNameProperty.stringValue = value;
                _guidProperty.stringValue = GetClassGuidFromTypeName(value);
                _parentObject.ApplyModifiedProperties();
            }
        }

        public bool TypeNameHasMultipleDifferentValues => _typeNameProperty.hasMultipleDifferentValues;

        private bool GuidAssignmentFailed
        {
            get => _guidAssignmentFailedProperty.boolValue;
            set
            {
                _guidAssignmentFailedProperty.boolValue = value;
                _parentObject.ApplyModifiedProperties();
            }
        }

        private string GUID
        {
            set
            {
                _guidProperty.stringValue = value;
                _parentObject.ApplyModifiedProperties();
            }
        }

        private static string GetClassGuidFromTypeName(string typeName)
        {
            var type = Type.GetType(typeName);
            return TypeReference.GetClassGUID(type);
        }

        private void SetGuidIfAssignmentFailed()
        {
            if ( ! GuidAssignmentFailed || string.IsNullOrEmpty(TypeNameAndAssembly))
                return;

            GuidAssignmentFailed = false;
            GUID = GetClassGuidFromTypeName(TypeNameAndAssembly);
        }
    }
}