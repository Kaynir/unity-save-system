using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kaynir.Saves
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<SerializablePair<TKey, TValue>> _collection;

        public void OnAfterDeserialize()
        {
            Clear();

            for (int i = 0; i < _collection.Count; i++)
            {
                Add(_collection[i].Key, _collection[i].Value);
            }
        }

        public void OnBeforeSerialize()
        {
            _collection = new List<SerializablePair<TKey, TValue>>();

            foreach (var item in this)
            {
                _collection.Add(new SerializablePair<TKey, TValue>(item.Key, item.Value));
            }
        }

        [Serializable]
        private struct SerializablePair<K, V>
        {
            [SerializeField] private K _key;
            [SerializeField] private V _value;

            public K Key => _key;
            public V Value => _value;

            public SerializablePair(K key, V value)
            {
                _key = key;
                _value = value;
            }
        }
    }
}