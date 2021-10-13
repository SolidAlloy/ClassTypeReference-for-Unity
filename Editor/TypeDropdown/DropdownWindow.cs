namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using SolidUtilities.Editor.Extensions;
    using SolidUtilities.Editor.Helpers;
    using SolidUtilities.Extensions;
    using UnityEditor;
    using UnityEngine;
    using Util;

    internal enum DropdownWindowType { Dropdown, Popup }

    /// <summary>Creates a dropdown window that shows the <see cref="SelectionTree"/> elements.</summary>
    internal partial class DropdownWindow : EditorWindow
    {
        private SelectionTree _selectionTree;

        public static DropdownWindow Create(SelectionTree selectionTree, int windowHeight, Vector2 windowPosition, DropdownWindowType windowType)
        {
            var window = CreateInstance<DropdownWindow>();
            window.OnCreate(selectionTree, windowHeight, windowPosition, windowType);
            return window;
        }

        /// <summary>
        /// This is basically a constructor. Since ScriptableObjects cannot have constructors,
        /// this one is called from a factory method.
        /// </summary>
        /// <param name="selectionTree">Tree that contains the dropdown items to show.</param>
        /// <param name="windowHeight">Height of the window. If set to 0, it will be auto-adjusted.</param>
        /// <param name="windowPosition">Position of the window to set.</param>
        private void OnCreate(SelectionTree selectionTree, float windowHeight, Vector2 windowPosition, DropdownWindowType windowType)
        {
            ResetControl();
            wantsMouseMove = true;
            _selectionTree = selectionTree;
            _selectionTree.SelectionChanged += Close;
            _optimalWidth = CalculateOptimalWidth(_selectionTree.SelectionPaths);
            _preventExpandingHeight = new PreventExpandingHeight(windowHeight == 0f);

            _positionOnCreation = GetWindowRect(windowPosition, windowHeight);

            if (windowType == DropdownWindowType.Dropdown)
            {
                // ShowAsDropDown usually shows the window under a button, but since we don't need to align the window to
                // any button, we set buttonRect.height to 0f.
                Rect buttonRect = new Rect(_positionOnCreation) { height = 0f };
                ShowAsDropDown(buttonRect, _positionOnCreation.size);
            }
            else if (windowType == DropdownWindowType.Popup)
            {
                position = _positionOnCreation;
                ShowPopup();
            }
            else
            {
                throw new Exception("Unknown window type");
            }
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

        private void OnLostFocus() => Close();

        private static void ResetControl()
        {
            GUIUtility.hotControl = 0;
            GUIUtility.keyboardControl = 0;
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
                using (EditorGUILayoutHelper.VerticalBlock(_preventExpandingHeight,
                    DropdownStyle.BackgroundColor, out float contentHeight))
                {
                    _selectionTree.Draw();

                    if (Event.current.type == EventType.Repaint)
                        _contentHeight = contentHeight;
                }

                EditorGUIHelper.DrawBorders(position.width, position.height, DropdownStyle.BorderColor);
            }
        }

        private void RepaintIfMouseWasUsed()
        {
            if (Event.current.isMouse || Event.current.type == EventType.Used || _selectionTree.RepaintRequested)
            {
                Repaint();
                _selectionTree.RepaintRequested = false;
            }
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