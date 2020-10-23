namespace TypeReferences
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>Custom comparer that allows to build TypeReference collections.</summary>
    public class TypeReferenceComparer : EqualityComparer<TypeReference>
    {
        public override bool Equals(TypeReference x, TypeReference y) => x?.Type == y?.Type;

        public override int GetHashCode(TypeReference obj)
        {
            if (obj.Type != null)
                return obj.Type.GetHashCode();

            // Even if Type is null, return the GUID hashcode, because the Type will most likely be found based on GUID.
            return obj.GUID == string.Empty ? default : obj.GUID.GetHashCode();
        }
    }

    /// <summary>
    /// Custom comparer for TypeReference arrays to be able to use TypeReference arrays as a key in <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    public class TypeReferenceArrayComparer : IEqualityComparer<TypeReference[]>
    {
        private readonly TypeReferenceComparer _typeReferenceComparer = new TypeReferenceComparer();

        public bool Equals(TypeReference[] firstArray, TypeReference[] secondArray)
        {
            if (firstArray == null && secondArray == null)
                return true;

            if (firstArray == null || secondArray == null)
                return false;

            return firstArray.SequenceEqual(secondArray, _typeReferenceComparer);
        }

        public int GetHashCode(TypeReference[] array) =>
            ((IStructuralEquatable) array).GetHashCode(_typeReferenceComparer);
    }
}