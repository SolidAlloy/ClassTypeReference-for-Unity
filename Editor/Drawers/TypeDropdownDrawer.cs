namespace TypeReferences.Editor.Drawers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using SolidUtilities;
    using UnityDropdown.Editor;
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
            var dropdownItems = GetDropdownItems().ToList();
            SelectItem(dropdownItems, _selectedType);

            var dropdownMenu = new DropdownMenu<Type>(dropdownItems, onTypeSelected, ProjectSettings.SearchbarMinItemsCount, true, _attribute.ShowNoneElement);

            if (_attribute.ExpandAllFolders)
                dropdownMenu.ExpandAllFolders();

            dropdownMenu.ShowAsContext(_attribute.DropdownHeight);
        }

        public IEnumerable<DropdownItem<Type>> GetDropdownItems()
        {
            return GetMainDropdownItems().Concat(GetIncludedTypes());
        }

        private void SelectItem(List<DropdownItem<Type>> dropdownItems, Type selectedType)
        {
            if (selectedType == null)
                return;

            var itemToSelect = dropdownItems.Find(item => item.Value == selectedType);

            if (itemToSelect != null)
                itemToSelect.IsSelected = true;
        }

        private IEnumerable<DropdownItem<Type>> GetIncludedTypes()
        {
            if (_attribute.IncludeTypes == null)
                return Array.Empty<DropdownItem<Type>>();

            var typeItems = new DropdownItem<Type>[_attribute.IncludeTypes.Length];

            for (int i = 0; i < _attribute.IncludeTypes.Length; i++)
            {
                Type type = _attribute.IncludeTypes[i];

                if (type != null)
                {
                    typeItems[i] = CreateItem(type, _attribute.Grouping, type.FullName);
                }
                else
                {
                    throw new ArgumentException("IncludeTypes must not contain null.");
                }
            }

            return typeItems;
        }

        private DropdownItem<Type> CreateItem(Type type, Grouping grouping, string searchName)
        {
            return new DropdownItem<Type>(type, TypeNameFormatter.Format(type, searchName, grouping), searchName: searchName);
        }

        private IEnumerable<DropdownItem<Type>> GetMainDropdownItems()
        {
            bool containsMSCorLib = false;
            var assemblies = _attribute.ShowAllTypes ? TypeCollector.GetAllAssemblies() : GetTypeRelatedAssemblies(out containsMSCorLib);
            
            if (_attribute.ShowAllTypes)
                containsMSCorLib = true;

            var filteredTypes = TypeCollector.GetFilteredTypesFromAssemblies(assemblies, _attribute);

            bool replaceBuiltInNames = ProjectSettings.UseBuiltInNames && containsMSCorLib;

            foreach (var filteredType in filteredTypes)
            {
                string fullTypeName = filteredType.FullName;
                Assert.IsNotNull(fullTypeName);

                if (replaceBuiltInNames)
                    fullTypeName = fullTypeName.ReplaceWithBuiltInName(true);

                yield return CreateItem(filteredType, _attribute.Grouping, fullTypeName);
            }
        }

        private IEnumerable<Assembly> GetTypeRelatedAssemblies(out bool containsMSCorLib)
        {
            var typeRelatedAssemblies = TypeCollector.GetAssembliesTypeHasAccessTo(_declaringType, out containsMSCorLib);

            if (_attribute.IncludeAdditionalAssemblies != null)
                typeRelatedAssemblies = typeRelatedAssemblies.Concat(GetAdditionalAssemblies());

            return typeRelatedAssemblies;
        }

        private IEnumerable<Assembly> GetAdditionalAssemblies()
        {
            return _attribute.IncludeAdditionalAssemblies.Select(TypeCollector.TryLoadAssembly).Where(additionalAssembly => additionalAssembly != null);
        }
    }
}