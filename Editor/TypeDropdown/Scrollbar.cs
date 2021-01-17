namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using SolidUtilities.Editor.Helpers;
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
        /// <returns>
        /// A scrollbar scope that automatically lays out elements inside the list and places a thumb in the correct position.
        /// </returns>
        public ScrollbarScope Draw() => new ScrollbarScope(this);

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

        private void ScrollToNode(Rect nodeRect)
        {
            float windowHalfHeight = _windowRect.height * 0.5f; // This is needed to center the item vertically.
            _position.y = nodeRect.y - windowHalfHeight; // This scrolls to the node but places it in the center of the window.
        }

        /// <summary>Draws elements with scrollbar if the list is large enough.</summary>
        public readonly struct ScrollbarScope : IDisposable
        {
            private readonly Scrollbar _scrollbar;
            private readonly EditorDrawHelper.VerticalBlock _outerVerticalBlock;
            private readonly EditorDrawHelper.ScrollView _scrollView;
            private readonly EditorDrawHelper.VerticalBlock _innerVerticalBlock;
            private readonly Rect _newWholeListRect;

            public ScrollbarScope(Scrollbar scrollbar)
            {
                _scrollbar = scrollbar;

                _outerVerticalBlock = new EditorDrawHelper.VerticalBlock(out Rect windowRect);

                if (Event.current.type == EventType.Repaint)
                    _scrollbar._windowRect = windowRect;

                _scrollView = new EditorDrawHelper.ScrollView(ref _scrollbar._position, _scrollbar._visible);

                _innerVerticalBlock = new EditorDrawHelper.VerticalBlock(out _newWholeListRect);
            }

            public void Dispose()
            {
                if (_scrollbar._wholeListRect.height == 0f || Event.current.type == EventType.Repaint)
                {
                    _scrollbar._visible = _scrollbar._wholeListRect.height > _scrollbar._windowRect.height;
                    _scrollbar._wholeListRect = _newWholeListRect;
                }

                _innerVerticalBlock.Dispose();
                _scrollView.Dispose();
                _outerVerticalBlock.Dispose();

                _scrollbar.ScrollToNodeIfNeeded();
            }
        }
    }
}