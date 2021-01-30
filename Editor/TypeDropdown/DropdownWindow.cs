namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using SolidUtilities.Editor.Extensions;
    using SolidUtilities.Editor.Helpers;
    using SolidUtilities.Extensions;
    using UnityEditor;
    using UnityEngine;
    using Util;

    /// <summary>Creates a dropdown window that shows the <see cref="SelectionTree"/> elements.</summary>
    internal class DropdownWindow : EditorWindow
    {
        private SelectionTree _selectionTree;
        private PreventExpandingHeight _preventExpandingHeight;
        private float _contentHeight;
        private float _optimalWidth;

        private Rect _positionOnCreation;
        private bool _positionWasSetAfterCreation;

        public event Action OnClose;

        public static DropdownWindow Create(SelectionTree selectionTree, int windowHeight, Vector2 windowPosition)
        {
            var window = CreateInstance<DropdownWindow>();
            window.OnCreate(selectionTree, windowHeight, windowPosition);
            return window;
        }

        /// <summary>
        /// This is basically a constructor. Since ScriptableObjects cannot have constructors,
        /// this one is called from a factory method.
        /// </summary>
        /// <param name="selectionTree">Tree that contains the dropdown items to show.</param>
        /// <param name="windowHeight">Height of the window. If set to 0, it will be auto-adjusted.</param>
        /// <param name="windowPosition">Position of the window to set.</param>
        private void OnCreate(SelectionTree selectionTree, float windowHeight, Vector2 windowPosition)
        {
            ResetControl();
            wantsMouseMove = true;
            _selectionTree = selectionTree;
            _selectionTree.SelectionChanged += Close;
            _optimalWidth = CalculateOptimalWidth(_selectionTree.SelectionPaths);
            _preventExpandingHeight = new PreventExpandingHeight(windowHeight == 0f);

            _positionOnCreation = GetWindowRect(windowPosition, windowHeight);

            // ShowAsDropDown usually shows the window under a button, but since we don't need to align the window to
            // any button, we set buttonRect.height to 0f.
            Rect buttonRect = new Rect(_positionOnCreation) { height = 0f };

            ShowAsDropDown(buttonRect, _positionOnCreation.size);
        }

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

        private static void ResetControl()
        {
            GUIUtility.hotControl = 0;
            GUIUtility.keyboardControl = 0;
        }

        private Rect GetWindowRect(Vector2 windowPosition, float windowHeight)
        {
            // If the window width is smaller than the distance from cursor to the right border of the window, the
            // window will not appear because the cursor is outside of the window and OnGUI will never be called.
            float screenWidth = EditorDrawHelper.GetScreenWidth();
            windowPosition.x -= 8f; // This will make the window appear so that foldout arrows are precisely below the cursor.
            float distanceToRightBorder = screenWidth - windowPosition.x;

            if (_optimalWidth > distanceToRightBorder)
            {
                distanceToRightBorder = _optimalWidth;
                windowPosition.x = screenWidth - _optimalWidth;
            }

            // If given less than 100f, the window will re-position to the top left corner. If given 0f on MacOS,
            // the window may not appear at all. Thus, the minimal value is 100f.
            const float minHeightOnStart = 100f;
            windowHeight = windowHeight < 100f ? minHeightOnStart : windowHeight;

            float distanceToBottomBorder = EditorDrawHelper.GetMainWindowPosition().yMax - windowPosition.y;

            if (distanceToBottomBorder < windowHeight)
            {
                windowPosition.y = EditorDrawHelper.GetMainWindowPosition().yMax - windowHeight;
            }

            var windowSize = new Vector2(distanceToRightBorder, windowHeight);

            return new Rect(windowPosition, windowSize);
        }

        private void OnGUI()
        {
            CloseOnEscPress();
            DrawContent();
            RepaintIfMouseWasUsed();
        }

        private void Update()
        {
            // Sometimes, Unity resets the window position to 0,0 after showing it as a drop-down, so it is necessary
            // to set it again once the window was created.
            if (!_positionWasSetAfterCreation)
            {
                _positionWasSetAfterCreation = true;
                position = _positionOnCreation;
            }

            // If called in OnGUI, the dropdown blinks before appearing for some reason. Thus, it works well only in Update.
            AdjustSizeIfNeeded();
        }

        private void OnDestroy() => OnClose?.Invoke();

        private void AdjustSizeIfNeeded()
        {
            float widthToSet = -1f;
            float heightToSet = -1f;

            if (_optimalWidth.DoesNotEqualApproximately(position.width))
                widthToSet = _optimalWidth;

            float wantedHeight = Math.Min(_contentHeight, DropdownStyle.MaxWindowHeight);

            if (_preventExpandingHeight && wantedHeight != 0f && wantedHeight.DoesNotEqualApproximately(position.height))
                heightToSet = wantedHeight;

            if (widthToSet == -1f && heightToSet == -1f)
                return;

            this.Resize(widthToSet, heightToSet);
        }

        private void CloseOnEscPress()
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
                Close();
                Event.current.Use();
            }
        }

        private void DrawContent()
        {
            using (new FixedRect(_preventExpandingHeight, position.width))
            {
                using (new EditorDrawHelper.VerticalBlock(_preventExpandingHeight,
                    DropdownStyle.BackgroundColor, out float contentHeight))
                {
                    _selectionTree.Draw();

                    if (Event.current.type == EventType.Repaint)
                        _contentHeight = contentHeight;
                }

                EditorDrawHelper.DrawBorders(position.width, position.height, DropdownStyle.BorderColor);
            }
        }

        private void RepaintIfMouseWasUsed()
        {
            if (Event.current.isMouse || Event.current.type == EventType.Used)
                Repaint();
        }

        private readonly struct FixedRect : IDisposable
        {
            private readonly bool _enable;

            public FixedRect(bool enable, float windowWidth)
            {
                _enable = enable;

                if (_enable)
                    GUILayout.BeginArea(new Rect(0f, 0f, windowWidth, DropdownStyle.MaxWindowHeight));
            }

            public void Dispose()
            {
                if (_enable)
                    GUILayout.EndArea();
            }
        }
    }
}