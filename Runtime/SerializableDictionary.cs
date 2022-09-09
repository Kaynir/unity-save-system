using System;
using System.Collections.Generic;
using UnityEngine;

namespace CozyDragon.Saves
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<KeyValuePair<TKey, TValue>> _collection = new List<KeyValuePair<TKey, TValue>>();

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
            _collection.Clear();

            foreach (var item in this)
            {
                _collection.Add(new KeyValuePair<TKey, TValue>(item.Key, item.Value));
            }
        }

        [Serializable]
        private struct KeyValuePair<K, V>
        {
            [SerializeField] private K _key;
            [SerializeField] private V _value;

            public K Key => _key;
            public V Value => _value;

            public KeyValuePair(K key, V value)
            {
                _key = key;
                _value = value;
            }
        }
    }
}