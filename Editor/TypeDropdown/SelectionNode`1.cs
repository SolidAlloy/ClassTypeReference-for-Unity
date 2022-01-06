namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class SelectionNode<T> : SelectionNode
        where T : class
    {
        public readonly T Value;

        public new readonly SelectionNode<T> ParentNode;

        private readonly SelectionTree<T> _parentTree;
        protected override SelectionTree ParentTree => _parentTree;

        public SelectionNode(T value, SelectionNode<T> parentNode, SelectionTree<T> parentTree, string name, string searchName)
            : base(parentNode, name, searchName)
        {
            ParentNode = parentNode;
            _parentTree = parentTree;
            Value = value;
            ParentNode = parentNode;
        }

        /// <summary>Creates a root node that does not have a parent and does not show up in the popup.</summary>
        /// <param name="parentTree">The tree this node belongs to.</param>
        /// <returns>The root node.</returns>
        public static SelectionNode<T> CreateRoot(SelectionTree<T> parentTree) => new SelectionNode<T>(null, null, parentTree, string.Empty, null);

        /// <summary>Creates a dropdown item that represents a <see cref="System.Type"/>.</summary>
        /// <param name="name">Name that will show up in the popup.</param>
        /// <param name="type"><see cref="System.Type"/>> this node represents.</param>
        /// <param name="fullTypeName">
        /// Full name of the type. It will show up instead of the short name when performing search.
        /// </param>
        public void CreateChildItem(string name, T value, string searchName)
        {
            var child = new SelectionNode<T>(value, this, _parentTree, name, searchName);
            ChildNodes.Add(child);
        }

        /// <summary>Creates a folder that contains dropdown items.</summary>
        /// <param name="name">Name of the folder.</param>
        /// <returns>A <see cref="SelectionNode"/> instance that represents the folder.</returns>
        public SelectionNode<T> CreateChildFolder(string name)
        {
            var child = new SelectionNode<T>(null, this, _parentTree, name, null);
            ChildNodes.Add(child);
            return child;
        }

        public IEnumerable<SelectionNode<T>> GetChildNodesRecursive()
        {
            if ( ! IsRoot)
                yield return this;

            foreach (var childNode in ChildNodes.SelectMany(node => node.GetChildNodesRecursive()))
                yield return childNode;
        }

        public readonly List<SelectionNode<T>> ChildNodes = new List<SelectionNode<T>>();
        protected override IReadOnlyCollection<SelectionNode> _ChildNodes => ChildNodes;

        protected override void SetSelfSelected()
        {
            _parentTree.SetSelectedNode(this);
        }

        public SelectionNode<T> GetNextChild(SelectionNode<T> currentChild)
        {
            int currentIndex = ChildNodes.IndexOf(currentChild);

            if (currentIndex < 0)
                return currentChild;

            if (currentIndex == ChildNodes.Count - 1)
                return ParentNode?.GetNextChild(this) ?? currentChild;

            return ChildNodes[currentIndex + 1];
        }

        public SelectionNode<T> GetPreviousChild(SelectionNode<T> currentChild)
        {
            int currentIndex = ChildNodes.IndexOf(currentChild);

            if (currentIndex < 0)
                return currentChild;

            if (currentIndex == 0)
                return this;

            return ChildNodes[currentIndex - 1];
        }

        /// <summary>
        /// Returns the direct child node with the matching name, or null if the matching node was not found.
        /// </summary>
        /// <remarks>
        /// One of the usages of FindNode is to build the selection tree. When a new item is added, it is checked
        /// whether its parent folder is already created. If the folder is created, it is usually the most recently
        /// created folder, so the list is iterated backwards to give the result as quickly as possible.
        /// </remarks>
        /// <param name="name">Name of the node to find.</param>
        /// <returns>Direct child node with the matching name or null.</returns>
        public SelectionNode<T> FindChild(ReadOnlySpan<char> name)
        {
            for (int index = ChildNodes.Count - 1; index >= 0; --index)
            {
                if (name.Equals(ChildNodes[index]._name.AsSpan(), StringComparison.Ordinal))
                    return ChildNodes[index];
            }

            return null;
        }
    }
}