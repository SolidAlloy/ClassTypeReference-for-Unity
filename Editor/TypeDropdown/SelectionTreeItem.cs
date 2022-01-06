namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using UnityEngine.Assertions;
    using Util;

    internal class SelectionTreeItem<T> : SelectionTreeItem
        where T : class
    {
        public readonly T Value;

        public SelectionTreeItem(T value, string path, string searchName) : base(path, searchName)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Item that contains <see cref="System.Type"/>, its full name, and path (full type name, formatted according to
    /// <see cref="Grouping"/>).
    /// </summary>
    internal class SelectionTreeItem
    {
        public readonly string Path;
        public readonly string SearchName;

        public SelectionTreeItem(string path, string searchName)
        {
            Assert.IsNotNull(searchName);
            SearchName = searchName;
            Path = path;
        }
    }
}