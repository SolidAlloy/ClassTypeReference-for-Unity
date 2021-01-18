namespace TypeReferences.Editor.Util
{
    using System;
    using System.Diagnostics.CodeAnalysis;
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
            // The method utilizes ReadOnlySpan<char> to iterate over items to reduce memory allocations.
            // The goal is to find the longest string in the tree so that is is fully visible when the list is scrolled.
            // However, indents also need to be accounted for since they also take up some width.
            // Calculating the width each string in the list takes up in pixels is expensive, so it is done only once at the end, in the GetStringWidthInPixels method.
            // First, it is calculated, approximately, how many characters fit in one indent.
            // Instead of comparing items length in pixels, the method just compares the number of characters they have.
            // When comparing strings lengths, indents are also accounted for by adding the number of characters that fit in the indent of an item.
            // This gives a pretty precise approximate answer even for very large lists (4000 items, +-1 pixel error).

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
            int partIndent = 0;
            var itemSpan = item.AsSpan();
            int itemLength = item.Length;

            if (flatTree)
                return new Item(itemSpan, partIndent, charsPerIndent, itemLength);

            // Iterates over an item, splits it in parts by Separator and checks the length of every part.
            // Returns the length of the longest part.
            Item maxPart = default;

            int previousSeparatorIndex = 0;
            int i;

            for (i = 0; i < itemLength; i++)
            {
                if (item[i] != Separator)
                    continue;

                int length = i - previousSeparatorIndex;

                var part = new Item(itemSpan.Slice(previousSeparatorIndex, length), partIndent,
                    charsPerIndent, length);

                previousSeparatorIndex = i + 1;
                partIndent++;

                if (part > maxPart)
                {
                    maxPart = part;
                }
            }

            int lastLength = i - previousSeparatorIndex;

            var lastPart = new Item(itemSpan.Slice(previousSeparatorIndex, lastLength), partIndent,
                charsPerIndent, lastLength);

            if (lastPart > maxPart)
            {
                maxPart = lastPart;
            }

            return maxPart;
        }

        private readonly ref struct Item
        {
            private readonly ReadOnlySpan<char> _span;
            private readonly int _indent;
            private readonly float _charsNumber;

            public Item(ReadOnlySpan<char> span, int indent, float charsPerIndent, int spanLength)
            {
                _span = span;
                _indent = indent;

                // span.Length creates a defensive copy. Since we slice a span before passing it into the struct,
                // we already know its length and passing it as a parameter is cheaper.
                _charsNumber = spanLength + charsPerIndent * _indent;
            }

            public static bool operator >(Item left, Item right) => left._charsNumber > right._charsNumber;

            public static bool operator <(Item left, Item right) => ! (left > right);

            [SuppressMessage("ReSharper", "EPS06",
                Justification = "The method is called only once, so creating a defensive copy is alright")]
            public int GetStringWidthInPixels(GUIStyle style, int indentWidth)
            {
                GUIContent itemContent = TempContent(_span.ToString());
                int stringWidth = (int) style.CalcSize(itemContent).x;
                return stringWidth + _indent * indentWidth;
            }
        }
    }
}