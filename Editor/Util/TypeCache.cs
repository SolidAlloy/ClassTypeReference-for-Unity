namespace TypeReferences.Editor.Util
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A class that improves performance by avoiding a large number of <see cref="M:Type.GetType"/> calls.
    /// </summary>
    internal static class TypeCache
    {
        private static readonly Dictionary<string, Type> _typeDict = new Dictionary<string, Type>();

        /// <summary>
        /// Get type from TypeCache if it is cached.
        /// Otherwise, find the type, cache it, and return it to the caller.
        /// </summary>
        /// <param name="typeNameAndAssembly">Type name, followed by a comma and assembly name.</param>
        /// <returns>Cached class type.</returns>
        public static Type GetType(string typeNameAndAssembly)
        {
            if (_typeDict.TryGetValue(typeNameAndAssembly, out Type type))
                return type;

            type = string.IsNullOrEmpty(typeNameAndAssembly) ? null : Type.GetType(typeNameAndAssembly);
            _typeDict[typeNameAndAssembly] = type;
            return type;
        }
    }
}