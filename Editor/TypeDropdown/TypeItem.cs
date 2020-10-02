namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Util;

    /// <summary>
    /// Item that contains <see cref="System.Type"/>, its full name, and path (full type name, formatted according to
    /// <see cref="Grouping"/>).
    /// </summary>
    internal readonly struct TypeItem
    {
        public readonly string Path;
        public readonly Type Type;
        public readonly string FullTypeName;

        public TypeItem(Type type, Grouping grouping)
            : this(type, type.FullName ?? string.Empty, grouping) { }

        public TypeItem(Type type, string fullTypeName, Grouping grouping)
        {
            Assert.IsNotNull(fullTypeName);
            FullTypeName = fullTypeName;
            Type = type;
            Path = TypeNameFormatter.Format(Type, FullTypeName, grouping);
        }
    }

    internal class TypeItemComparer : IComparer<TypeItem>
    {
        public int Compare(TypeItem x, TypeItem y)
        {
            return string.Compare(x.Path, y.Path, StringComparison.Ordinal);
        }
    }
}