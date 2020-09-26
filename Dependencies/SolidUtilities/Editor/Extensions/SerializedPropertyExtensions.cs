namespace SolidUtilities.Editor.Extensions
{
    using UnityEditor;

    /// <summary>Different useful extensions for <see cref="SerializedProperty"/>.</summary>
    public static class SerializedPropertyExtensions
    {
        /// <summary>
        /// Checks whether the serialized property is built-in. <see cref="SerializedObject"/> has a lot of built-in
        /// properties and we are often interested only in the custom ones.
        /// </summary>
        /// <param name="property">The property to check.</param>
        /// <returns>Whether the property is built-in.</returns>
        public static bool IsBuiltIn(this SerializedProperty property)
        {
            if (property.name == "size" || property.name == "Array")
                return true;

            string firstTwoChars = property.name.Substring(0, 2);
            return firstTwoChars == "m_";
        }
    }
}