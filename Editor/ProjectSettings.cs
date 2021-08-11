namespace TypeReferences.Editor
{
    using UnityEditor.SettingsManagement;

    internal static class ProjectSettings
    {
        private const string PackageName = "com.solidalloy.type-references";

        private static Settings _instance;

        private static UserSetting<int> _searchbarMinItemsCount;

        public static int SearchbarMinItemsCount
        {
            get
            {
                InitializeIfNeeded();
                return _searchbarMinItemsCount.value;
            }

            set => _searchbarMinItemsCount.value = value;
        }

        private static UserSetting<bool> _useBuiltInNames;

        public static bool UseBuiltInNames
        {
            get
            {
                InitializeIfNeeded();
                return _useBuiltInNames.value;
            }

            set => _useBuiltInNames.value = value;
        }

        private static void InitializeIfNeeded()
        {
            if (_instance != null)
                return;

            _instance = new Settings(PackageName);

            _searchbarMinItemsCount = new UserSetting<int>(_instance, nameof(_searchbarMinItemsCount), 10);
            _useBuiltInNames = new UserSetting<bool>(_instance, nameof(_useBuiltInNames), true);
        }
    }
}