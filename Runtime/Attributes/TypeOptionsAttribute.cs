namespace TypeReferences
{
    using System;
    using System.Linq;
    using JetBrains.Annotations;
    using SolidUtilities.Extensions;
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
        [PublicAPI] public Grouping Grouping = Grouping.ByNamespaceFlat;

        /// <summary>
        /// Removes (None) from the dropdown and disallows setting Type to null in Inspector.
        /// Note that the type can still be null by default or if set through code.
        /// Defaults to <see langword="false"/> unless explicitly specified.
        /// </summary>
        [PublicAPI] public bool ExcludeNone;

        /// <summary>Includes additional types in the drop-down list.</summary>
        [PublicAPI] public Type[] IncludeTypes;

        /// <summary>Excludes some of the types from the drop-down list.</summary>
        [PublicAPI] public Type[] ExcludeTypes;

        /// <summary>
        /// Adds types from additional assemblies to the drop-down list.
        /// By default, only types that can be accessed directly by the class are shown in the list.
        /// </summary>
        [PublicAPI] public string[] IncludeAdditionalAssemblies;

        /// <summary>Gets or sets the height of the dropdown. Default is zero.</summary>
        [PublicAPI] public int DropdownHeight;

        /// <summary>
        /// If the dropdown renders a tree-view, then setting this to <see langword="true"/> will ensure everything
        /// is expanded by default.
        /// </summary>
        [PublicAPI] public bool ExpandAllFolders;

        /// <summary>
        /// Sets the minimum number of items in the drop-down for the search bar to appear. Defaults to 10.
        /// </summary>
        [PublicAPI] public int SearchbarMinItemsCount = 10;

        /// <summary>
        /// Makes the field show the short name of the selected type instead of the full one.
        /// <see langword="false"/> by default.
        /// </summary>
        [PublicAPI] public bool ShortName;

        /// <summary>
        /// Whether to make dropdown show built-in types by their keyword name (int) instead of the full name
        /// (System.Int32). Defaults to <see langword="true"/>.
        /// </summary>
        [PublicAPI] public bool UseBuiltInNames = true;

        /// <summary>
        /// If enabled, shows only types that can be serialized by Unity. Defaults to <see langword="false"/>.
        /// </summary>
        [PublicAPI] public bool SerializableOnly;

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> matches requirements set in the attribute.
        /// </summary>
        /// <param name="type">Type to test.</param>
        /// <returns>
        /// A <see cref="bool"/> value indicating if the type specified by <paramref name="type"/>
        /// matches the requirements and should thus be selectable.
        /// </returns>
        internal virtual bool MatchesRequirements(Type type)
        {
            bool passesExcludedFilter = ! ExcludeTypes?.Contains(type) ?? true;
            bool passesSerializableFilter = ! SerializableOnly || type.IsUnitySerializable();
            return passesExcludedFilter && passesSerializableFilter;
        }
    }
}