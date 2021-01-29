namespace TypeReferences
{
    using System;
    using System.Linq;
    using JetBrains.Annotations;
    using SolidUtilities.Extensions;

    /// <summary>
    /// Constraint that allows selection of types that inherit a specific parent type or interface when
    /// selecting a <see cref="TypeReference"/> with the Unity inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class InheritsAttribute : TypeOptionsAttribute
    {
        public Type[] BaseTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritsAttribute"/> class using one base type and optional
        /// additional base types.
        /// </summary>
        /// <param name="baseType">Type that selectable types must inherit from.</param>
        /// <param name="additionalBaseTypes">
        /// Additional types the selectable types must inherit from (e.g. multiple interfaces).
        /// </param>
        [PublicAPI]
        public InheritsAttribute(Type baseType, [CanBeNull] params Type[] additionalBaseTypes)
        {
            if (additionalBaseTypes == null || additionalBaseTypes.Length == 0)
            {
                BaseTypes = new[] { baseType };
            }
            else
            {
                BaseTypes = new Type[additionalBaseTypes.Length+1];
                additionalBaseTypes.CopyTo(BaseTypes, 0);
                BaseTypes[additionalBaseTypes.Length] = baseType;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritsAttribute"/> class using multiple base types.
        /// </summary>
        /// <param name="baseTypes">
        /// Types the selectable types must inherit from (e.g. parent class and additional interfaces).
        /// </param>
        [PublicAPI]
        public InheritsAttribute(Type[] baseTypes)
        {
            BaseTypes = baseTypes;
        }

        /// <summary>
        /// Allows to choose the base type from the drop-down as well.
        /// Defaults to a value of <c>false</c> unless explicitly specified.
        /// </summary>
        [PublicAPI]
        public bool IncludeBaseType { get; set; }

        /// <summary>
        /// Allows abstract classes and interfaces to be selected from drop-down.
        /// Defaults to a value of <c>false</c> unless explicitly specified.
        /// </summary>
        [PublicAPI]
        public bool AllowAbstract { get; set; }

        /// <inheritdoc/>
        internal override bool MatchesRequirements(Type type)
        {
            if (BaseTypes.Contains(type) && !IncludeBaseType)
            {
                return false;
            }

            // Include base type in the drop-down even if it is abstract.
            // If the user set IncludeBaseType to true, they probably want to include the base type in the dropdown
            // even though it is abstract.
            if (BaseTypes.Contains(type))
                return true;

            bool passesAbstractConstraint = AllowAbstract || !type.IsAbstract;

            return BaseTypes.All(type.InheritsFrom) && passesAbstractConstraint && base.MatchesRequirements(type);
        }
    }
}