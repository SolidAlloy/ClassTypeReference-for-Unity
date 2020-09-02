namespace TypeReferences.Deprecated
{
    using System;

    /// <summary>
    /// Constraint that allows selection of classes that extend a specific class when
    /// selecting a <see cref="ClassTypeReference"/> with the Unity inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ClassExtendsAttribute : ClassTypeConstraintAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassExtendsAttribute"/> class.
        /// </summary>
        /// <param name="baseType">Type of class that selectable classes must derive from.</param>
        public ClassExtendsAttribute(Type baseType)
        {
            BaseType = baseType;
        }

        /// <summary>
        /// Gets the type of class that selectable classes must derive from.
        /// </summary>
        public Type BaseType { get; private set; }

        /// <summary>
        /// Allows to choose the base type from the drop-down as well.
        /// </summary>
        public bool IncludeBaseType { get; set; }

        /// <inheritdoc/>
        public override bool IsConstraintSatisfied(Type type)
        {
            if (type == BaseType && ! IncludeBaseType)
                return false;

            // Include base type in the drop-down even if it is abstract.
            // If the user set IncludeBaseType to true, they probably want to include the base type in the dropdown
            // even though it is abstract.
            if (type == BaseType)
                return true;

            return BaseType.IsAssignableFrom(type) && base.IsConstraintSatisfied(type);
        }
    }
}