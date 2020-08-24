namespace TypeReferences.Deprecated.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Deprecated;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Draws expanded drop-down list of class types.
    /// </summary>
    internal class TypeDropDownDrawer
    {
        private readonly Type _selectedType;
        private readonly ClassTypeConstraintAttribute _constraints;
        private readonly Type _declaringType;
        private GenericMenu _menu;

        public TypeDropDownDrawer(string typeName, ClassTypeConstraintAttribute constraints, Type declaringType)
        {
            _selectedType = CachedTypeReference.GetType(typeName);
            _constraints = constraints;
            _declaringType = declaringType;
        }

        public void Draw(Rect position)
        {
            _menu = new GenericMenu();

            AddNoneElementIfNotExcluded();

            var classGrouping = _constraints?.Grouping ?? ClassTypeConstraintAttribute.DefaultGrouping;
            AddTypes(classGrouping);

            _menu.DropDown(position);
        }

        private void AddNoneElementIfNotExcluded()
        {
            bool excludeNone = _constraints?.ExcludeNone ?? false;
            if (excludeNone)
                return;

            _menu.AddItem(
                new GUIContent(ClassTypeReference.NoneElement),
                _selectedType == null,
                CachedTypeReference.SelectedTypeName,
                null);

            _menu.AddSeparator(string.Empty);
        }

        private void AddTypes(ClassGrouping classGrouping)
        {
            var types = GetFilteredTypes();

            AddIncludedTypes(types);
            RemoveExcludedTypes(types);

            foreach (var nameTypePair in types)
            {
                string menuLabel = TypeNameFormatter.Format(nameTypePair.Value, classGrouping);
                AddLabelIfNotEmpty(menuLabel, nameTypePair.Value);
            }
        }

        private SortedList<string, Type> GetFilteredTypes()
        {
            var typeRelatedAssemblies = TypeCollector.GetAssembliesTypeHasAccessTo(_declaringType);

            if (_constraints.IncludeAdditionalAssemblies != null)
                IncludeAdditionalAssemblies(typeRelatedAssemblies);

            var filteredTypes = TypeCollector.GetFilteredTypesFromAssemblies(
                typeRelatedAssemblies,
                _constraints);

            var sortedTypes = new SortedList<string, Type>(filteredTypes.ToDictionary(type => type.FullName));

            return sortedTypes;
        }

        private void AddIncludedTypes(IDictionary<string, Type> types)
        {
            var typesToInclude = _constraints?.IncludeTypes;
            if (typesToInclude == null)
                return;

            foreach (var typeToInclude in _constraints?.IncludeTypes)
            {
                if (typeToInclude != null)
                    types.Add(typeToInclude.FullName ?? string.Empty, typeToInclude);
            }
        }

        private void RemoveExcludedTypes(IDictionary<string, Type> types)
        {
            var typesToExclude = _constraints?.ExcludeTypes;
            if (typesToExclude == null)
                return;

            foreach (var typeToExclude in _constraints?.ExcludeTypes)
            {
                if (typeToExclude == null || string.IsNullOrEmpty(typeToExclude.FullName))
                    continue;

                types.Remove(typeToExclude.FullName);
            }
        }

        private void AddLabelIfNotEmpty(string menuLabel, Type type)
        {
            if (string.IsNullOrEmpty(menuLabel))
                return;

            var content = new GUIContent(menuLabel);
            _menu.AddItem(content, _selectedType == type, CachedTypeReference.SelectedTypeName, type);
        }

        private void IncludeAdditionalAssemblies(ICollection<Assembly> typeRelatedAssemblies)
        {
            foreach (string assemblyName in _constraints.IncludeAdditionalAssemblies)
            {
                var additionalAssembly = TypeCollector.TryLoadAssembly(assemblyName);
                if (additionalAssembly == null)
                    continue;

                if ( ! typeRelatedAssemblies.Contains(additionalAssembly))
                    typeRelatedAssemblies.Add(additionalAssembly);
            }
        }
    }
}