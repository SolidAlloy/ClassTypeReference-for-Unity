namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using System.Collections.Generic;

    /**
     * Part of the class, responsible solely for filling the tree with items. Only FillTreeWithItems method is used in
     * the main part of the class.
     */
    internal partial class SelectionTree<T>
    {
        private void FillTreeWithItems(SelectionTreeItem<T>[] items)
        {
            if (items == null)
                return;

            foreach (var item in items)
                CreateDropdownItem(item);
        }

        private void CreateDropdownItem(in SelectionTreeItem<T> item)
        {
            SplitFullItemPath(item.Path, out string folderPath, out string itemName);
            var directParentOfNewNode = folderPath.Length == 0 ? _root : CreateFoldersInPathIfNecessary(folderPath);
            directParentOfNewNode.CreateChildItem(itemName, item.Value, item.SearchName);
        }

        private static void SplitFullItemPath(string nodePath, out string namespaceName, out string typeName)
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

        private SelectionNode<T> CreateFoldersInPathIfNecessary(string path)
        {
            var parentNode = _root;

            foreach (var folderName in path.AsSpan().Split('/'))
            {
                parentNode = parentNode.FindChild(folderName)
                             ?? parentNode.CreateChildFolder(folderName.ToString());
            }

            return parentNode;
        }
    }
}