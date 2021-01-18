namespace TypeReferences.Editor.Util
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine;

    /// <summary>Helps to draw a popup window.</summary>
    public static class PopupHelper
    {
        private const char Separator = '/';
        private const int ScrollbarWidth = 12;

        /// <summary>This makes a little indent after the longest item so that it is readable.</summary>
        private const int RightIndent = 5;

        private static readonly GUIContent _tempContent = new GUIContent();

        private static GUIContent TempContent(string text)
        {
            _tempContent.text = text;
            return _tempContent;
        }

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
            if (items.Length == 0)
                return 0;

            float charsPerIndent = GetCharsPerIndent(indentWidth, style);

            Item maxItem = default;

            foreach (string item in items)
            {
                Item itemStruct = GetMaxPart(item, charsPerIndent, flatTree);

                if (itemStruct > maxItem)
                {
                    maxItem = itemStruct;
                }
            }

            return maxItem.GetStringWidthInPixels(style, indentWidth) + globalOffsetWidth + ScrollbarWidth + RightIndent;
        }

        private static float GetCharsPerIndent(int indentWidth, GUIStyle style)
        {
            const string testString = "test";
            GUIContent testContent = TempContent(testString);
            float contentWidth = style.CalcSize(testContent).x;
            return indentWidth / contentWidth * testString.Length;
        }

        private static Item GetMaxPart(string item, float charsPerIndent, bool flatTree)
        {
            int indent = 0;

            if (flatTree)
                return new Item(item.AsSpan(), indent, charsPerIndent);

            Item maxPart = default;

            foreach (var part in item.AsSpan().Split(Separator))
            {
                var partItem = new Item(part, indent++, charsPerIndent);

                if (partItem > maxPart)
                {
                    maxPart = partItem;
                }
            }

            return maxPart;
        }

        private readonly ref struct Item
        {
            private readonly ReadOnlySpan<char> _string;
            private readonly int _indent;
            private readonly float _charsNumber;

            public Item(ReadOnlySpan<char> str, int indent, float charsPerIndent)
            {
                _string = str;
                _indent = indent;
                _charsNumber = _string.Length + charsPerIndent * _indent;
            }

            public int GetStringWidthInPixels(GUIStyle style, int indentWidth)
            {
                GUIContent itemContent = TempContent(_string.ToString());
                int stringWidth = (int) style.CalcSize(itemContent).x;
                return stringWidth + _indent * indentWidth;
            }

            public static bool operator >(Item left, Item right) => left._charsNumber > right._charsNumber;

            public static bool operator <(Item left, Item right) => ! (left > right);
        }
    }
}