namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using SolidUtilities.Editor.Extensions;
    using SolidUtilities.Editor.Helpers;
    using SolidUtilities.Extensions;
    using UnityEngine;
    using Util;

    internal partial class DropdownWindow
    {
        private PreventExpandingHeight _preventExpandingHeight;
        private float _contentHeight;
        private float _optimalWidth;
        private Rect _positionOnCreation;
        private bool _positionWasSetAfterCreation;

        public static float CalculateOptimalWidth(string[] selectionPaths)
        {
            float windowWidth = PopupHelper.CalculatePopupWidth(
                selectionPaths,
                DropdownStyle.DefaultLabelStyle,
                (int) DropdownStyle.GlobalOffset,
                (int) DropdownStyle.IndentWidth,
                false);

            return windowWidth < DropdownStyle.MinWindowWidth ? DropdownStyle.MinWindowWidth : windowWidth;
        }

        private Rect GetWindowRect(Vector2 windowPosition, float windowHeight)
        {
            var windowSize = new Vector2(_optimalWidth, GetWindowHeight(windowHeight));
            windowPosition.x = GetWindowXPosition(windowPosition.x, windowSize.x);
            windowPosition.y = GetWindowYPosition(windowPosition.y, windowSize.y);
            return new Rect(windowPosition, windowSize);
        }

        private float GetWindowHeight(float windowHeight)
        {
            // If given less than 100f, the window will re-position to the top left corner. If given 0f on MacOS,
            // the window may not appear at all. Thus, the minimal value is 100f.
            const float minHeightOnStart = 100f;
            return windowHeight < 100f ? minHeightOnStart : windowHeight;
        }

        private float GetWindowYPosition(float requestedYPosition, float windowHeight)
        {
            float distanceToBottomBorder = EditorGUIUtilityHelper.GetMainWindowPosition().yMax - requestedYPosition;

            if (distanceToBottomBorder < windowHeight)
            {
                return EditorGUIUtilityHelper.GetMainWindowPosition().yMax - windowHeight;
            }

            return requestedYPosition;
        }

        private float GetWindowXPosition(float requestedXPosition, float windowWidth)
        {
            // If the window width is smaller than the distance from cursor to the right border of the window, the
            // window will not appear because the cursor is outside of the window and OnGUI will never be called.
            float screenWidth = EditorGUIUtilityHelper.GetScreenWidth();
            requestedXPosition -=
                8f; // This will make the window appear so that foldout arrows are precisely below the cursor.
            float distanceToRightBorder = screenWidth - requestedXPosition;

            if (windowWidth > distanceToRightBorder)
            {
                requestedXPosition = screenWidth - windowWidth;
            }

            return requestedXPosition;
        }

        private void AdjustSizeIfNeeded()
        {
            float widthToSet = -1f;
            float heightToSet = -1f;

            if (_optimalWidth.DoesNotEqualApproximately(position.width))
                widthToSet = _optimalWidth;

            float wantedHeight = Math.Min(_contentHeight, DropdownStyle.MaxWindowHeight);

            if (_preventExpandingHeight && wantedHeight != 0f &&
                wantedHeight.DoesNotEqualApproximately(position.height))
                heightToSet = wantedHeight;

            if (widthToSet == -1f && heightToSet == -1f)
                return;

            this.Resize(widthToSet, heightToSet);
        }
    }
}