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
        private static readonly HashSet<string> _systemAssemblyNames = new HashSet<string> { "mscorlib", "System", "System.Core" };

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
        public static IEnumerable<Assembly> GetAssembliesTypeHasAccessTo(Type type, out bool containsSystemAssembly)
        {
            if (type == null)
            {
                containsSystemAssembly = true;
                return GetAllAssemblies();
            }

            containsSystemAssembly = false;
            Assembly typeAssembly = type.Assembly;

            var referencedAssemblies = typeAssembly.GetReferencedAssemblies();

            containsSystemAssembly = IsSystemAssembly(typeAssembly) || referencedAssemblies.Any(IsSystemAssembly);
            return referencedAssemblies.Select(Assembly.Load).Append(typeAssembly);
        }

        private static bool IsSystemAssembly(Assembly assembly)
        {
            return IsSystemAssembly(assembly.GetName());
        }
        
        private static bool IsSystemAssembly(AssemblyName assemblyName)
        {
            return _systemAssemblyNames.Contains(assemblyName.Name);
        }

        public static IEnumerable<Assembly> GetAllAssemblies() => AppDomain.CurrentDomain.GetAssemblies();

        public static IEnumerable<Type> GetFilteredTypesFromAssemblies(IEnumerable<Assembly> assemblies, TypeOptionsAttribute filter)
        {
            return assemblies.SelectMany(assembly => GetFilteredTypesFromAssembly(assembly, filter));
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
        {
            var assemblyTypes = GetAllTypesFromAssembly(assembly);

            // Don't allow internal types of mscorlib even if the AllowInternal property is set to true because it leads
            // to a lot of garbage in the dropdown while it's almost impossible users will need a System internal class.
            bool allowInternal = filter.AllowInternal && !IsSystemAssembly(assembly);

            return assemblyTypes.Where(type => (allowInternal || type.IsVisible) && FilterConstraintIsSatisfied(filter, type));
        }

        private static IEnumerable<Type> GetAllTypesFromAssembly(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                Debug.LogError($"Types could not be extracted from assembly {assembly}: {e.Message}");
                return Enumerable.Empty<Type>();
            }
        }

        private static bool FilterConstraintIsSatisfied(TypeOptionsAttribute filter, Type type)
        {
            string typeFullName = type.FullName;
            
            if (typeFullName == null || typeFullName.StartsWith("<") || type.Name.StartsWith("<"))
                return false;

            return filter == null || filter.MatchesRequirements(type);
        }
    }
}