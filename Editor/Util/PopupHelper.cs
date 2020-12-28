namespace TypeReferences.Editor.Util
{
    using System;
    using JetBrains.Annotations;
    using SolidUtilities.Editor.Helpers;
    using UnityEngine;

    /// <summary>Helps to draw a popup window.</summary>
    public static class PopupHelper
    {
        private const char Separator = '/';
        private const int ScrollbarWidth = 12;

        /// <summary>This makes a little indent after the longest item so that it is readable.</summary>
        private const int RightIndent = 5;

        /// <summary>
        /// Calculates recommended width for a popup window with a tree of items so that all items in the tree
        /// are readable.
        /// </summary>
        /// <param name="items">Paths to the items in popup. Parts of the path should be separated by slashes.</param>
        /// <param name="style">Style of the items' labels.</param>
        /// <param name="globalOffsetWidth">Global offset in pixels all items have, if any.</param>
        /// <param name="indentWidth">Indent width in pixels each new subfolder has in the path.</param>
        /// <param name="flatTree">Calculate recommended width for a flat tree.</param>
        /// <returns>The recommended width a popup window should have.</returns>
        /// <example><code>
        /// float windowWidth = PopupHelper.CalculatePopupWidth(
        ///     _selectionTree.SelectionPaths,
        ///     DropdownStyle.DefaultLabelStyle,
        ///     (int) DropdownStyle.GlobalOffset,
        ///     (int) DropdownStyle.IndentWidth,
        ///     false);
        /// </code></example>
        [PublicAPI]
        public static int CalculatePopupWidth(string[] items, GUIStyle style, int globalOffsetWidth, int indentWidth, bool flatTree)
        {
            int maxItemWidth = 0;

            if ( items.Length == 0)
                return maxItemWidth;

            foreach (string item in items)
            {
                int itemWidth = GetItemWidth(item, style, indentWidth, flatTree);
                if (itemWidth > maxItemWidth)
                    maxItemWidth = itemWidth;
            }

            maxItemWidth += globalOffsetWidth + ScrollbarWidth + RightIndent;
            return maxItemWidth;
        }

        private static int GetItemWidth(string item, GUIStyle style, int indentWidth, bool flatTree)
        {
            if (flatTree)
                return GetStringWidthInPixels(item, style);

            var parts = item.Split(Separator);
            int partsCount = parts.Length;
            int maxPartWidth = 0;

            for (int i = 0; i < partsCount; i++)
            {
                int partWidth = GetStringWidthInPixels(parts[i], style) + i * indentWidth;

                if (partWidth > maxPartWidth)
                    maxPartWidth = partWidth;
            }

            return maxPartWidth;
        }

        private static int GetStringWidthInPixels(string item, GUIStyle style)
        {
            GUIContent itemContent = EditorDrawHelper.ContentCache.GetItem(item);
            Vector2 size = style.CalcSize(itemContent);
            return Convert.ToInt32(size.x);
        }
    }
}