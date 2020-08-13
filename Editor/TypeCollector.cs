namespace TypeReferences.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;

    /// <summary>
    /// A class responsible for collecting class types according to given filters and conditions.
    /// </summary>
    internal static class TypeCollector
    {
        public static IEnumerable<Assembly> GetAssembliesTypeHasAccessTo(Type type)
        {
            var typeAssembly = type == null ? Assembly.Load("Assembly-CSharp") : type.Assembly;
            var assemblies = new List<Assembly> { typeAssembly };

            assemblies.AddRange(
                typeAssembly.GetReferencedAssemblies()
                    .Select(Assembly.Load));

            return assemblies;
        }

        public static List<Type> GetFilteredTypesFromAssemblies(
            IEnumerable<Assembly> assemblies,
            ClassTypeConstraintAttribute filter)
        {
            var types = new List<Type>();

            foreach (var assembly in assemblies)
                types.AddRange(GetFilteredTypesFromAssembly(assembly, filter));

            return types;
        }

        private static IEnumerable<Type> GetFilteredTypesFromAssembly(
            Assembly assembly,
            ClassTypeConstraintAttribute filter)
        {
            return from type in GetTypesFromAssembly(assembly)
                where type.IsVisible && type.IsClass
                where FilterConstraintIsSatisfied(filter, type)
                select type;
        }

        private static Type[] GetTypesFromAssembly(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                Debug.LogError($"Types could not be extracted from assembly {assembly}: {e.Message}");
                return new Type[0];
            }
        }

        private static bool FilterConstraintIsSatisfied(ClassTypeConstraintAttribute filter, Type type)
        {
            if (filter == null)
                return true;

            return filter.IsConstraintSatisfied(type);
        }
    }
}