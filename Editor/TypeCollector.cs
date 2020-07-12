namespace TypeReferences.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class TypeCollector
    {
        public static IEnumerable<Assembly> GetTypeRelatedAssemblies(Type type)
        {
            var declaringTypeAssembly = type.Assembly;
            var assemblies = new List<Assembly> { declaringTypeAssembly };

            assemblies.AddRange(
                declaringTypeAssembly.GetReferencedAssemblies()
                    .Select(Assembly.Load));

            return assemblies;
        }

        public static List<Type> GetFilteredTypesFromAssemblies(
            IEnumerable<Assembly> assemblies,
            ClassTypeConstraintAttribute filter)
        {
            var types = new List<Type>();

            foreach (var assembly in assemblies)
                types.AddRange(GetFilteredTypesInAssembly(assembly, filter));

            return types;
        }

        private static IEnumerable<Type> GetFilteredTypesInAssembly(
            Assembly assembly,
            ClassTypeConstraintAttribute filter)
        {
            return from type in assembly.GetTypes()
                where type.IsVisible && type.IsClass
                where FilterConstraintIsSatisfied(filter, type)
                select type;
        }

        private static bool FilterConstraintIsSatisfied(ClassTypeConstraintAttribute filter, Type type)
        {
            if (filter == null)
                return true;

            return filter.IsConstraintSatisfied(type);
        }
    }
}