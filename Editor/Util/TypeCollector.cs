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

            var referencedAssemblies = typeAssembly.GetReferencedAssemblies();
            var assemblies = new List<Assembly>(referencedAssemblies.Length + 1);

            for (int i = 0; i < referencedAssemblies.Length; i++)
            {
                assemblies[i] = Assembly.Load(referencedAssemblies[i]);
            }

            assemblies[referencedAssemblies.Length] = typeAssembly;

            return assemblies;
        }

        public static List<Assembly> GetAssembliesTypeHasAccessTo(Type type, out bool containsMSCorLib)
        {
            if (type == null)
            {
                containsMSCorLib = true;
                return GetAllAssemblies();
            }

            containsMSCorLib = false;
            Assembly typeAssembly = type.Assembly;

            var referencedAssemblies = typeAssembly.GetReferencedAssemblies();
            var assemblies = new List<Assembly>(referencedAssemblies.Length + 1);

            for (int i = 0; i < referencedAssemblies.Length; i++)
            {
                var assemblyName = referencedAssemblies[i];

                if ( ! containsMSCorLib && assemblyName.Name == "mscorlib")
                {
                    containsMSCorLib = true;
                }

                assemblies.Add(Assembly.Load(assemblyName));
            }

            if ( ! containsMSCorLib && typeAssembly.FullName.Contains("mscorlib"))
            {
                containsMSCorLib = true;
            }

            assemblies.Add(typeAssembly);

            return assemblies;
        }

        private static List<Assembly> GetAllAssemblies()
        {
            return new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static List<Type> GetFilteredTypesFromAssemblies(
            List<Assembly> assemblies,
            TypeOptionsAttribute filter)
        {
            int assembliesCount = assemblies.Count;

            var types = new List<Type>(assembliesCount * 20);

            for (int i = 0; i < assembliesCount; i++)
            {
                var filteredTypes = GetFilteredTypesFromAssembly(assemblies[i], filter);
                int filteredTypesCount = filteredTypes.Count;

                for (int j = 0; j < filteredTypesCount; j++)
                {
                    types.Add(filteredTypes[j]);
                }
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

        private static List<Type> GetFilteredTypesFromAssembly(Assembly assembly, TypeOptionsAttribute filter)
        {
            var visibleTypes = GetVisibleTypesFromAssembly(assembly);
            var filteredTypes = new List<Type>();
            int visibleTypesCount = visibleTypes.Count;

            for (int i = 0; i < visibleTypesCount; i++)
            {
                Type type = visibleTypes[i];

                if (FilterConstraintIsSatisfied(filter, type))
                {
                    filteredTypes.Add(type);
                }
            }

            return filteredTypes;
        }

        private static List<Type> GetVisibleTypesFromAssembly(Assembly assembly)
        {
            try
            {
                var assemblyTypes = assembly.GetTypes();

                var visibleTypes = new List<Type>(assemblyTypes.Length);

                for (int i = 0; i < assemblyTypes.Length; i++)
                {
                    Type type = assemblyTypes[i];

                    if (type.IsVisible)
                        visibleTypes.Add(type);
                }

                return visibleTypes;
            }
            catch (ReflectionTypeLoadException e)
            {
                Debug.LogError($"Types could not be extracted from assembly {assembly}: {e.Message}");
                return new List<Type>(0);
            }
        }

        private static bool FilterConstraintIsSatisfied(TypeOptionsAttribute filter, Type type)
        {
            return filter == null || filter.MatchesRequirements(type);
        }
    }
}