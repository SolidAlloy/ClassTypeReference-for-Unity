namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SolidUtilities.Editor.Helpers;
    using SolidUtilities.Extensions;
    using SolidUtilities.Helpers;
    using SolidUtilities.UnityEngineInternals;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Represents a node tree that contains folders (namespaces) and types. It is also responsible for drawing all the
    /// nodes, along with search toolbar and scrollbar.
    /// </summary>
    internal partial class SelectionTree : IRepainter
    {
        private readonly SelectionNode _root;

        private readonly List<SelectionNode> _searchModeTree = new List<SelectionNode>();
        private readonly NoneElement _noneElement;
        private readonly string _searchFieldControlName = GUID.Generate().ToString();
        private readonly Action<Type> _onTypeSelected;
        private readonly bool _drawSearchbar;
        private readonly Scrollbar _scrollbar;

        private string _searchString = string.Empty;
        private Rect _visibleRect;
        public bool RepaintRequested;
        public SelectionNode SelectedNode;

        public SelectionTree(
            TypeItem[] items,
            Type selectedType,
            Action<Type> onTypeSelected,
            int searchbarMinItemsCount,
            bool hideNoneElement)
        {
            _root = SelectionNode.CreateRoot(this);

            if ( ! hideNoneElement)
                _noneElement = NoneElement.Create(this);

            SelectionPaths = items.Select(item => item.Path).ToArray();
            FillTreeWithItems(items);
            _drawSearchbar = items.Length >= searchbarMinItemsCount;

            _scrollbar = new Scrollbar(this);
            SetSelection(items, selectedType);
            _onTypeSelected = onTypeSelected;
        }

        public event Action SelectionChanged;

        public string[] SelectionPaths { get; }

        public bool DrawInSearchMode { get; private set; }

        private List<SelectionNode> Nodes => _root.ChildNodes;

        public void FinalizeSelection()
        {
            _onTypeSelected?.Invoke(SelectedNode.Type);
            SelectionChanged?.Invoke();
        }

        public void ExpandAllFolders()
        {
            foreach (SelectionNode node in EnumerateTree())
                node.Expanded = true;
        }

        public void Draw()
        {
            if (Nodes.Count == 0)
            {
                DrawInfoMessage();
                return;
            }

            if (_drawSearchbar)
            {
                using (new EditorDrawHelper.SearchToolbarStyle(DropdownStyle.SearchToolbarHeight))
                    DrawSearchToolbar();
            }

            if ( ! DrawInSearchMode)
                _noneElement?.Draw();

            using (_scrollbar.Draw())
            {
                _visibleRect = GUIClip.GetVisibleRect();
                DrawTree(_visibleRect);
            }
        }

        private static void DrawInfoMessage()
        {
            using (new GUILayoutHelper.Vertical(DropdownStyle.NoPadding))
            {
                EditorGUILayoutHelper.DrawInfoMessage("No types to select.");
            }
        }

        private IEnumerable<SelectionNode> EnumerateTree() => _root.GetChildNodesRecursive();

        private void SetSelection(TypeItem[] items, Type selectedType)
        {
            if (selectedType == null)
            {
                SelectedNode = _noneElement;
                return;
            }

            ReadOnlySpan<char> nameOfItemToSelect = default;

            foreach (TypeItem item in items)
            {
                if (item.Type == selectedType)
                    nameOfItemToSelect = item.Path.AsSpan();
            }

            if (nameOfItemToSelect == default)
                return;

            SelectionNode itemToSelect = _root;

            foreach (var part in nameOfItemToSelect.Split('/'))
                itemToSelect = itemToSelect.FindChild(part);

            SelectedNode = itemToSelect;
            _scrollbar.RequestScrollToNode(itemToSelect, Scrollbar.NodePosition.Center);
        }

        private void DrawSearchToolbar()
        {
            Rect innerToolbarArea = GetInnerToolbarArea();

            EditorGUI.BeginChangeCheck();
            _searchString = DrawSearchField(innerToolbarArea, _searchString);
            bool changed = EditorGUI.EndChangeCheck();

            if ( ! changed)
                return;

            if (string.IsNullOrEmpty(_searchString))
            {
                DisableSearchMode();
            }
            else
            {
                EnableSearchMode();
            }
        }

        private void DisableSearchMode()
        {
            // Without GUI.changed, the change will take place only on mouse move.
            GUI.changed = true;
            DrawInSearchMode = false;
            _scrollbar.RequestScrollToNode(SelectedNode, Scrollbar.NodePosition.Center);
        }

        private void EnableSearchMode()
        {
            if ( ! DrawInSearchMode)
                _scrollbar.ToTop();

            DrawInSearchMode = true;

            _searchModeTree.Clear();
            _searchModeTree.AddRange(EnumerateTree()
                .Where(node => node.Type != null)
                .Select(node =>
                {
                    bool includeInSearch = FuzzySearch.CanBeIncluded(_searchString, node.FullTypeName, out int score);
                    return new { score, item = node, include = includeInSearch };
                })
                .Where(x => x.include)
                .OrderByDescending(x => x.score)
                .Select(x => x.item));
        }

        private static Rect GetInnerToolbarArea()
        {
            Rect outerToolbarArea = GUILayoutUtility.GetRect(
                0f,
                DropdownStyle.SearchToolbarHeight,
                GUILayoutHelper.ExpandWidth(true));

            Rect innerToolbarArea = outerToolbarArea
                .AddHorizontalPadding(10f, 2f)
                .AlignMiddleVertically(DropdownStyle.LabelHeight);

            return innerToolbarArea;
        }

        private void DrawTree(Rect visibleRect)
        {
            List<SelectionNode> nodes = DrawInSearchMode ? _searchModeTree : Nodes;
            int nodesListLength = nodes.Count;

            for (int index = 0; index < nodesListLength; ++index)
                nodes[index].DrawSelfAndChildren(0, visibleRect);

            HandleKeyboardEvents();
        }

        private string DrawSearchField(Rect innerToolbarArea, string searchText)
        {
            (Rect searchFieldArea, Rect buttonRect) = innerToolbarArea.CutVertically(DropdownStyle.IconSize, true);

            bool keyDown = Event.current.type == EventType.KeyDown;

            searchText = EditorGUIHelper.FocusedTextField(searchFieldArea, searchText, "Search",
                DropdownStyle.SearchToolbarStyle, _searchFieldControlName);

            // When the search field is in focus, it uses the keyDown event on DownArrow while doing nothing.
            // We need this event for moving through the tree nodes.
            if (keyDown)
                Event.current.type = EventType.KeyDown;

            if (GUIHelper.CloseButton(buttonRect))
            {
                searchText = string.Empty;
                GUI.FocusControl(null); // Without this, the old text does not disappear for some reason.
                GUI.changed = true;
            }

            return searchText;
        }

        public void RequestRepaint()
        {
            RepaintRequested = true;
        }
    }
}