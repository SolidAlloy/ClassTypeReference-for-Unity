namespace TypeReferences.Editor.Drawers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using SolidUtilities.Helpers;
    using TypeDropdown;
    using UnityEngine;
    using Util;

    /// <summary>
    /// This class gathers needed types from assemblies based on the attribute options, and creates a popup window with
    /// the collected types.
    /// </summary>
    internal readonly struct TypeDropdownDrawer
    {
        private static readonly List<TypeItem> _emptyList = new List<TypeItem>(0);

        private readonly TypeOptionsAttribute _attribute;
        private readonly Type _declaringType;
        private readonly Type _selectedType;

        public TypeDropdownDrawer(Type selectedType, TypeOptionsAttribute attribute, Type declaringType)
        {
            _attribute = attribute;
            _declaringType = declaringType;
            _selectedType = selectedType;
        }

        public void Draw(Action<Type> onTypeSelected)
        {
            var dropdownItems = GetDropdownItems();
            var selectionTree = new SelectionTree(dropdownItems, _selectedType, onTypeSelected, _attribute.SearchbarMinItemsCount, _attribute.ExcludeNone);

            if (_attribute.ExpandAllFolders)
                selectionTree.ExpandAllFolders();

            Vector2 dropdownPosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            DropdownWindow.Create(selectionTree, _attribute.DropdownHeight, dropdownPosition);
        }

        public SortedSet<TypeItem> GetDropdownItems()
        {
            var types = GetFilteredTypes();

            foreach (var typeItem in GetIncludedTypes())
                types.Add(typeItem);

            return types;
        }

        private List<TypeItem> GetIncludedTypes()
        {
            var typesToInclude = _attribute.IncludeTypes;

            if (typesToInclude == null)
                return _emptyList;

            var typeItems = new List<TypeItem>(typesToInclude.Length);

            foreach (Type typeToInclude in _attribute.IncludeTypes)
            {
                if (typeToInclude == null || typeToInclude.FullName == null)
                    continue;

                typeItems.Add(new TypeItem(typeToInclude, _attribute.Grouping));
            }

            return typeItems;
        }

        private SortedSet<TypeItem> GetFilteredTypes()
        {
            var typeRelatedAssemblies = TypeCollector.GetAssembliesTypeHasAccessTo(_declaringType);

            if (_attribute.IncludeAdditionalAssemblies != null)
                IncludeAdditionalAssemblies(typeRelatedAssemblies);

            var filteredTypes = TypeCollector.GetFilteredTypesFromAssemblies(
                typeRelatedAssemblies,
                _attribute);

            bool replaceBuiltInNames = _attribute.UseBuiltInNames && typeRelatedAssemblies
                .Any(assembly => assembly.FullName.Contains("mscorlib"));

            var sortedTypes = new SortedSet<TypeItem>(new TypeItemComparer());

            int filteredTypesLength = filteredTypes.Count;

            for (int i = 0; i < filteredTypesLength; i++)
            {
                var type = filteredTypes[i];
                string fullTypeName = type.FullName;
                if (fullTypeName == null)
                    continue;

                if (replaceBuiltInNames)
                    fullTypeName = fullTypeName.ReplaceWithBuiltInName(true);

                sortedTypes.Add(new TypeItem(type, fullTypeName, _attribute.Grouping));
            }

            return sortedTypes;
        }

        private void IncludeAdditionalAssemblies(List<Assembly> typeRelatedAssemblies)
        {
            foreach (string assemblyName in _attribute.IncludeAdditionalAssemblies)
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