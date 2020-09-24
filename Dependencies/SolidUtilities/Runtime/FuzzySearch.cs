namespace SolidUtilities
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Implementation of the fuzzy search algorithm.
    /// </summary>
    public static class FuzzySearch
    {
        private const int BaseCharScore = 5;
        private const int ScoreForEqualChars = 2;

        /// <summary>
        /// Determines if an item should be included in the search result and outputs its score (its position in the
        /// list of matching items.)
        /// </summary>
        /// <param name="searchString">Search string to compare the item to.</param>
        /// <param name="itemName">Name of the item to include in the result list.</param>
        /// <param name="score">Score of the item that determines how high in the result list it should be placed.</param>
        /// <returns>Whether to include the result in the list.</returns>
        /// <example><code>
        /// EnumerateTree()
        /// .Where(node => node.Type != null)
        /// .Select(node =>
        /// {
        ///     bool includeInSearch = FuzzySearch.CanBeIncluded(_searchString, node.FullTypeName, out int score);
        ///     return new { score, item = node, include = includeInSearch };
        /// })
        /// .Where(x => x.include)
        /// .OrderByDescending(x => x.score)
        /// .Select(x => x.item));
        /// </code></example>
        public static bool CanBeIncluded(string searchString, string itemName, out int score)
        {
            /*
             * The score for one char starts from 5. For each char in item name that matches the corresponding char in
             * search string, the score per char increases by 2. If chars don't match, the score is reset to the base
             * value. The base value depends on whether the previous char of item name was a letter (2) or not (5).
             * Finally, the difference between item name length and search string length is subtracted from the final
             * score.
             *
             * Whether the item can be included in search depends on whether all the letters from search string were
             * found in the item name.
             */

            score = 0;

            if (string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(itemName))
                return false;

            int searchStringLength = searchString.Length;
            int itemNameLength = itemName.Length;

            int searchStringIndex = 0;

            char searchStringChar = GetNextSearchStringChar(searchString, searchStringLength,
                ref searchStringIndex, out bool searchStringCharIsUpper);

            int charScore = BaseCharScore;
            bool previousItemNameCharIsNotLetter = true;

            for (int itemNameIndex = 0; itemNameIndex < itemNameLength; itemNameIndex++)
            {
                char itemNameChar = itemName[itemNameIndex];

                MakeCharactersSameCase(searchStringCharIsUpper, previousItemNameCharIsNotLetter, ref itemNameChar,
                    ref charScore);

                if (searchStringChar == itemNameChar)
                {
                    score += charScore;
                    charScore += ScoreForEqualChars;
                    searchStringIndex++;
                    if (searchStringIndex == searchStringLength)
                        break;

                    searchStringChar = GetNextSearchStringChar(searchString, searchStringLength, ref searchStringIndex,
                        out searchStringCharIsUpper);
                }
                else if ( ! searchStringCharIsUpper || char.IsUpper(itemNameChar))
                {
                    charScore = previousItemNameCharIsNotLetter ? BaseCharScore : ScoreForEqualChars;
                }

                previousItemNameCharIsNotLetter = ! char.IsLetter(itemNameChar);
            }

            score -= itemNameLength - searchStringIndex;
            return searchStringIndex == searchStringLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char GetNextSearchStringChar(string searchString, int searchStringLength,
            ref int searchStringIndex, out bool searchStringCharIsUpper)
        {
            char searchStringChar = NextNonWhitespaceChar(searchString, searchStringLength, ref searchStringIndex);

            searchStringCharIsUpper = char.IsUpper(searchStringChar);
            if ( ! searchStringCharIsUpper && IsUppercaseLetter(searchStringChar))
                searchStringChar = char.ToLower(searchStringChar);

            return searchStringChar;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char NextNonWhitespaceChar(string searchString, int searchStringLength, ref int charIndex)
        {
            char character = searchString[charIndex];

            while (character == ' ' && ++charIndex < searchStringLength)
                character = searchString[charIndex];

            return character;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsUppercaseLetter(char character)
        {
            return character >= 'A' && character <= 'Z';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MakeCharactersSameCase(bool searchStringCharIsUpper, bool previousItemNameCharIsNotLetter,
            ref char itemNameChar, ref int charScore)
        {
            if (searchStringCharIsUpper)
            {
                if (previousItemNameCharIsNotLetter)
                {
                    charScore = BaseCharScore;
                    itemNameChar = char.ToUpper(itemNameChar);
                }
            }
            else if (IsUppercaseLetter(itemNameChar))
            {
                itemNameChar = char.ToLower(itemNameChar);
            }
        }
    }
}