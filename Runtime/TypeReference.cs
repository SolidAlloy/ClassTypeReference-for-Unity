namespace TypeReferences
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
    public sealed class TypeReference : ISerializationCallbackReceiver
    {
        /// <summary>
        /// Name of the element in the drop-down list that corresponds to null value.
        /// </summary>
        public const string NoneElement = "(None)";

        public const string NameOfTypeNameField = nameof(_typeNameAndAssembly);
        public const string NameOfGuidField = nameof(_GUID);

        public bool GuidAssignmentFailed;

        [SerializeField] private string _typeNameAndAssembly;
        [SerializeField] private string _GUID;
        private Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeReference"/> class with Type equal to null.
        /// </summary>
        public TypeReference() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeReference"/> class
        /// with Type equal to the name of the type passed in.
        /// </summary>
        /// <param name="assemblyQualifiedTypeName">Assembly qualified type name.</param>
        public TypeReference(string assemblyQualifiedTypeName)
        {
            Type = IsNotEmpty(assemblyQualifiedTypeName)
                ? Type.GetType(assemblyQualifiedTypeName)
                : null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeReference"/> class
        /// with Type equal to the passed type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="type"/> does not have a full name.
        /// </exception>
        public TypeReference(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets or sets type of class reference.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="value"/> does not have a full name.
        /// </exception>
        public Type Type
        {
            get => _type;

            set
            {
                MakeSureTypeHasName(value);

                _type = value;
                _typeNameAndAssembly = GetTypeNameAndAssembly(value);
                _GUID = GetClassGUID(value);
            }
        }

        public static implicit operator Type(TypeReference typeReference)
        {
            return typeReference.Type;
        }

        public static implicit operator TypeReference(Type type)
        {
            return new TypeReference(type);
        }

        public static string GetTypeNameAndAssembly(Type type)
        {
            MakeSureTypeHasName(type);

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
        /// <returns>String representing the GUID of the file, or empty string if no file found.</returns>
        public string GetClassGUID(Type type)
        {
            if (type == null || type.FullName == null)
                return string.Empty;

            string[] guids;

#if UNITY_EDITOR
            try
            {
                guids = AssetDatabase.FindAssets(type.FullName);
            }
            // It is thrown on assembly recompiling if field initialization is used on field.
            catch (UnityException)
            {
                GuidAssignmentFailed = true;
                return string.Empty;
            }

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

        private static void MakeSureTypeHasName(Type type)
        {
            if (type == null)
                return;

            if (type.FullName == null)
                throw new ArgumentException($"'{type}' does not have full name.", nameof(type));
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