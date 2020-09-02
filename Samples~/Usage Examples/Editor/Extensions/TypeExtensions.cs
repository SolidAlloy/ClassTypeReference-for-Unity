namespace TypeReferences.Demo.Editor
{
    using System;
    using System.Reflection;

    internal static class TypeExtensions
    {
        public static FieldInfo GetFieldAtPath(this Type parentType, string path)
        {
            FieldInfo field = null;
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            foreach (string part in path.Split('.'))
            {
                field = parentType.GetField(part, flags);
                if (field == null)
                    return null;

                parentType = field.FieldType;
            }

            return field;
        }
    }
}