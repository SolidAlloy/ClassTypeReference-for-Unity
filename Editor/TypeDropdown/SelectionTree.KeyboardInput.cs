namespace TypeReferences.Editor.TypeDropdown
{
    using System.Linq;
    using SolidUtilities.Editor.Extensions;
    using UnityEngine;

    internal partial class SelectionTree
    {
        #region KeyboardEvents

        protected void HandleKeyboardEvents()
        {
            // KeyDown is always used somehow, so we use KeyUp here
            if (Event.current.type != EventType.KeyDown)
                return;

            switch (Event.current.keyCode)
            {
                case KeyCode.RightArrow:
                    OnArrowRight();
                    break;
                case KeyCode.LeftArrow:
                    OnArrowLeft();
                    break;
                case KeyCode.KeypadEnter:
                case KeyCode.Return:
                    OnEnter();
                    break;
                case KeyCode.DownArrow:
                    if (_noneElement is { IsSelected: true })
                    {
                        OnArrowDownNone();
                    }
                    else
                    {
                        OnArrowDown();
                    }
                    break;
                case KeyCode.UpArrow:
                    OnArrowUp();
                    break;
            }
        }

        private void OnArrowRight()
        {
            if (!SelectedNode.IsFolder || SelectedNode.Expanded)
                return;

            SelectedNode.Expanded = true;
            Event.current.Use();
        }

        private void OnArrowLeft()
        {
            if (!SelectedNode.IsFolder || !SelectedNode.Expanded)
                return;

            SelectedNode.Expanded = false;
            Event.current.Use();
        }

        private void OnEnter()
        {
            if (SelectedNode.IsFolder)
            {
                SelectedNode.Expanded = ! SelectedNode.Expanded;
                RequestRepaint();
            }
            else
            {
                FinalizeSelection();
            }
        }

        private void OnArrowDown()
        {
            if (SelectedNode.IsFolder && SelectedNode.Expanded)
            {
                SelectedNode = SelectedNode.ChildNodes[0];
            }
            else
            {
                if (SelectedNode.IsRoot)
                    return;

                SelectedNode = SelectedNode.ParentNode.GetNextChild(SelectedNode);

                if (!_visibleRect.Contains(SelectedNode.Rect))
                {
                    _scrollbar.RequestScrollToNode(SelectedNode, Scrollbar.NodePosition.Bottom);
                }
            }
        }

        private void OnArrowDownNone()
        {
            var firstItem = _root.ChildNodes.FirstOrDefault();

            if (firstItem != null)
                SelectedNode = firstItem;
        }

        private void OnArrowUp()
        {
            if (SelectedNode.IsRoot)
                return;

            if (SelectedNode.ParentNode.IsRoot)
            {
                bool isFirst = SelectedNode.ParentNode.ChildNodes.IndexOf(SelectedNode) == 0;

                if (isFirst && _noneElement != null)
                {
                    SelectedNode = _noneElement;
                    return;
                }
            }

            var previousNode = SelectedNode.ParentNode.GetPreviousChild(SelectedNode);

            if (IsExpandedFolder(previousNode))
            {
                // choose last item of the previous folder instead.
                previousNode = previousNode.ChildNodes[previousNode.ChildNodes.Count - 1];
            }

            SelectedNode = previousNode;

            if (!_visibleRect.Contains(SelectedNode.Rect))
            {
                _scrollbar.RequestScrollToNode(SelectedNode, Scrollbar.NodePosition.Top);
            }
        }

        private bool IsExpandedFolder(SelectionNode previousNode)
        {
            return SelectedNode.IsFolder && previousNode.IsFolder && previousNode.Expanded && previousNode.ChildNodes.Count != 0;
        }

        #endregion
    }
}