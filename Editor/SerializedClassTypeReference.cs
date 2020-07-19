namespace TypeReferences.Editor
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class SerializedClassTypeReference
    {
        private readonly SerializedProperty _typeNameProperty;
        private readonly SerializedProperty _guidProperty;
        private readonly SerializedProperty _assetPathProperty;

        public SerializedClassTypeReference(SerializedProperty classTypeReferenceProperty)
        {
            _typeNameProperty = classTypeReferenceProperty.FindPropertyRelative(ClassTypeReference.NameOfTypeNameField);
            _guidProperty = classTypeReferenceProperty.FindPropertyRelative(ClassTypeReference.NameOfGuidField);
            _assetPathProperty = classTypeReferenceProperty.FindPropertyRelative(ClassTypeReference.NameOfAssetPath);
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
            return ClassTypeReference.GetClassGUID(type);
        }

        public void TryUpdateTypeUsingGUID()
        {
            if (_guidProperty.stringValue == string.Empty)
                return;

            string assetPath = AssetDatabase.GUIDToAssetPath(_guidProperty.stringValue);
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
            if (script == null)
                return;

            Type type = script.GetClass();
            _typeNameProperty.stringValue = ClassTypeReference.GetTypeNameAndAssembly(type);
        }
    }
}