namespace TypeReferences.Editor.TypeDropdown
{
    using System.Collections.Generic;

    /**
     * Part of the class, responsible solely for filling the tree with items. Only FillTreeWithItems method is used in
     * the main part of the class.
     */
    internal partial class SelectionTree
    {
        private void FillTreeWithItems(SortedSet<TypeItem> items)
        {
            if (items == null)
                return;

            foreach (TypeItem item in items)
                CreateDropdownItem(item);
        }

        private void CreateDropdownItem(TypeItem item)
        {
            (string namespaceName, string typeName) = SplitFullTypeName(item.Path);

            SelectionNode directParentOfNewNode =
                string.IsNullOrEmpty(namespaceName) ? _root : CreateFoldersInPathIfNecessary(namespaceName);

            directParentOfNewNode.CreateChildItem(typeName, item.Type, item.FullTypeName);
        }

        private static (string, string) SplitFullTypeName(string nodePath)
        {
            string namespaceName, typeName;
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

            return (namespaceName, typeName);
        }

        private SelectionNode CreateFoldersInPathIfNecessary(string path)
        {
            SelectionNode parentNode = _root;

            foreach (string folderName in path.Split('/'))
            {
                SelectionNode folderNode = parentNode.FindChild(folderName);

                if (folderNode == null)
                    folderNode = parentNode.CreateChildFolder(folderName);

                parentNode = folderNode;
            }

            return parentNode;
        }
    }
}