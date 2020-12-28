namespace TypeReferences.Editor.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using TypeReferences;
    using UnityEngine;

    /// <summary>
    /// A class responsible for collecting types according to given filters and conditions.
    /// </summary>
    internal static class TypeCollector
    {
        /// <summary>
        /// Collects assemblies the type has access to: the type's native assembly and all its referenced assemblies.
        /// </summary>
        /// <param name="type">Type to collect the assemblies for.</param>
        /// <returns>Collection of assemblies the type has access to.</returns>
        /// <exception cref="FileNotFoundException">
        /// If the method tried to load the Assembly-Csharp assembly but it does not exist.
        /// </exception>
        /// <remarks>
        /// Additional assemblies may be added using <see cref="TypeOptionsAttribute.IncludeAdditionalAssemblies"/>,
        /// hence the List return type.
        /// </remarks>
        public static List<Assembly> GetAssembliesTypeHasAccessTo(Type type)
        {
            Assembly typeAssembly;

            try
            {
                typeAssembly = type == null ? Assembly.Load("Assembly-CSharp") : type.Assembly;
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Assembly-CSharp.dll was not found. Please create any " +
                                                "script in the Assets folder so that the assembly is generated.");
            }

            return typeAssembly.GetReferencedAssemblies()
                .Select(Assembly.Load)
                .Append(typeAssembly)
                .ToList();
        }

        public static List<Type> GetFilteredTypesFromAssemblies(
            IEnumerable<Assembly> assemblies,
            TypeOptionsAttribute filter)
        {
            var types = new List<Type>();

            foreach (var assembly in assemblies)
                types.AddRange(GetFilteredTypesFromAssembly(assembly, filter));

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

        private static IEnumerable<Type> GetFilteredTypesFromAssembly(Assembly assembly, TypeOptionsAttribute filter)
            => GetVisibleTypesFromAssembly(assembly).Where(type => FilterConstraintIsSatisfied(filter, type));

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

        private static bool FilterConstraintIsSatisfied(TypeOptionsAttribute filter, Type type)
        {
            return filter == null || filter.MatchesRequirements(type);
        }
    }
}