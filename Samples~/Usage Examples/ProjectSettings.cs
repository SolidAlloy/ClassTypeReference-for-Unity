namespace TypeReferences.Demo
{
    using UnityEditor;
    using Utils;

    public class ProjectSettings : TypeReferenceExample
    {
        [InfoBox("You can customize the look of the drop-down further in Project Settings. Hover over the setting label to find out what it means.")]
        [Button]
        public void GoToSettings()
        {
#if UNITY_EDITOR
            SettingsService.OpenProjectSettings("Project/Type References");
#endif
        }
    }
}