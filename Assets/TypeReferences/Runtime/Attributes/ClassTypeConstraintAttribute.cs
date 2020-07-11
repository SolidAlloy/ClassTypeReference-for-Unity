// Copyright ClassTypeReference Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

namespace TypeReferences
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Base class for class selection constraints that can be applied when selecting
    /// a <see cref="ClassTypeReference"/> with the Unity inspector.
    /// </summary>
    public class ClassTypeConstraintAttribute : PropertyAttribute
    {
        /// <summary>
        /// Gets or sets grouping of selectable classes. Defaults to <see cref="ClassGrouping.ByNamespaceFlat"/>
        /// unless explicitly specified.
        /// </summary>
        public ClassGrouping Grouping { get; set; } = ClassGrouping.ByNamespaceFlat;

        /// <summary>
        /// Gets or sets whether abstract classes can be selected from drop-down.
        /// Defaults to a value of <c>false</c> unless explicitly specified.
        /// </summary>
        public bool AllowAbstract { get; set; } = false;

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> satisfies filter constraint.
        /// </summary>
        /// <param name="type">Type to test.</param>
        /// <returns>
        /// A <see cref="bool"/> value indicating if the type specified by <paramref name="type"/>
        /// satisfies this constraint and should thus be selectable.
        /// </returns>
        public virtual bool IsConstraintSatisfied(Type type)
        {
            return AllowAbstract || !type.IsAbstract;
        }
    }
}
