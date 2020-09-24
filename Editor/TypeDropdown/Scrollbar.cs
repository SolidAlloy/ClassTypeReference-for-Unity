namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using System.Reflection;
    using SolidUtilities.Editor.Helpers;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Assertions;

    internal class Scrollbar
    {
        private bool _visible = true;
        private Vector2 _position;
        private SelectionNode _nodeToScrollTo;
        private Rect _wholeListRect;
        private Rect _windowRect;

        private static bool ScrollCannotBePerformed => Event.current.type != EventType.Repaint;

        private static Rect VisibleRect => VisibleRectGetter.Get();

        public void DrawWithScrollbar(Action<Rect> drawContent)
        {
            EditorDrawHelper.DrawVertically(windowRect =>
            {
                if (Event.current.type == EventType.Repaint)
                    _windowRect = windowRect;

                DrawInScrollView(() =>
                {
                    Rect newWholeListRect = EditorDrawHelper.DrawVertically(() => { drawContent(VisibleRect); });

                    if (_wholeListRect.height == 0f || Event.current.type == EventType.Repaint)
                    {
                        _visible = _wholeListRect.height > _windowRect.height;
                        _wholeListRect = newWholeListRect;
                    }
                });
            });

            ScrollToNodeIfNeeded();
        }

        public void ToTop() => _position.y = 0f;

        public void RequestScrollToNode(SelectionNode node)
        {
            if (node == null)
                return;

            _nodeToScrollTo = node;

            foreach (SelectionNode parentNode in node.GetParentNodesRecursive(false))
                parentNode.Expanded = true;

            if (ScrollCannotBePerformed)
                return;

            ScrollToNode(node.Rect);
            _nodeToScrollTo = null;
        }

        private void ScrollToNodeIfNeeded()
        {
            if (_nodeToScrollTo == null || ScrollCannotBePerformed)
                return;

            ScrollToNode(_nodeToScrollTo.Rect);
            _nodeToScrollTo = null;
        }

        private void DrawInScrollView(Action drawContent)
        {
            _position = _visible
                ? EditorGUILayout.BeginScrollView(_position, GUILayout.ExpandHeight(false))
                : EditorGUILayout.BeginScrollView(_position, GUIStyle.none, GUIStyle.none, GUILayout.ExpandHeight(false));

            drawContent();

            EditorGUILayout.EndScrollView();
        }

        private void ScrollToNode(Rect nodeRect)
        {
            float windowHalfHeight = _windowRect.height * 0.5f; // This is needed to center the item vertically.
            _position.y = nodeRect.y - windowHalfHeight; // This scrolls to the node but places it in the center of the window.
        }

        private static class VisibleRectGetter
        {
            private static readonly PropertyInfo VisibleRectProperty = GetProperty();

            public static Rect Get() => (Rect) VisibleRectProperty.GetValue(null);

            private static PropertyInfo GetProperty()
            {
                const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic;
                Type guiClipType = typeof(GUI).Assembly.GetType("UnityEngine.GUIClip");
                PropertyInfo property = guiClipType.GetProperty("visibleRect", flags);
                Assert.IsNotNull(property);
                return property;
            }
        }
    }
}