namespace TypeReferences
{
    using System.Collections.Generic;

    /// <summary>Custom comparer that allows to build TypeReference collections.</summary>
    public class TypeReferenceComparer : IEqualityComparer<TypeReference>
    {
        public bool Equals(TypeReference x, TypeReference y) => x?.Type == y?.Type;

        public int GetHashCode(TypeReference obj) =>
            string.IsNullOrEmpty(obj.GUID) ? obj.Type.GetHashCode() : obj.GUID.GetHashCode();
    }
}