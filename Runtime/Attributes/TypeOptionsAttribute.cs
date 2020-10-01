namespace TypeReferences
{
    using System;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Attribute for class selection constraints that can be applied when selecting
    /// a <see cref="TypeReference"/> with the Unity inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class TypeOptionsAttribute : PropertyAttribute
    {
        /// <summary>
        /// Gets or sets grouping of selectable types. Defaults to <see><cref>Grouping.ByNamespaceFlat</cref></see>
        /// unless explicitly specified.
        /// </summary>
        public Grouping Grouping = Grouping.ByNamespaceFlat;

        /// <summary>
        /// Removes (None) from the dropdown and disallows setting Type to null in Inspector.
        /// Note that the type can still be null by default or if set through code.
        /// Defaults to <c>false</c> unless explicitly specified.
        /// </summary>
        public bool ExcludeNone = false;

        /// <summary>Includes additional types in the drop-down list.</summary>
        public Type[] IncludeTypes;

        /// <summary>Excludes some of the types from the drop-down list.</summary>
        public Type[] ExcludeTypes;

        /// <summary>
        /// Adds types from additional assemblies to the drop-down list.
        /// By default, only types that can be accessed directly by the class are shown in the list.
        /// </summary>
        public string[] IncludeAdditionalAssemblies;

        /// <summary>Gets or sets the height of the dropdown. Default is zero.</summary>
        public int DropdownHeight = 0;

        /// <summary>
        /// If the dropdown renders a tree-view, then setting this to true will ensure everything is expanded by default.
        /// </summary>
        public bool ExpandAllFolders = false;

        /// <summary>
        /// Sets the minimum number of items in the drop-down for the search bar to appear. Defaults to 10.
        /// </summary>
        public int SearchbarMinItemsCount = 10;

        /// <summary>
        /// Makes the field show the short name of the selected type instead of the full one. False by default.
        /// </summary>
        public bool ShortName = false;

        /// <summary>
        /// If the dropdown shows built-in types, it will show them by their keyword names (int) instead of full names
        /// (System.Int32).
        /// </summary>
        public bool UseBuiltInTypeNames = true;

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> matches requirements set in the attribute.
        /// </summary>
        /// <param name="type">Type to test.</param>
        /// <returns>
        /// A <see cref="bool"/> value indicating if the type specified by <paramref name="type"/>
        /// matches the requirements and should thus be selectable.
        /// </returns>
        public virtual bool MatchesRequirements(Type type)
        {
            if (ExcludeTypes == null)
                return true;

            return ! ExcludeTypes.Contains(type);
        }
    }
}