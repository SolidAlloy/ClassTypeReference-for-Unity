namespace TypeReferences.Deprecated.Editor.Deprecated
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using TypeReferences.Deprecated;
    using UnityEngine;

    /// <summary>
    /// A class responsible for collecting class types according to given filters and conditions.
    /// </summary>
    public static class TypeCollector
    {
        public static List<Assembly> GetAssembliesTypeHasAccessTo(Type type)
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

        public static List<Type> GetVisibleTypesFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            var types = new List<Type>();

            foreach (var assembly in assemblies)
            {
                types.AddRange(GetVisibleTypesFromAssembly(assembly));
            }

            return types;
        }

        public static Assembly TryLoadAssembly(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
                return null;

            Assembly assembly = null;

            try
            {
                assembly = Assembly.Load(assemblyName);
            }
            catch (FileNotFoundException)
            {
                Debug.LogError($"{assemblyName} was not found. It will not be added to dropdown.");
            }
            catch (FileLoadException)
            {
                Debug.LogError($"Failed to load {assemblyName}. It will not be added to dropdown.");
            }
            catch (BadImageFormatException)
            {
                Debug.LogError($"{assemblyName} is not a valid assembly. It will not be added to dropdown.");
            }

            return assembly;
        }

        private static IEnumerable<Type> GetFilteredTypesFromAssembly(
            Assembly assembly,
            ClassTypeConstraintAttribute filter)
        {
            return from type in GetVisibleTypesFromAssembly(assembly)
                where type.IsVisible && type.IsClass
                where FilterConstraintIsSatisfied(filter, type)
                select type;
        }

        private static IEnumerable<Type> GetVisibleTypesFromAssembly(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes().Where(type => type.IsVisible);
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