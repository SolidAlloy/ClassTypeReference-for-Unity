namespace TypeReferences.Editor.TypeDropdown
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// A node that represents the null type value. It is drawn separately from other nodes and has its own root.
    /// </summary>
    internal class NoneElement<T> : SelectionNode<T>
        where T : class
    {
        private NoneElement(SelectionNode<T> root, SelectionTree<T> parentTree)
            : base(null, root, parentTree, TypeReference.NoneElement, null) { }

        public static NoneElement<T> Create(SelectionTree<T> parentTree)
        {
            var root = CreateRoot(parentTree);
            var child = new NoneElement<T>(root, parentTree);
            root.ChildNodes.Add(child);
            return child;
        }

        public override void DrawSelfAndChildren(int indentLevel, Rect visibleRect)
        {
            if (ReserveSpaceAndStop())
                return;

            if (Event.current.type == EventType.Repaint)
                DrawNodeContent(0, 1);

            HandleMouseEvents();

            DrawBottomSeparator();
        }

        private void DrawBottomSeparator()
        {
            var lineRect = new Rect(Rect.x, Rect.y + Rect.height - 1f, Rect.width, 1f);
            EditorGUI.DrawRect(lineRect, DropdownStyle.BorderColor);
        }
    }
}