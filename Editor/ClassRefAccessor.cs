namespace TypeReferences.Editor
{
    using UnityEditor;

    public class ClassRefAccessor
    {
        private SerializedProperty _boundProperty;

        public ClassRefAccessor(SerializedProperty classRefProperty)
        {
            _boundProperty = classRefProperty;
        }

        public string Value
        {
            get => _boundProperty.stringValue;
            set => _boundProperty.stringValue = value;
        }

        public bool HasMultipleDifferentValues => _boundProperty.hasMultipleDifferentValues;
    }
}