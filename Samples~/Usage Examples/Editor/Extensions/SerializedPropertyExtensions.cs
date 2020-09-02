namespace TypeReferences.Demo.Editor
{
    using UnityEditor;

    internal static class SerializedPropertyExtensions
    {
        public static bool IsBuiltIn(this SerializedProperty property)
        {
            if (property.name == "size" || property.name == "Array")
                return true;

            string firstTwoChars = property.name.Substring(0, 2);
            return firstTwoChars == "m_";
        }
    }
}