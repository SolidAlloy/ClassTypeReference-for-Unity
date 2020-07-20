namespace TypeReferences.Editor
{
    using System;
    using System.Collections.Generic;
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

            foreach (var type in GetFilteredTypes())
            {
                var menuLabel = TypeNameFormatter.Format(type, classGrouping);
                AddLabelIfNotEmpty(menuLabel, type);
            }

            _menu.DropDown(position);
        }

        private void AddNoneElementIfNotExcluded()
        {
            var excludeNone = _constraints?.ExcludeNone ?? false;
            if (excludeNone)
                return;

            _menu.AddItem(
                new GUIContent(ClassTypeReference.NoneElement),
                _selectedType == null,
                CachedTypeReference.SelectedTypeName,
                null);

            _menu.AddSeparator(string.Empty);
        }

        private IEnumerable<Type> GetFilteredTypes()
        {
            var typeRelatedAssemblies = TypeCollector.GetAssembliesTypeHasAccessTo(_declaringType);

            var filteredTypes = TypeCollector.GetFilteredTypesFromAssemblies(
                typeRelatedAssemblies,
                _constraints);

            filteredTypes.Sort((a, b) => a.FullName.CompareTo(b.FullName));

            return filteredTypes;
        }

        private void AddLabelIfNotEmpty(string menuLabel, Type type)
        {
            if (string.IsNullOrEmpty(menuLabel))
                return;

            var content = new GUIContent(menuLabel);
            _menu.AddItem(content, _selectedType == type, CachedTypeReference.SelectedTypeName, type);
        }
    }
}