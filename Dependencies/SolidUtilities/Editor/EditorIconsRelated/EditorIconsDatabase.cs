namespace SolidUtilities.Editor.EditorIconsRelated
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Scriptable object that holds references to resources needed for <see cref="EditorIcons"/>. With this database,
    /// we only need to know a GUID of the scriptable object instead of GUIDS or paths to all the resources used
    /// in <see cref="EditorIcons"/>.
    /// </summary>
    internal class EditorIconsDatabase : ScriptableObject
    {
        [SerializeField, Multiline(6)] private string _description;

        public Texture2D TriangleRight = null;
        [SerializeField] private Material _activeDarkSkin = null;
        [SerializeField] private Material _activeLightSkin = null;
        [SerializeField] private Material _highlightedDarkSkin = null;
        [SerializeField] private Material _highlightedLightSkin = null;

        public Material Active => DarkSkin ? _activeDarkSkin : _activeLightSkin;
        public Material Highlighted => DarkSkin ? _highlightedDarkSkin : _highlightedLightSkin;

        private static bool DarkSkin => EditorGUIUtility.isProSkin;
    }
}