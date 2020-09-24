namespace SolidUtilities.SerializableCollections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using UnityEngine;

    /// <summary>Dictionary that can be serialized by Unity and shown in the inspector.</summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <remarks>
    /// This is a fork of https://github.com/azixMcAze/Unity-SerializableDictionary. I cleaned it up and refactored a bit.
    /// </remarks>
    /// <example><code>
    /// public class Animals : MonoBehaviour
    /// {
    ///     [SerializeField] private SerializableDictionary&lt;string, Animal&gt; _animals;
    /// }
    /// </code></example>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary,
        ISerializationCallbackReceiver, IDeserializationCallback, ISerializable
    {
        private Dictionary<TKey, TValue> _dict;
        [SerializeField] private TKey[] _keys;
        [SerializeField] private TValue[] _values;

        public SerializableDictionary() => _dict = new Dictionary<TKey, TValue>();

        public SerializableDictionary(IDictionary<TKey, TValue> dict) => _dict = new Dictionary<TKey, TValue>(dict);

        public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>) _dict).Keys;
        public ICollection<TValue> Values => ((IDictionary<TKey, TValue>) _dict).Values;
        public int Count => ((IDictionary<TKey, TValue>) _dict).Count;
        public bool IsReadOnly => ((IDictionary<TKey, TValue>) _dict).IsReadOnly;
        public bool IsFixedSize => ((IDictionary) _dict).IsFixedSize;
        ICollection IDictionary.Keys => ((IDictionary) _dict).Keys;
        ICollection IDictionary.Values => ((IDictionary) _dict).Values;
        public bool IsSynchronized => ((IDictionary) _dict).IsSynchronized;
        public object SyncRoot => ((IDictionary) _dict).SyncRoot;

        public object this[object key]
        {
            get => ((IDictionary) _dict)[key];
            set => ((IDictionary) _dict)[key] = value;
        }

        public TValue this[TKey key]
        {
            get => ((IDictionary<TKey, TValue>) _dict)[key];
            set => ((IDictionary<TKey, TValue>) _dict)[key] = value;
        }

        public void CopyFrom(IDictionary<TKey, TValue> dict)
        {
            _dict.Clear();
            foreach (var kvp in dict)
                _dict[kvp.Key] = kvp.Value;
        }

        public void OnAfterDeserialize()
        {
            if (_keys == null || _values == null || _keys.Length != _values.Length)
                return;

            _dict.Clear();
            int keysLength = _keys.Length;

            for (int i = 0; i < keysLength; ++i)
                _dict[_keys[i]] = GetValue(_values, i);

            _keys = null;
            _values = null;
        }

        public void OnBeforeSerialize()
        {
            int dictLength = _dict.Count;
            _keys = new TKey[dictLength];
            _values = new TValue[dictLength];

            int keysIndex = 0;
            foreach (var pair in _dict)
            {
                _keys[keysIndex] = pair.Key;
                SetValue(_values, keysIndex, pair.Value);
                ++keysIndex;
            }
        }

        public void Add(TKey key, TValue value) => ((IDictionary<TKey, TValue>) _dict).Add(key, value);

        public bool ContainsKey(TKey key) => ((IDictionary<TKey, TValue>) _dict).ContainsKey(key);

        public bool Remove(TKey key) => ((IDictionary<TKey, TValue>) _dict).Remove(key);

        public bool TryGetValue(TKey key, out TValue value) =>
            ((IDictionary<TKey, TValue>) _dict).TryGetValue(key, out value);

        public void Add(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>) _dict).Add(item);

        public void Clear() => ((IDictionary<TKey, TValue>) _dict).Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item) =>
            ((IDictionary<TKey, TValue>) _dict).Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            ((IDictionary<TKey, TValue>) _dict).CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>) _dict).Remove(item);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
            ((IDictionary<TKey, TValue>) _dict).GetEnumerator();

        public void Add(object key, object value) => ((IDictionary) _dict).Add(key, value);

        public bool Contains(object key) => ((IDictionary) _dict).Contains(key);

        public void Remove(object key) => ((IDictionary) _dict).Remove(key);

        public void CopyTo(Array array, int index) => ((IDictionary) _dict).CopyTo(array, index);

        public void OnDeserialization(object sender) => ((IDeserializationCallback) _dict).OnDeserialization(sender);

        public void GetObjectData(SerializationInfo info, StreamingContext context) =>
            ((ISerializable) _dict).GetObjectData(info, context);

        IEnumerator IEnumerable.GetEnumerator() => ((IDictionary<TKey, TValue>) _dict).GetEnumerator();

        IDictionaryEnumerator IDictionary.GetEnumerator() => ((IDictionary) _dict).GetEnumerator();

        private TValue GetValue(TValue[] storage, int i) => storage[i];

        private void SetValue(TValue[] storage, int i, TValue value) => storage[i] = value;
    }
}