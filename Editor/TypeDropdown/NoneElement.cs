namespace TypeReferences.Editor.TypeDropdown
{
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// A node that represents the null type value. It is drawn separately from other nodes and has its own root.
    /// </summary>
    internal class NoneElement : SelectionNode
    {
        private NoneElement(SelectionNode root, SelectionTree parentTree)
            : base(TypeReference.NoneElement, root, parentTree, null, null) { }

        public static NoneElement Create(SelectionTree parentTree)
        {
            SelectionNode root = CreateRoot(parentTree);
            var child = new NoneElement(root, parentTree);
            root.ChildNodes.Add(child);
            return child;
        }

        public void Draw()
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