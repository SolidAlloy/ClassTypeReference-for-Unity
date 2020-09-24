namespace SolidUtilities.Editor.Helpers
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Allows to create a <see cref="GUIContent"/> instance and keep it in cache, reducing the garbage collection
    /// overhead.
    /// </summary>
    public class ContentCache
    {
        private readonly Dictionary<string, GUIContent> _contentCache = new Dictionary<string, GUIContent>();

        /// <summary>
        /// Get cached GUIContent or create a new one and cache it.
        /// </summary>
        /// <param name="text">Text in GUIContent.</param>
        /// <returns>GUIContent instance containing the text.</returns>
        /// <example><code>
        /// GUI.Label(placeHolderArea, ContentCache.GetItem(placeholder), PlaceholderStyle);
        /// </code></example>
        public GUIContent GetItem(string text)
        {
            if (_contentCache.TryGetValue(text, out GUIContent content))
                return content;

            content = new GUIContent(text);
            _contentCache.Add(text, content);
            return content;
        }
    }
}