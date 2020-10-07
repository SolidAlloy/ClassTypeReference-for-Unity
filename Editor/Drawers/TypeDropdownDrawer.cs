namespace TypeReferences.Editor.Drawers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TypeDropdown;
    using Util;

    /// <summary>
    /// This class gathers needed types from assemblies based on the attribute options, and creates a popup window with
    /// the collected types.
    /// </summary>
    public class TypeDropdownDrawer
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
            var selectionTree = new SelectionTree(dropdownItems, _selectedType, onTypeSelected, _attribute.SearchbarMinItemsCount, _attribute.ExcludeNone);

            if (_attribute.ExpandAllFolders)
                selectionTree.ExpandAllFolders();

            DropdownWindow.Create(selectionTree, _attribute.DropdownHeight);
        }

        private SortedSet<TypeItem> GetDropdownItems()
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
                return new List<TypeItem>();

            var typeItems = new List<TypeItem>(typesToInclude.Length);

            foreach (Type typeToInclude in _attribute.IncludeTypes)
            {
                if (typeToInclude != null && typeToInclude.FullName != null)
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
                .Any(assembly => assembly.GetName().Name == "mscorlib");

            var sortedTypes = new SortedSet<TypeItem>(new TypeItemComparer());

            for (int i = 0; i < filteredTypes.Count; i++)
            {
                var type = filteredTypes[i];
                string fullTypeName = type.FullName;
                if (fullTypeName == null)
                    continue;

                if (replaceBuiltInNames)
                    TypeNameFormatter.TryReplaceWithBuiltInName(ref fullTypeName);

                sortedTypes.Add(new TypeItem(type, fullTypeName, _attribute.Grouping));
            }

            return sortedTypes;
        }

        private void IncludeAdditionalAssemblies(ICollection<Assembly> typeRelatedAssemblies)
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