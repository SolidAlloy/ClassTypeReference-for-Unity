namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using System.Collections.Generic;
    using SolidUtilities.Editor.Extensions;
    using SolidUtilities.Editor.Helpers;
    using SolidUtilities.Extensions;
    using UnityEditor;
    using UnityEngine;

    /// <summary>Creates a dropdown window that shows the <see cref="SelectionTree"/> elements.</summary>
    internal class DropdownWindow : EditorWindow
    {
        private SelectionTree _selectionTree;
        private PreventExpandingHeight _preventExpandingHeight;
        private float _contentHeight;
        private float _optimalWidth;

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

            // If the window width is smaller than the distance from cursor to the right border of the window, the
            // window will not appear because the cursor is outside of the window and OnGUI will never be called.
            float distanceToRightBorder = Screen.currentResolution.width - windowPosition.x + 8f;
            var windowSize = new Vector2(Mathf.Max(distanceToRightBorder, _optimalWidth), windowHeight);

            // ShowAsDropDown usually shows the window under a button, but since we don't need to align the window to
            // any button, we set buttonRect.height to 0f.
            var buttonRect = new Rect(windowPosition, new Vector2(distanceToRightBorder, 0f));
            ShowAsDropDown(buttonRect, windowSize);
        }

        public static float CalculateOptimalWidth(IEnumerable<string> selectionPaths)
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

        private void OnGUI()
        {
            CloseOnEscPress();
            DrawContent();
            RepaintIfMouseWasUsed();
        }

        private void Update()
        {
            // If called in OnGUI, the dropdown blinks before appearing for some reason. Thus, it works well only in Update.
            AdjustSizeIfNeeded();
        }

        private void OnDestroy() => OnClose?.Invoke();

        private void AdjustSizeIfNeeded()
        {
            if (_optimalWidth.DoesNotEqualApproximately(position.width))
                this.Resize(_optimalWidth);

            if (_preventExpandingHeight || _contentHeight.DoesNotEqualApproximately(position.height))
                this.Resize(height: Math.Min(_contentHeight, DropdownStyle.MaxWindowHeight));
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
            DrawInFixedRectIfConditionIsMet(_preventExpandingHeight, () =>
            {
                float contentHeight = EditorDrawHelper.DrawVertically(_selectionTree.Draw, _preventExpandingHeight,
                    DropdownStyle.BackgroundColor);

                if (_contentHeight == 0f || Event.current.type == EventType.Repaint)
                    _contentHeight = contentHeight;

                EditorDrawHelper.DrawBorders(position.width, position.height, DropdownStyle.BorderColor);
            });
        }

        private void RepaintIfMouseWasUsed()
        {
            if (Event.current.isMouse || Event.current.type == EventType.Used)
                Repaint();
        }

        private void DrawInFixedRectIfConditionIsMet(bool condition, Action drawContent)
        {
            if (condition)
                GUILayout.BeginArea(new Rect(0.0f, 0.0f, position.width, DropdownStyle.MaxWindowHeight));

            drawContent();

            if (condition)
                GUILayout.EndArea();
        }
    }
}