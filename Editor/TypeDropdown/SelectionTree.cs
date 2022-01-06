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
    internal abstract class SelectionTree : IRepainter
    {
        private readonly string _searchFieldControlName = GUID.Generate().ToString();
        private readonly bool _drawSearchbar;
        protected readonly Scrollbar _scrollbar;

        protected string _searchString = string.Empty;
        protected Rect _visibleRect;
        public bool RepaintRequested;

        protected abstract IReadOnlyCollection<SelectionNode> SearchModeTree { get; }

        protected abstract SelectionNode NoneElement { get; }

        public SelectionTree(IReadOnlyCollection<SelectionTreeItem> items, int searchbarMinItemsCount)
        {
            SelectionPaths = items.Select(item => item.Path).ToArray();
            _drawSearchbar = items.Count >= searchbarMinItemsCount;
            _scrollbar = new Scrollbar(this);
        }

        public abstract SelectionNode SelectedNode { get; }

        public event Action SelectionChanged;

        public string[] SelectionPaths { get; }

        public bool DrawInSearchMode { get; private set; }

        protected abstract IReadOnlyCollection<SelectionNode> Nodes { get; }

        public virtual void FinalizeSelection()
        {
            SelectionChanged?.Invoke();
        }

        protected abstract IEnumerable<SelectionNode> EnumerateTree();

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
                NoneElement?.DrawSelfAndChildren(default, default);

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

            InitializeSearchModeTree();
        }

        protected abstract void InitializeSearchModeTree();

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
            var nodes = DrawInSearchMode ? SearchModeTree : Nodes;

            foreach (SelectionNode node in nodes)
                node.DrawSelfAndChildren(0, visibleRect);

            HandleKeyboardEvents();
        }

        protected abstract void HandleKeyboardEvents();

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