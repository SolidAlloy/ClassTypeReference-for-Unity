namespace TypeReferences.Editor
{
    using System;
    using Deprecated;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// A class that gives access to serialized properties inside <see cref="TypeReference"/>.
    /// </summary>
    internal class SerializedTypeReference
    {
        private readonly SerializedProperty _typeNameProperty;
        private readonly SerializedProperty _guidProperty;

        public SerializedTypeReference(SerializedProperty typeReferenceProperty)
        {
            _typeNameProperty = typeReferenceProperty.FindPropertyRelative(TypeReference.NameOfTypeNameField);
            _guidProperty = typeReferenceProperty.FindPropertyRelative(TypeReference.NameOfGuidField);
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

        /// <summary>
        /// Try finding the class type given the GUID of the file where it is located.
        /// If found, change the ClassTypeReference._typeNameAndAssembly value.
        /// </summary>
        public void TryUpdatingTypeUsingGUID()
        {
            if (_guidProperty.stringValue == string.Empty)
                return;

            string assetPath = AssetDatabase.GUIDToAssetPath(_guidProperty.stringValue);
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
            if (script == null)
                return;

            Type type = script.GetClass();
            var previousValue = _typeNameProperty.stringValue;
            _typeNameProperty.stringValue = TypeReference.GetTypeNameAndAssembly(type);
            Debug.LogFormat(
                "Type reference has been updated from '{0}' to '{1}'.",
                previousValue,
                _typeNameProperty.stringValue);
        }

        private static string GetClassGuidFromTypeName(string typeName)
        {
            var type = Type.GetType(typeName);
            return ClassTypeReference.GetClassGUID(type);
        }
    }
}