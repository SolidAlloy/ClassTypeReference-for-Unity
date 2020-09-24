namespace SolidUtilities.Editor.Helpers
{
    using System;
    using UnityEditor;
    using UnityEngine;

    /// <summary>Different useful methods that simplify <see cref="EditorGUILayout"/> API.</summary>
    public static class EditorDrawHelper
    {
        /// <summary>
        /// Cache that creates <see cref="GUIContent"/> instances and keeps them, reducing the garbage
        /// collection overhead.
        /// </summary>
        public static readonly ContentCache ContentCache = new ContentCache();

        private const float PlaceholderIndent = 14f;

        private static readonly GUIStyle SearchToolbarStyle = new GUIStyle(EditorStyles.toolbar)
        {
            padding = new RectOffset(0, 0, 0, 0),
            stretchHeight = true,
            stretchWidth = true,
            fixedHeight = 0f
        };

        private static readonly GUIStyle InfoMessageStyle = new GUIStyle( "HelpBox")
        {
            margin = new RectOffset(4, 4, 2, 2),
            fontSize = 10,
            richText = true
        };

        private static readonly GUIStyle PlaceholderStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
        {
            alignment = TextAnchor.MiddleLeft,
            clipping = TextClipping.Clip,
            margin = new RectOffset(4, 4, 4, 4)
        };

        /// <summary>Draws content in an automatically laid out scroll view.</summary>
        /// <param name="scrollPos">Position of the thumb.</param>
        /// <param name="drawContent">Action that draws the content in the scroll view.</param>
        /// <returns>The new thumb position.</returns>
        /// <example><code>
        /// _thumbPos = EditorDrawHelper.DrawInScrollView(_thumbPos, () =>
        /// {
        ///     float contentHeight = EditorDrawHelper.DrawVertically(_selectionTree.Draw, _preventExpandingHeight,
        ///         DropdownStyle.BackgroundColor);
        /// });
        /// </code></example>
        public static Vector2 DrawInScrollView(Vector2 scrollPos, Action drawContent)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            drawContent();
            EditorGUILayout.EndScrollView();
            return scrollPos;
        }

        /// <summary>Draws content in the vertical direction.</summary>
        /// <param name="drawContent">Action that draws the content.</param>
        /// <param name="option">Option to draw the vertical group with.</param>
        /// <param name="backgroundColor">Background of the vertical group rectangle.</param>
        /// <returns>Height of the vertical group rectangle.</returns>
        /// <example><code>
        /// float contentHeight = EditorDrawHelper.DrawVertically(_selectionTree.Draw, _preventExpandingHeight,
        ///     DropdownStyle.BackgroundColor);
        /// </code></example>
        public static float DrawVertically(Action drawContent, GUILayoutOption option, Color backgroundColor)
        {
            Rect rect = EditorGUILayout.BeginVertical(option);
            EditorGUI.DrawRect(rect, backgroundColor);
            drawContent();
            EditorGUILayout.EndVertical();
            return rect.height;
        }

        /// <summary>Draws content in the vertical direction.</summary>
        /// <param name="drawContent">Action that draws the content.</param>
        /// <returns>Rectangle of the vertical group.</returns>
        /// <example><code>
        /// Rect newWholeListRect = EditorDrawHelper.DrawVertically(() =>
        /// {
        ///     for (int index = 0; index &lt; nodes.Count; ++index)
        ///         nodes[index].DrawSelfAndChildren(0, visibleRect);
        /// });
        /// </code></example>
        public static Rect DrawVertically(Action drawContent)
        {
            Rect rect = EditorGUILayout.BeginVertical();
            drawContent();
            EditorGUILayout.EndVertical();
            return rect;
        }

        /// <summary>Draws content in the vertical direction.</summary>
        /// <param name="drawContent">Action that draws the content.</param>
        /// <example><code>
        /// EditorDrawHelper.DrawVertically(windowRect =>
        /// {
        ///     if (Event.current.type == EventType.Repaint)
        ///         _windowRect = windowRect;
        ///
        ///     for (int index = 0; index &lt; nodes.Count; ++index)
        ///         nodes[index].DrawSelfAndChildren(0, visibleRect);
        /// });
        /// </code></example>
        public static void DrawVertically(Action<Rect> drawContent)
        {
            Rect rect = EditorGUILayout.BeginVertical();
            drawContent(rect);
            EditorGUILayout.EndVertical();
        }

        /// <summary>Draws borders with a given color and width around a rectangle.</summary>
        /// <param name="rectWidth">Width of the rectangle.</param>
        /// <param name="rectHeight">Height of the rectangle.</param>
        /// <param name="color">Color of the borders.</param>
        /// <param name="borderWidth">Width of the borders.</param>
        /// <example><code>
        /// EditorDrawHelper.DrawBorders(position.width, position.height, DropdownStyle.BorderColor);
        /// </code></example>
        public static void DrawBorders(float rectWidth, float rectHeight, Color color, float borderWidth = 1f)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            EditorGUI.DrawRect(new Rect(0f, 0f, borderWidth, rectHeight), color);
            EditorGUI.DrawRect(new Rect(0f, 0f, rectWidth, borderWidth), color);
            EditorGUI.DrawRect(new Rect(rectWidth - borderWidth, 0f, borderWidth, 0f), color);
            EditorGUI.DrawRect(new Rect(0f, rectHeight - borderWidth, rectWidth, borderWidth), color);
        }

        /// <summary>Draws search toolbar with the search toolbar style.</summary>
        /// <param name="drawToolbar">Action that draws the toolbar.</param>
        /// <param name="toolbarHeight">Height of the toolbar.</param>
        /// <example><code>
        /// EditorDrawHelper.DrawWithSearchToolbarStyle(DrawSearchToolbar, DropdownStyle.SearchToolbarHeight);
        /// </code></example>
        public static void DrawWithSearchToolbarStyle(Action drawToolbar, float toolbarHeight)
        {
            EditorGUILayout.BeginHorizontal(
                SearchToolbarStyle,
                GUILayout.Height(toolbarHeight),
                DrawHelper.ExpandWidth(false));

            drawToolbar();

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>Shows the info message.</summary>
        /// <param name="message">The message to output.</param>
        /// <example><code>EditorDrawHelper.DrawInfoMessage("No types to select.");</code></example>
        public static void DrawInfoMessage(string message)
        {
            var messageContent = new GUIContent(message, EditorIcons.Info);
            Rect labelPos = EditorGUI.IndentedRect(GUILayoutUtility.GetRect(messageContent, InfoMessageStyle));
            GUI.Label(labelPos, messageContent, InfoMessageStyle);
        }

        /// <summary>Draws content and checks if it was changed.</summary>
        /// <param name="drawContent">Action that draws the content.</param>
        /// <returns>Whether the content was changed.</returns>
        /// <example><code>
        /// bool changed = EditorDrawHelper.CheckIfChanged(() =>
        /// {
        ///     _searchString = DrawSearchField(innerToolbarArea, _searchString);
        /// });
        /// </code></example>
        public static bool CheckIfChanged(Action drawContent)
        {
            EditorGUI.BeginChangeCheck();
            drawContent();
            return EditorGUI.EndChangeCheck();
        }

        /// <summary>Draws a text field that is always focused.</summary>
        /// <param name="rect">Rectangle to draw the field in.</param>
        /// <param name="text">The text to show in the field.</param>
        /// <param name="placeholder">Placeholder to show if the field is empty.</param>
        /// <param name="style">Style to draw the field with.</param>
        /// <param name="controlName">Unique control name of the field.</param>
        /// <returns>The text that was written to the field.</returns>
        /// <example><code>
        /// searchText = EditorDrawHelper.FocusedTextField(searchFieldArea, searchText, "Search",
        ///     DropdownStyle.SearchToolbarStyle, _searchFieldControlName);
        /// </code></example>
        public static string FocusedTextField(Rect rect, string text, string placeholder, GUIStyle style, string controlName)
        {
            GUI.SetNextControlName(controlName);
            text = EditorGUI.TextField(rect, text, style);
            EditorGUI.FocusTextInControl(controlName);

            if (Event.current.type == EventType.Repaint && string.IsNullOrEmpty(text))
            {
                var placeHolderArea = new Rect(rect.x + PlaceholderIndent, rect.y, rect.width - PlaceholderIndent, rect.height);
                GUI.Label(placeHolderArea, ContentCache.GetItem(placeholder), PlaceholderStyle);
            }

            return text;
        }
    }
}