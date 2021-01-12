namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using SolidUtilities.Editor.Helpers;
    using SolidUtilities.UnityEngineInternals;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// This class automates drawing the scrollbar next to a large list and scrolling to the selected node.
    /// </summary>
    internal class Scrollbar
    {
        private bool _visible = true;
        private Vector2 _position;
        private SelectionNode _nodeToScrollTo;
        private Rect _wholeListRect;
        private Rect _windowRect;

        private static bool ScrollCannotBePerformed => Event.current.type != EventType.Repaint;

        /// <summary>Draws elements with scrollbar if the list is large enough.</summary>
        /// <param name="drawContent">
        /// Action that takes a visible rect as an argument. Visible rect is the rect where GUI elements are in the
        /// window borders and are shown on screen.
        /// </param>
        public void DrawWithScrollbar(Action<Rect> drawContent)
        {
            EditorDrawHelper.DrawVertically(windowRect =>
            {
                if (Event.current.type == EventType.Repaint)
                    _windowRect = windowRect;

                DrawInScrollView(() =>
                {
                    Rect newWholeListRect = EditorDrawHelper.DrawVertically(() => drawContent(GUIClip.GetVisibleRect()));

                    if (_wholeListRect.height == 0f || Event.current.type == EventType.Repaint)
                    {
                        _visible = _wholeListRect.height > _windowRect.height;
                        _wholeListRect = newWholeListRect;
                    }
                });
            });

            ScrollToNodeIfNeeded();
        }

        /// <summary>Move thumb to the beginning of the list.</summary>
        public void ToTop() => _position.y = 0f;

        /// <summary>Ask scrollbar to start moving to a node. The process can take several frames.</summary>
        /// <param name="node">The node to scroll to.</param>
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
    }
}