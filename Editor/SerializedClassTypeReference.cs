namespace TypeReferences.Editor
{
    using System;
    using UnityEditor;

    public class SerializedClassTypeReference
    {
        private readonly SerializedProperty _typeNameProperty;
        private readonly SerializedProperty _guidProperty;

        public SerializedClassTypeReference(SerializedProperty classTypeReferenceProperty)
        {
            _typeNameProperty = classTypeReferenceProperty.FindPropertyRelative(ClassTypeReference.NameOfTypeNameField);
            _guidProperty = classTypeReferenceProperty.FindPropertyRelative(ClassTypeReference.NameOfGuidField);
        }

        public string TypeNameAndAssembly
        {
            get => _typeNameProperty.stringValue;
            set
            {
                _typeNameProperty.stringValue = value;
                _guidProperty.stringValue = GetClassGuidFromTypeName(value);
            }
        }

        public bool TypeNameHasMultipleDifferentValues => _typeNameProperty.hasMultipleDifferentValues;

        private static string GetClassGuidFromTypeName(string typeName)
        {
            var type = Type.GetType(typeName);
            return ClassTypeReference.GetClassGuid(type);
        }
    }
}