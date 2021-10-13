namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using System.Collections.Generic;

    /**
     * Part of the class, responsible solely for filling the tree with items. Only FillTreeWithItems method is used in
     * the main part of the class.
     */
    internal partial class SelectionTree
    {
        private void FillTreeWithItems(TypeItem[] items)
        {
            if (items == null)
                return;

            foreach (TypeItem item in items)
                CreateDropdownItem(item);
        }

        private void CreateDropdownItem(in TypeItem item)
        {
            SplitFullTypeName(item.Path, out string namespaceName, out string typeName);

            SelectionNode directParentOfNewNode =
                namespaceName.Length == 0 ? _root : CreateFoldersInPathIfNecessary(namespaceName);

            directParentOfNewNode.CreateChildItem(typeName, item.Type, item.FullTypeName);
        }

        private static void SplitFullTypeName(string nodePath, out string namespaceName, out string typeName)
        {
            int indexOfLastSeparator = nodePath.LastIndexOf('/');

            if (indexOfLastSeparator == -1)
            {
                namespaceName = string.Empty;
                typeName = nodePath;
            }
            else
            {
                namespaceName = nodePath.Substring(0, indexOfLastSeparator);
                typeName = nodePath.Substring(indexOfLastSeparator + 1);
            }
        }

        private SelectionNode CreateFoldersInPathIfNecessary(string path)
        {
            SelectionNode parentNode = _root;

            foreach (var folderName in path.AsSpan().Split('/'))
            {
                parentNode = parentNode.FindChild(folderName)
                             ?? parentNode.CreateChildFolder(folderName.ToString());
            }

            return parentNode;
        }
    }
}