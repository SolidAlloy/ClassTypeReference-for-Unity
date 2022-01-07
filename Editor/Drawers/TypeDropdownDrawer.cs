namespace TypeReferences.Editor.Drawers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using SolidUtilities;
    using UnityDropdown.Editor;
    using UnityEngine;
    using UnityEngine.Assertions;
    using Util;

    /// <summary>
    /// This class gathers needed types from assemblies based on the attribute options, and creates a popup window with
    /// the collected types.
    /// </summary>
    internal readonly struct TypeDropdownDrawer
    {
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
            var selectionTree = new DropdownTree<Type>(dropdownItems, _selectedType, onTypeSelected, ProjectSettings.SearchbarMinItemsCount, true, _attribute.ExcludeNone);

            if (_attribute.ExpandAllFolders)
                selectionTree.ExpandAllFolders();

            DropdownWindow.Create(selectionTree, DropdownWindowType.Context, windowHeight: _attribute.DropdownHeight);
        }

        public DropdownItem<Type>[] GetDropdownItems()
        {
            var filteredTypes = GetFilteredTypes();
            var includedTypes = GetIncludedTypes();

            return includedTypes.Length == 0 ? filteredTypes : MergeArrays(filteredTypes, includedTypes);
        }

        private DropdownItem<Type>[] MergeArrays(DropdownItem[] filteredTypes, DropdownItem[] includedTypes)
        {
            var totalTypes = new DropdownItem<Type>[filteredTypes.Length + includedTypes.Length];
            filteredTypes.CopyTo(totalTypes, 0);
            includedTypes.CopyTo(totalTypes, filteredTypes.Length);
            return totalTypes;
        }

        private DropdownItem<Type>[] GetIncludedTypes()
        {
            if (_attribute.IncludeTypes == null)
                return Array.Empty<DropdownItem<Type>>();

            var typeItems = new DropdownItem<Type>[_attribute.IncludeTypes.Length];

            for (int i = 0; i < _attribute.IncludeTypes.Length; i++)
            {
                Type type = _attribute.IncludeTypes[i];

                if (type != null)
                {
                    typeItems[i] = CreateItem(type, _attribute.Grouping);
                }
                else
                {
                    throw new ArgumentException("IncludeTypes must not contain null.");
                }
            }

            return typeItems;
        }

        private DropdownItem<Type> CreateItem(Type type, Grouping grouping, string searchName = null)
        {
            searchName ??= type.FullName ?? string.Empty;
            return new DropdownItem<Type>(type, TypeNameFormatter.Format(type, searchName, grouping), searchName: searchName);
        }

        private DropdownItem<Type>[] GetFilteredTypes()
        {
            bool containsMSCorLib = false;

            var typeRelatedAssemblies = ProjectSettings.UseBuiltInNames
                ? TypeCollector.GetAssembliesTypeHasAccessTo(_declaringType, out containsMSCorLib)
                : TypeCollector.GetAssembliesTypeHasAccessTo(_declaringType);

            if (_attribute.IncludeAdditionalAssemblies != null)
                IncludeAdditionalAssemblies(typeRelatedAssemblies);

            var filteredTypes = TypeCollector.GetFilteredTypesFromAssemblies(typeRelatedAssemblies, _attribute);

            bool replaceBuiltInNames = ProjectSettings.UseBuiltInNames && containsMSCorLib;

            int filteredTypesLength = filteredTypes.Count;

            var typeItems = new DropdownItem<Type>[filteredTypesLength];

            for (int i = 0; i < filteredTypesLength; i++)
            {
                var type = filteredTypes[i];

                string fullTypeName = type.FullName;
                Assert.IsNotNull(fullTypeName);

                if (replaceBuiltInNames)
                    fullTypeName = fullTypeName.ReplaceWithBuiltInName(true);

                typeItems[i] = CreateItem(type, _attribute.Grouping, fullTypeName);
            }

            return typeItems;
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