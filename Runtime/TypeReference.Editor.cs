namespace TypeReferences
{
    using System;
    using System.Linq;
    using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
    using SolidUtilities.Editor;
    using UnityEditor;
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
            return AssetSearcher.GetClassGUID(type);
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
            string previousTypeName = TypeNameAndAssembly;
            TypeNameAndAssembly = GetTypeNameAndAssembly(_type);

            if (! _suppressLogs)
                Debug.Log($"Type reference has been updated from '{previousTypeName}' to '{TypeNameAndAssembly}'.");
#endif
        }

        private void ReportObjectsWithMissingValue()
        {
#if UNITY_EDITOR
            var foundObjects = AssetSearcher.FindObjectsWithValue(nameof(TypeNameAndAssembly), TypeNameAndAssembly);
            Debug.Log("The value is set in the following objects:");

            foreach (FoundObject foundObject in foundObjects)
            {
                var details = foundObject
                    .Select(detail => $"{detail.Key}: {detail.Value}");

                Debug.Log($"[{foundObject.Type}] {string.Join(", ", details)}");
            }
#endif
        }
    }
}