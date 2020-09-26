namespace SolidUtilities.Editor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;

    /// <summary>
    /// Allows to iterate over child properties of a serialized object without entering nested properties.
    /// </summary>
    /// <example><code>
    /// var childProperties = new ChildProperties(serializedObject, true);
    /// foreach (var child in childProperties)
    /// {
    ///     FieldInfo field = targetType.GetFieldAtPath(child.propertyPath);
    ///     Draw(field);
    /// }
    /// </code></example>
    public class ChildProperties : IEnumerator<SerializedProperty>, IEnumerable<SerializedProperty>
    {
        private readonly SerializedObject _parentObject;
        private readonly bool _enterChildren;
        private SerializedProperty _currentProp;
        private bool _endPropertyNotReached = true;
        private bool _startedIteration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildProperties"/> class.
        /// </summary>
        /// <param name="parentObject">The parent serialized object which child properties you want to inspect.</param>
        /// <param name="enterChildren">Whether to iterate through child properties recursively.</param>
        public ChildProperties(SerializedObject parentObject, bool enterChildren)
        {
            _parentObject = parentObject;
            _enterChildren = enterChildren;
            MoveToFirstProp();
        }

        SerializedProperty IEnumerator<SerializedProperty>.Current => _currentProp;
        object IEnumerator.Current => _currentProp;

        bool IEnumerator.MoveNext()
        {
            if ( ! _startedIteration)
                _endPropertyNotReached = MoveToFirstProp();

            if ( ! _endPropertyNotReached)
                return false;

            _endPropertyNotReached = _currentProp.Next(_enterChildren);
            return _endPropertyNotReached;
        }

        void IEnumerator.Reset()
        {
            MoveToFirstProp();
            _startedIteration = false;
            _endPropertyNotReached = false;
        }

        void IDisposable.Dispose() { }

        IEnumerator<SerializedProperty> IEnumerable<SerializedProperty>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        private bool MoveToFirstProp()
        {
            _currentProp = _parentObject.GetIterator();
            _startedIteration = true;
            return _currentProp.Next(true);
        }
    }
}