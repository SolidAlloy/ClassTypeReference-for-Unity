namespace TypeReferences.Deprecated
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif


    /// <summary>
    /// Reference to a class <see cref="System.Type"/> with support for Unity serialization.
    /// </summary>
    [Serializable]
    public sealed class ClassTypeReference : ISerializationCallbackReceiver
    {
        /// <summary>
        /// Name of the element in the drop-down list that corresponds to null value.
        /// </summary>
        public const string NoneElement = "(None)";

        public const string NameOfTypeNameField = nameof(_typeNameAndAssembly);
        public const string NameOfGuidField = nameof(_GUID);

        [SerializeField] private string _typeNameAndAssembly;
        [SerializeField] private string _GUID;
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

        /// <summary>
        /// Get GUID of the file that contains the class of the given type.
        /// It works only for MonoBehaviours, ScriptableObjects, and other classes
        /// where the name of the file must match the class name.
        /// </summary>
        /// <param name="type">Type of the class to search for.</param>
        /// <returns>string representing the GUID of the file, or empty string if no file found.</returns>
        public static string GetClassGUID(Type type)
        {
            if (type == null || type.FullName == null)
                return string.Empty;

#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets(type.FullName);
            return guids.Length == 1 ? guids[0] : string.Empty;
#else
            return string.Empty;
#endif
        }

        public override string ToString()
        {
            if (Type != null && Type.FullName != null)
            {
                return Type.FullName;
            }
            else
            {
                return NoneElement;
            }
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
            {
                Debug.LogWarningFormat(
                    "'{0}' was referenced but class type was not found.",
                    _typeNameAndAssembly);
            }

            return type;
        }
    }
}
