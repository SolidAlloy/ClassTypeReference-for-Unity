namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SolidUtilities.Helpers;

    internal partial class SelectionTree<T> : SelectionTree
        where T : class
    {
        private readonly Action<T> _onValueSelected;
        private readonly SelectionNode<T> _root;

        private readonly NoneElement<T> _noneElement;
        protected override SelectionNode NoneElement => _noneElement;

        private readonly List<SelectionNode<T>> _searchModeTree = new List<SelectionNode<T>>();
        protected override IReadOnlyCollection<SelectionNode> SearchModeTree => _searchModeTree;

        // private List<SelectionNode<T>> _nodes => _root.ChildNodes;
        protected override IReadOnlyCollection<SelectionNode> Nodes => _root.ChildNodes; // Or just expose root as SelectionNode
        protected override void InitializeSearchModeTree()
        {
            _searchModeTree.Clear();
            _searchModeTree.AddRange(EnumerateTreeTyped()
                .Where(node => node.Value != null)
                .Select(node =>
                {
                    bool includeInSearch = FuzzySearch.CanBeIncluded(_searchString, node.SearchName, out int score);
                    return new { score, item = node, include = includeInSearch };
                })
                .Where(x => x.include)
                .OrderByDescending(x => x.score)
                .Select(x => x.item));
        }

        private IEnumerable<SelectionNode<T>> EnumerateTreeTyped() => _root.GetChildNodesRecursive();

        protected override IEnumerable<SelectionNode> EnumerateTree() => EnumerateTreeTyped();

        public SelectionTree(
            SelectionTreeItem<T>[] items,
            T currentValue,
            Action<T> onValueSelected,
            int searchbarMinItemsCount,
            bool hideNoneElement)
            : base(items, searchbarMinItemsCount)
        {
            _root = SelectionNode<T>.CreateRoot(this);

            if ( ! hideNoneElement)
                _noneElement = NoneElement<T>.Create(this);

            FillTreeWithItems(items);

            SetSelection(items, currentValue);
            _onValueSelected = onValueSelected;
        }

        private void SetSelection(SelectionTreeItem<T>[] items, T selectedValue)
        {
            if (selectedValue == null)
            {
                _selectedNode = _noneElement;
                return;
            }

            ReadOnlySpan<char> nameOfItemToSelect = default;

            foreach (SelectionTreeItem<T> item in items)
            {
                if (item.Value == selectedValue)
                    nameOfItemToSelect = item.Path.AsSpan();
            }

            if (nameOfItemToSelect == default)
                return;

            var itemToSelect = _root;

            foreach (var part in nameOfItemToSelect.Split('/'))
                itemToSelect = itemToSelect.FindChild(part);

            _selectedNode = itemToSelect;
            _scrollbar.RequestScrollToNode(itemToSelect, Scrollbar.NodePosition.Center);
        }

        public override void FinalizeSelection()
        {
            base.FinalizeSelection();
            _onValueSelected?.Invoke(_selectedNode.Value);
        }

        private SelectionNode<T> _selectedNode;
        public override SelectionNode SelectedNode => _selectedNode;
        public void SetSelectedNode(SelectionNode<T> selectionNode) => _selectedNode = selectionNode;
    }
}