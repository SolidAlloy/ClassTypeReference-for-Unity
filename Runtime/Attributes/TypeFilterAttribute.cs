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
    public class TypeFilterAttribute : PropertyAttribute
    {
        /// <summary>
        /// Default grouping of selectable types.
        /// </summary>
        public const Grouping DefaultGrouping = Grouping.ByNamespaceFlat;

        /// <summary>
        /// Gets or sets grouping of selectable types. Defaults to <see cref="Grouping.ByNamespaceFlat"/>
        /// unless explicitly specified.
        /// </summary>
        public Grouping Grouping { get; set; } = Grouping.ByNamespaceFlat;

        /// <summary>
        /// Removes (None) from the dropdown and disallows setting Type to null in Inspector.
        /// Note that the type can still be null by default or if set through code.
        /// Defaults to <c>false</c> unless explicitly specified.
        /// </summary>
        public bool ExcludeNone { get; set; } = false;

        /// <summary>
        /// Includes additional types in the drop-down list.
        /// </summary>
        public Type[] IncludeTypes { get; set; }

        /// <summary>
        /// Excludes some of the types from the drop-down list.
        /// </summary>
        public Type[] ExcludeTypes { get; set; }

        /// <summary>
        /// Adds types from additional assemblies to the drop-down list.
        /// By default, only types that can be accessed directly by the class are shown in the list.
        /// </summary>
        public string[] IncludeAdditionalAssemblies { get; set; }

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
            return ! ExcludeTypes.Contains(type);
        }
    }
}