namespace TypeReferences.Editor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class TypeDropDown
    {
        private readonly Type _selectedType;
        private readonly ClassTypeConstraintAttribute _filter;
        private readonly Type _declaringType;
        private GenericMenu _menu;

        public TypeDropDown(string classRef, ClassTypeConstraintAttribute filter, Type declaringType)
        {
            _selectedType = CachedTypeReference.GetType(classRef);
            _filter = filter;
            _declaringType = declaringType;
        }

        /// <summary>
        /// Gets or sets a function that returns a collection of types that are
        /// to be excluded from drop-down. A value of <c>null</c> specifies that
        /// no types are to be excluded.
        /// </summary>
        /// <remarks>
        /// <para>This property must be set immediately before presenting a class
        /// type reference property field using <see cref="M:EditorGUI.PropertyField"/>
        /// or <see cref="M:EditorGUILayout.PropertyField"/> since the value of this
        /// property is reset to <c>null</c> each time the control is drawn.</para>
        /// <para>Since filtering makes extensive use of <see cref="ICollection{Type}.Contains"/>
        /// it is recommended to use a collection that is optimized for fast
        /// lookups such as <see cref="HashSet{Type}"/> for better performance.</para>
        /// </remarks>
        /// <example>
        /// <para>Exclude a specific type from being selected:</para>
        /// <code language="csharp"><![CDATA[
        /// private SerializedProperty _someClassTypeReferenceProperty;
        ///
        /// public override void OnInspectorGUI()
        /// {
        ///     serializedObject.Update();
        ///
        ///     ClassTypeReferencePropertyDrawer.ExcludedTypeCollectionGetter = GetExcludedTypeCollection;
        ///     EditorGUILayout.PropertyField(_someClassTypeReferenceProperty);
        ///
        ///     serializedObject.ApplyModifiedProperties();
        /// }
        ///
        /// private ICollection<Type> GetExcludedTypeCollection()
        /// {
        ///     var set = new HashSet<Type>();
        ///     set.Add(typeof(SpecialClassToHideInDropdown));
        ///     return set;
        /// }
        /// ]]></code>
        /// </example>
        public Func<ICollection<Type>> ExcludedTypeCollectionGetter { get; set; }

        public void Display(Rect position)
        {
            _menu = new GenericMenu();

            AddNoneElementIfNotExcluded();

            var classGrouping = _filter?.Grouping ?? ClassTypeConstraintAttribute.DefaultGrouping;

            foreach (var type in GetFilteredTypes())
            {
                var menuLabel = TypeNameFormatter.Format(type, classGrouping);
                AddLabelIfNotEmpty(menuLabel, type);
            }

            _menu.DropDown(position);
        }

        private void AddNoneElementIfNotExcluded()
        {
            var excludeNone = _filter?.ExcludeNone ?? false;
            if (excludeNone)
                return;

            _menu.AddItem(
                new GUIContent(CachedTypeReference.NoneElement),
                _selectedType == null,
                CachedTypeReference.SelectedTypeName,
                null);

            _menu.AddSeparator(string.Empty);
        }

        private IEnumerable<Type> GetFilteredTypes()
        {
            var excludedTypes = ExcludedTypeCollectionGetter?.Invoke();
            var typeRelatedAssemblies = TypeCollector.GetTypeRelatedAssemblies(_declaringType);

            var filteredTypes = TypeCollector.GetFilteredTypesFromAssemblies(
                typeRelatedAssemblies,
                _filter,
                excludedTypes);

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