namespace TypeReferences
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine;
    using UnityEngine.Serialization;
#if UNITY_EDITOR
    using SolidUtilities.Editor.Extensions;
    using UnityEditor;
#endif

    /// <summary>
    /// Reference to a class <see cref="System.Type"/> with support for Unity serialization.
    /// </summary>
    [Serializable]
    public class TypeReference : ISerializationCallbackReceiver
    {
        /// <summary>Name of the element in the drop-down list that corresponds to null value.</summary>
        internal const string NoneElement = "(None)";

        [SerializeField] internal bool GuidAssignmentFailed;
        [SerializeField] internal string GUID;

        [FormerlySerializedAs("_typeNameAndAssembly")]
        [SerializeField] internal string TypeNameAndAssembly;

        [SerializeField] private bool _suppressLogs;

        private Type _type;

        private bool _needToLogTypeNotFound;
        private bool _typeChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeReference"/> class with Type equal to null.
        /// </summary>
        /// <remarks>
        /// The fully empty constructor without even named parameters is needed for the class to be deserialized in
        /// IL2CPP builds. For some reason, if the class doesn't have a fully empty constructor, OnAfterDeserialize
        /// will not be called in IL2CPP builds.</remarks>
        public TypeReference()
        {
            _suppressLogs = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeReference"/> class with Type equal to null.
        /// </summary>
        /// <param name="suppressLogs">
        /// Whether to suppress logs that show up when a type disappeared or was renamed. Default is <c>false</c>.
        /// </param>
        public TypeReference(bool suppressLogs = false)
        {
            _suppressLogs = suppressLogs;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeReference"/> class
        /// with Type equal to the name of the type passed in.
        /// </summary>
        /// <param name="assemblyQualifiedTypeName">Assembly qualified type name.</param>
        /// <param name="suppressLogs">
        /// Whether to suppress logs that show up when a type disappeared or was renamed. Default is <c>false</c>.
        /// </param>
        public TypeReference(string assemblyQualifiedTypeName, bool suppressLogs = false)
            : this(suppressLogs)
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
        /// <param name="suppressLogs">
        /// Whether to suppress logs that show up when a type disappeared or was renamed. Default is <c>false</c>.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="type"/> does not have a full name.
        /// </exception>
        public TypeReference(Type type, bool suppressLogs = false)
            : this(suppressLogs)
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

                if (_type != value)
                    _typeChanged = true;

                _type = value;
                TypeNameAndAssembly = GetTypeNameAndAssembly(value);
            }
        }

        public static implicit operator Type(TypeReference typeReference) => typeReference?.Type;

        public static implicit operator TypeReference(Type type) => new TypeReference(type);

        public override string ToString() => (Type != null && Type.FullName != null) ? Type.FullName : NoneElement;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _type = IsNotEmpty(TypeNameAndAssembly) ? TryGetTypeFromSerializedFields() : null;
#if UNITY_EDITOR
            EditorApplication.delayCall += TryUpdatingTypeUsingGUID;
            EditorApplication.delayCall += LogTypeNotFoundIfNeeded;
#endif
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (_typeChanged)
            {
                SetClassGuidIfExists(Type);
                _typeChanged = false;
            }

#if UNITY_EDITOR
            EditorApplication.delayCall -= TryUpdatingTypeUsingGUID;
            EditorApplication.delayCall -= LogTypeNotFoundIfNeeded;
#endif
        }

        internal static string GetTypeNameAndAssembly(Type type)
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
        internal static string GetClassGUID(Type type)
        {
            if (type == null || type.FullName == null)
                return string.Empty;

#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets(type.Name);

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);

                if (asset == null)
                    continue;

                if (asset.GetClassType() == type)
                    return guid;
            }
#endif
            return string.Empty;
        }

        private static bool IsNotEmpty(string value) => ! string.IsNullOrEmpty(value);

        private static void MakeSureTypeHasName(Type type)
        {
            if (type != null && type.FullName == null)
                throw new ArgumentException($"'{type}' does not have full name.", nameof(type));
        }

        private void TryUpdatingTypeUsingGUID()
        {
#if UNITY_EDITOR
            if (_type != null || GUID == string.Empty)
                return;

            string assetPath = AssetDatabase.GUIDToAssetPath(GUID);
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);

            if (script == null)
            {
                LogTypeNotFound();
                GUID = string.Empty;
                return;
            }

            var type = script.GetClassType();
            if (type == null)
            {
                LogTypeNotFound();
                GUID = string.Empty;
                return;
            }

            _type = type;
            string previousTypeName = TypeNameAndAssembly;
            TypeNameAndAssembly = GetTypeNameAndAssembly(_type);

            if (! _suppressLogs)
                Debug.Log($"Type reference has been updated from '{previousTypeName}' to '{TypeNameAndAssembly}'.");
#endif
        }

        private void LogTypeNotFound()
        {
            if (! _suppressLogs)
                Debug.LogWarning($"'{TypeNameAndAssembly}' was referenced but such type was not found.");
        }

        /// <summary>
        /// Sometimes, the fact that the type disappeared is found during the deserialization. A warning cannot be
        /// logged during the deserialization, so it must be delayed with help of this method.
        /// </summary>
        private void LogTypeNotFoundIfNeeded()
        {
            if (! _needToLogTypeNotFound)
                return;

            LogTypeNotFound();
            _needToLogTypeNotFound = false;
        }

        private void SetClassGuidIfExists(Type type)
        {
            try
            {
                GUID = GetClassGUID(type);
            }
            catch (UnityException) // thrown on assembly recompiling if field initialization is used on field.
            {
                // GUID will be found in SerializedTypeReference.SetGuidIfAssignmentFailed()
                GuidAssignmentFailed = true;
                GUID = string.Empty;
            }
        }

        [CanBeNull]
        private Type TryGetTypeFromSerializedFields()
        {
            var type = Type.GetType(TypeNameAndAssembly);

            // If GUID is not empty, there is still hope the type will be found in the TryUpdatingTypeUsingGUID method.
            if (type == null && GUID == string.Empty)
                _needToLogTypeNotFound = true;

            return type;
        }
    }
}