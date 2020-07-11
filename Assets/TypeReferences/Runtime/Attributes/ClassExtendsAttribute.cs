namespace TypeReferences
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
        public ClassExtendsAttribute() { }

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

        /// <inheritdoc/>
        public override bool IsConstraintSatisfied(Type type)
        {
            return base.IsConstraintSatisfied(type)
                   && BaseType.IsAssignableFrom(type) && type != BaseType;
        }
    }
}