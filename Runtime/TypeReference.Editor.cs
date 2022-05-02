namespace TypeReferences
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
    using SolidUtilities.Editor;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;
#endif

    // This part of the class contains only the methods that are meant to be executed in Editor and not in builds.
    public partial class TypeReference
    {
        private void SubscribeToDelayCall()
        {
#if UNITY_EDITOR
            EditorApplication.delayCall += TryUpdatingTypeUsingGUID;
            EditorApplication.delayCall += LogTypeNotFoundIfNeeded;
#endif
        }

        private void UnsubscribeFromDelayCall()
        {
#if UNITY_EDITOR
            EditorApplication.delayCall -= TryUpdatingTypeUsingGUID;
            EditorApplication.delayCall -= LogTypeNotFoundIfNeeded;
#endif
        }

        private static string GetGUIDFromType(Type type)
        {
#if UNITY_EDITOR
            return AssetHelper.GetClassGUID(type);
#else
            return string.Empty;
#endif
        }

        private void TryUpdatingTypeUsingGUID()
        {
#if UNITY_EDITOR
            if (_type != null || string.IsNullOrEmpty(GUID))
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
            string previousTypeName = _typeNameAndAssembly;
            _typeNameAndAssembly = GetTypeNameAndAssembly(_type);

            if (! _suppressLogs)
                Debug.Log($"Type reference has been updated from '{previousTypeName}' to '{_typeNameAndAssembly}'.");
#endif
        }

        [Conditional("UNITY_EDITOR")]
        private static void ReportObjectsWithMissingValue(string typeName)
        {
#if UNITY_EDITOR
            bool firstLineLogged = false;

            var serializedObjects = ProjectDependencySearcher.GetSerializedObjectsFromOpenScenes(new ProjectDependencySearcher.FoundObjects());
            var typeReferenceProperties = SerializedPropertyHelper.FindPropertiesOfType(serializedObjects, "TypeReference");

            foreach (var typeReferenceProperty in typeReferenceProperties)
            {
                if (typeReferenceProperty.FindPropertyRelative(nameof(_typeNameAndAssembly)).stringValue != typeName
                    || typeReferenceProperty.FindPropertyRelative(nameof(_suppressLogs)).boolValue) // also don't report the missing type if the logs were suppressed for this TypeReference instance.
                {
                    continue;
                }

                var targetObject = typeReferenceProperty.serializedObject.targetObject;

                // Log the first message only if any objects with missing value were found.
                if (!firstLineLogged)
                {
                    Debug.LogWarning($"'{typeName}' was referenced but such type was not found. The value was set in the following objects:");
                    firstLineLogged = true;
                }
                
                Debug.Log($"<a>{targetObject.name}{(targetObject is MonoBehaviour ? $".{targetObject.GetType().Name}" : string.Empty)}</a>");
            }
#endif
        }
    }
}