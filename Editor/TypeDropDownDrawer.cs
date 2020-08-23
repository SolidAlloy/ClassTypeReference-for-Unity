namespace TypeReferences.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TypeReferences;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Draws expanded drop-down list of types.
    /// </summary>
    internal class TypeDropDownDrawer
    {
        private readonly Type _selectedType;
        private readonly TypeOptionsAttribute _constraints;
        private readonly Type _declaringType;
        private GenericMenu _menu;

        public TypeDropDownDrawer(string typeName, TypeOptionsAttribute constraints, Type declaringType)
        {
            _selectedType = CachedTypeReference.GetType(typeName);
            _constraints = constraints;
            _declaringType = declaringType;
        }

        public void Draw(Rect position)
        {
            _menu = new GenericMenu();

            AddNoneElementIfNotExcluded();

            var grouping = _constraints?.Grouping ?? TypeOptionsAttribute.DefaultGrouping;
            AddTypesToMenu(grouping);

            _menu.DropDown(position);
        }

        private void AddNoneElementIfNotExcluded()
        {
            bool excludeNone = _constraints?.ExcludeNone ?? false;
            if (excludeNone)
                return;

            _menu.AddItem(
                new GUIContent(TypeReference.NoneElement),
                _selectedType == null,
                CachedTypeReference.SelectedTypeName,
                null);

            _menu.AddSeparator(string.Empty);
        }

        private void AddTypesToMenu(Grouping typeGrouping)
        {
            var types = GetFilteredTypes();

            AddIncludedTypes(types);

            foreach (var nameTypePair in types)
            {
                string menuLabel = TypeNameFormatter.Format(nameTypePair.Value, typeGrouping);
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