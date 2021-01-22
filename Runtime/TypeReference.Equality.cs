namespace TypeReferences
{
    using System;

    public partial class TypeReference : IEquatable<TypeReference>
    {
        public bool Equals(TypeReference p)
        {
            if (ReferenceEquals(p, null))
            {
                return false;
            }

            if (ReferenceEquals(this, p))
            {
                return true;
            }

            if (this.GUID?.Length != 0 && p.GUID?.Length != 0 && (this.Type == null || p.Type == null))
                return this.GUID == p.GUID;

            return this.Type == p.Type;
        }

        public static bool operator ==(TypeReference lhs, TypeReference rhs)
        {
            return lhs?.Equals(rhs) ?? ReferenceEquals(rhs, null);
        }

        public static bool operator !=(TypeReference lhs, TypeReference rhs)
        {
            return ! (lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TypeReference);
        }

        public override int GetHashCode()
        {
            return _type == null ? 0 : _type.GetHashCode();
        }
    }
}