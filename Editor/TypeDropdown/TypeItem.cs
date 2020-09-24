namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using System.Collections.Generic;
    using Util;

    internal readonly struct TypeItem
    {
        public readonly string Path;
        public readonly Type Type;
        public readonly string FullTypeName;

        public TypeItem(Type type, Grouping grouping)
        {
            FullTypeName = type.FullName ?? string.Empty;
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