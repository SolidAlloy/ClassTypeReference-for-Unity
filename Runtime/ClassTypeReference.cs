// Copyright ClassTypeReference Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

namespace TypeReferences
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Reference to a class <see cref="System.Type"/> with support for Unity serialization.
    /// </summary>
    [Serializable]
    public sealed class ClassTypeReference : ISerializationCallbackReceiver
    {
        /// <summary>
        /// Name of the element in the drop-down list choosing of which will set the type to null.
        /// </summary>
        public const string NoneElement = "(None)";

        public const string NameOfTypeNameField = nameof(_typeNameAndAssembly);
        public const string NameOfGuidField = nameof(_GUID);
        public const string NameOfAssetPath = nameof(_assetPath);

        [SerializeField] private string _typeNameAndAssembly;
        [SerializeField] private string _GUID;
        [SerializeField] private string _assetPath;
        private Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassTypeReference"/> class with Type equal to null.
        /// </summary>
        public ClassTypeReference() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassTypeReference"/> class
        /// with Type equal to the type of the passed class.
        /// </summary>
        /// <param name="assemblyQualifiedClassName">Assembly qualified class name.</param>
        public ClassTypeReference(string assemblyQualifiedClassName)
        {
            Type = IsNotEmpty(assemblyQualifiedClassName)
                ? Type.GetType(assemblyQualifiedClassName)
                : null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassTypeReference"/> class
        /// with Type equal to the passed class type.
        /// </summary>
        /// <param name="type">Class type.</param>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="type"/> is not a class type.
        /// </exception>
        public ClassTypeReference(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets or sets type of class reference.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="value"/> is not a class type.
        /// </exception>
        public Type Type
        {
            get => _type;

            set
            {
                MakeSureValueIsClassType(value);
                _type = value;
                _typeNameAndAssembly = GetTypeNameAndAssembly(value);
                _GUID = GetClassGUID(value);
            }
        }

        public static implicit operator Type(ClassTypeReference typeReference)
        {
            return typeReference.Type;
        }

        public static implicit operator ClassTypeReference(Type type)
        {
            return new ClassTypeReference(type);
        }

        public static string GetTypeNameAndAssembly(Type type)
        {
            return type != null
                ? type.FullName + ", " + type.Assembly.GetName().Name
                : string.Empty;
        }

        public static string GetClassGUID(Type type)
        {
            if (type == null || type.FullName == null)
                return string.Empty;

            var guids = AssetDatabase.FindAssets(type.FullName);
            return guids.Length == 1 ? guids[0] : string.Empty;
        }

        public override string ToString()
        {
            return Type != null ? Type.FullName : NoneElement;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _type = IsNotEmpty(_typeNameAndAssembly) ? TryGetTypeFromSerializedFields() : null;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        private static bool IsNotEmpty(string value)
        {
            return ! string.IsNullOrEmpty(value);
        }

        private static void MakeSureValueIsClassType(Type value)
        {
            if (value != null && !value.IsClass)
                throw new ArgumentException($"'{value.FullName}' is not a class type.", nameof(value));
        }

        private Type TryGetTypeFromSerializedFields()
        {
            var type = Type.GetType(_typeNameAndAssembly);

            if (type == null)
                Debug.LogWarningFormat("'{0}' was referenced but class type was not found.", _typeNameAndAssembly);

            return type;
        }
    }
}
