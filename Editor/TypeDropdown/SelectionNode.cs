namespace TypeReferences.Editor.TypeDropdown
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using SolidUtilities.Editor;
  using SolidUtilities.Editor.EditorIconsRelated;
  using SolidUtilities.Extensions;
  using UnityEditor;
  using UnityEngine;
  using UnityEngine.Assertions;

  internal class SelectionNode
  {
    public readonly List<SelectionNode> ChildNodes = new List<SelectionNode>();
    public readonly Type Type;

    private readonly string _name;
    private readonly SelectionTree _parentTree;
    private readonly SelectionNode _parentNode;

    private bool _expanded;
    private Rect _rect;

    /// <summary>
    /// Default constructor that creates a child node of another parent node.
    /// </summary>
    /// <param name="name">Name that will show up in the popup.</param>
    /// <param name="parentNode">Parent node of this node.</param>
    /// <param name="parentTree">The tree this node belongs to.</param>
    /// <param name="type"><see cref="System.Type"/>> this node represents.</param>
    /// <param name="fullTypeName">
    /// Full name of the type. It will show up instead of the short name when performing search.
    /// </param>
    protected SelectionNode(string name, SelectionNode parentNode, SelectionTree parentTree, Type type, string fullTypeName)
    {
      Assert.IsNotNull(name);

      _name = name;
      _parentNode = parentNode;
      _parentTree = parentTree;
      Type = type;
      FullTypeName = fullTypeName;
    }

    /// <summary>Constructor of a root node that does not have a parent and does not show up in the popup.</summary>
    /// <param name="parentTree">The tree this node belongs to.</param>
    private SelectionNode(SelectionTree parentTree)
    {
      _parentNode = null;
      _parentTree = parentTree;
      _name = string.Empty;
      Type = null;
      FullTypeName = null;
    }

    /// <summary>Creates a root node that does not have a parent and does not show up in the popup.</summary>
    /// <param name="parentTree">The tree this node belongs to.</param>
    /// <returns>The root node.</returns>
    public static SelectionNode CreateRoot(SelectionTree parentTree) => new SelectionNode(parentTree);

    /// <summary>Creates a dropdown item that represents a <see cref="System.Type"/>.</summary>
    /// <param name="name">Name that will show up in the popup.</param>
    /// <param name="type"><see cref="System.Type"/>> this node represents.</param>
    /// <param name="fullTypeName">
    /// Full name of the type. It will show up instead of the short name when performing search.
    /// </param>
    /// <returns>A <see cref="SelectionNode"/> instance that represents the dropdown item.</returns>
    public SelectionNode CreateChildItem(string name, Type type, string fullTypeName)
    {
      var child = new SelectionNode(name, this, _parentTree, type, fullTypeName);
      ChildNodes.Add(child);
      return child;
    }

    /// <summary>Creates a folder that contains dropdown items.</summary>
    /// <param name="name">Name of the folder.</param>
    /// <returns>A <see cref="SelectionNode"/> instance that represents the folder.</returns>
    public SelectionNode CreateChildFolder(string name)
    {
      var child = new SelectionNode(name, this, _parentTree, null, null);
      ChildNodes.Add(child);
      return child;
    }

    public Rect Rect => _rect;

    public string FullTypeName { get; }

    /// <summary>
    /// Makes a folder expanded or closed.
    /// It can be set for dropdown items but will do anything as they cannot be expanded.
    /// </summary>
    public bool Expanded
    {
      get => IsFolder && _expanded;
      set => _expanded = value;
    }

    private bool IsSelected => _parentTree.SelectedNode == this;

    private bool IsFolder => ChildNodes.Count != 0;

    private bool IsHoveredOver => _rect.Contains(Event.current.mousePosition);

    private bool IsRoot => _parentNode == null;

    public void Select() => _parentTree.SelectedNode = this;

    public IEnumerable<SelectionNode> GetChildNodesRecursive()
    {
      if ( ! IsRoot)
        yield return this;

      foreach (SelectionNode childNode in ChildNodes.SelectMany(node => node.GetChildNodesRecursive()))
        yield return childNode;
    }

    public IEnumerable<SelectionNode> GetParentNodesRecursive(
      bool includeSelf)
    {
      if (includeSelf)
        yield return this;

      if (IsRoot)
        yield break;

      foreach (SelectionNode node in _parentNode.GetParentNodesRecursive(true))
        yield return node;
    }

    /// <summary>
    /// Returns the direct child node with the matching name, or null if the matching node was not found.
    /// </summary>
    /// <remarks>
    /// One of the usages of FindNode is to build the selection tree. When a new item is added, it is checked whether
    /// its parent folder is already created. If the folder is created, it is usually the most recently created folder,
    /// so the list is iterated backwards to give the result as quickly as possible.
    /// </remarks>
    /// <param name="name">Name of the node to find.</param>
    /// <returns>Direct child node with the matching name or null.</returns>
    public SelectionNode FindChild(string name)
    {
      for (int index = ChildNodes.Count - 1; index >= 0; --index)
      {
        if (ChildNodes[index]._name == name)
          return ChildNodes[index];
      }

      return null;
    }

    public void DrawSelfAndChildren(int indentLevel, Rect visibleRect)
    {
      Draw(indentLevel, visibleRect);
      if ( ! Expanded)
        return;

      foreach (SelectionNode childItem in ChildNodes)
        childItem.DrawSelfAndChildren(indentLevel + 1, visibleRect);
    }

    /// <summary>
    /// Reserves a space for the rect but does not draw its content.
    /// </summary>
    /// <returns>True if there is no need to draw the contents.</returns>
    protected bool ReserveSpaceAndStop()
    {
      Rect buttonRect = GUILayoutUtility.GetRect(0f, DropdownStyle.NodeHeight);

      if (Event.current.type == EventType.Layout)
        return true;

      if (Event.current.type == EventType.Repaint || _rect.width == 0f)
        _rect = buttonRect;

      return false;
    }

    protected void DrawNodeContent(int indentLevel, int raiseText = 0)
    {
      if (IsSelected)
      {
        EditorGUI.DrawRect(_rect, DropdownStyle.SelectedColor);
      }
      else if (IsHoveredOver)
      {
        EditorGUI.DrawRect(_rect, DropdownStyle.HighlightedColor);
      }

      Rect indentedNodeRect = _rect;
      indentedNodeRect.xMin += DropdownStyle.GlobalOffset + indentLevel * DropdownStyle.IndentWidth;
      indentedNodeRect.y -= raiseText;

      if (IsFolder)
      {
        Rect triangleRect = GetTriangleRect(indentedNodeRect);
        DrawTriangleIcon(triangleRect);
      }

      DrawLabel(indentedNodeRect);

      DrawSeparator();
    }

    protected void HandleMouseEvents()
    {
      bool leftMouseButtonWasPressed = Event.current.type == EventType.MouseDown
                                       && IsHoveredOver
                                       && Event.current.button == 0;

      if ( ! leftMouseButtonWasPressed)
        return;

      if (IsFolder)
        Expanded = !Expanded;
      else
        Select();

      Event.current.Use();
    }

    private void Draw(int indentLevel, Rect visibleRect)
    {
      if (ReserveSpaceAndStop())
        return;

      if (_rect.y > 1000f && NodeIsOutsideOfVisibleRect(visibleRect))
        return;

      if (Event.current.type == EventType.Repaint)
        DrawNodeContent(indentLevel);

      HandleMouseEvents();
    }

    private bool NodeIsOutsideOfVisibleRect(Rect visibleRect) =>
      _rect.y + _rect.height < visibleRect.y || _rect.y > visibleRect.y + visibleRect.height;

    private Rect GetTriangleRect(Rect nodeRect)
    {
      Rect triangleRect = nodeRect.AlignMiddleVertically(DropdownStyle.IconSize);
      triangleRect.width = DropdownStyle.IconSize;
      triangleRect.x -= DropdownStyle.IconSize;
      return triangleRect;
    }

    private void DrawTriangleIcon(Rect triangleRect)
    {
      EditorIcon triangleIcon = Expanded ? EditorIcons.TriangleDown : EditorIcons.TriangleRight;

      Texture2D tintedIcon = IsHoveredOver
        ? triangleIcon.Highlighted
        : triangleIcon.Active;

      tintedIcon.Draw(triangleRect);
    }

    private void DrawLabel(Rect indentedNodeRect)
    {
      Rect labelRect = indentedNodeRect.AlignMiddleVertically(DropdownStyle.LabelHeight);
      string label = _parentTree.DrawInSearchMode ? FullTypeName : _name;
      GUIStyle style = IsSelected ? DropdownStyle.SelectedLabelStyle : DropdownStyle.DefaultLabelStyle;
      GUI.Label(labelRect, label, style);
    }

    private void DrawSeparator()
    {
      var lineRect = new Rect(_rect.x, _rect.y - 1f, _rect.width, 1f);
      EditorGUI.DrawRect(lineRect, DropdownStyle.DarkSeparatorLine);
      ++lineRect.y;
      EditorGUI.DrawRect(lineRect, DropdownStyle.LightSeparatorLine);
    }
  }
}