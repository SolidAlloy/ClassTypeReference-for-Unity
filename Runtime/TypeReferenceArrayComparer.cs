namespace TypeReferences
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Custom comparer for TypeReference arrays to be able to use TypeReference arrays as a key in <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    public class TypeReferenceArrayComparer : IEqualityComparer<TypeReference[]>
    {
        public bool Equals(TypeReference[] firstArray, TypeReference[] secondArray)
        {
            if (firstArray == null && secondArray == null)
                return true;

            if (firstArray == null || secondArray == null)
                return false;

            return firstArray.SequenceEqual(secondArray);
        }

        public int GetHashCode(TypeReference[] array)
        {
            int hashCode = 0;

            foreach (TypeReference item in array)
                hashCode ^= item.GetHashCode();

            return hashCode;
        }
    }
}