namespace TypeReferences.Editor.TypeDropdown
{
    using System.Linq;
    using SolidUtilities.Editor.Extensions;
    using UnityEngine;

    internal partial class SelectionTree<T>
    {
        #region KeyboardEvents

        protected override void HandleKeyboardEvents()
        {
            if (Event.current.type != EventType.KeyDown)
                return;

            bool eventUsed = Event.current.keyCode switch
            {
                KeyCode.RightArrow => OnArrowRight(),
                KeyCode.LeftArrow => OnArrowLeft(),
                KeyCode.KeypadEnter => OnEnter(),
                KeyCode.Return => OnEnter(),
                KeyCode.DownArrow => OnArrowDown(),
                KeyCode.UpArrow => OnArrowUp(),
                _ => false
            };

            if (eventUsed)
                Event.current.Use();
        }

        private bool OnArrowRight()
        {
            if (_selectedNode == null || DrawInSearchMode || !_selectedNode.IsFolder || _selectedNode.Expanded)
                return false;

            _selectedNode.Expanded = true;
            return true;
        }

        private bool OnArrowLeft()
        {
            if (_selectedNode == null ||DrawInSearchMode || !_selectedNode.IsFolder || !_selectedNode.Expanded)
                return false;

            _selectedNode.Expanded = false;
            return true;
        }

        private bool OnEnter()
        {
            if (_selectedNode == null)
                return false;

            if (_selectedNode.IsFolder)
            {
                _selectedNode.Expanded = ! _selectedNode.Expanded;
            }
            else
            {
                FinalizeSelection();
            }

            return true;
        }

        private bool OnArrowDown()
        {
            if (_noneElement is { IsSelected: true })
                return OnArrowDownNone();

            if (DrawInSearchMode)
                return OnArrowDownSearch();

            return OnArrowDownRegular();
        }

        private bool OnArrowDownRegular()
        {
            if (_selectedNode == null)
            {
                if (_root.ChildNodes.Count == 0)
                {
                    return false;
                }

                _selectedNode = _root.ChildNodes[0];
                return true;
            }

            if (_selectedNode.IsFolder && _selectedNode.Expanded)
            {
                _selectedNode = _selectedNode.ChildNodes[0];
                return true;
            }

            if (_selectedNode.IsRoot)
                return false;

            _selectedNode = _selectedNode.ParentNode.GetNextChild(_selectedNode);

            if (!_visibleRect.Contains(_selectedNode.Rect))
            {
                _scrollbar.RequestScrollToNode(_selectedNode, Scrollbar.NodePosition.Bottom);
            }

            return true;
        }

        private bool OnArrowDownSearch()
        {
            if (_searchModeTree.Count == 0)
                return false;

            int indexOfSelected = _searchModeTree.IndexOf(_selectedNode);

            if (indexOfSelected == _searchModeTree.Count - 1)
                return false;

            if (indexOfSelected == -1)
            {
                _selectedNode = _searchModeTree[0];
                return true;
            }

            _selectedNode = _searchModeTree[indexOfSelected + 1];
            return true;
        }

        private bool OnArrowDownNone()
        {
            var firstItem = _root.ChildNodes.FirstOrDefault();

            if (firstItem == null)
                return false;

            _selectedNode = firstItem;
            return true;
        }

        private bool OnArrowUp()
        {
            if (DrawInSearchMode)
                return OnArrowUpSearch();

            if (_selectedNode == null || _selectedNode.IsRoot)
                return false;

            if (_selectedNode.ParentNode.IsRoot)
            {
                bool isFirst = _selectedNode.ParentNode.ChildNodes.IndexOf(_selectedNode) == 0;

                if (isFirst && _noneElement != null)
                {
                    _selectedNode = _noneElement;
                    return true;
                }
            }

            var previousNode = _selectedNode.ParentNode.GetPreviousChild(_selectedNode);

            if (IsExpandedFolder(previousNode) && !previousNode.ChildNodes.Contains(_selectedNode))
            {
                // choose last item of the previous folder instead.
                previousNode = previousNode.ChildNodes[previousNode.ChildNodes.Count - 1];
            }

            _selectedNode = previousNode;

            if (!_visibleRect.Contains(_selectedNode.Rect))
            {
                _scrollbar.RequestScrollToNode(_selectedNode, Scrollbar.NodePosition.Top);
            }

            return true;
        }

        private bool OnArrowUpSearch()
        {
            if (_searchModeTree.Count == 0)
                return false;

            int indexOfSelected = _searchModeTree.IndexOf(_selectedNode);

            if (indexOfSelected <= 0)
                return false;

            _selectedNode = _searchModeTree[indexOfSelected - 1];
            return true;
        }

        private bool IsExpandedFolder(SelectionNode<T> previousNode)
        {
            return previousNode.IsFolder && previousNode.Expanded && previousNode.ChildNodes.Count != 0;
        }

        #endregion
    }
}