using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kaynir.Saves.Tools
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<KeyValuePair> _collection;

        public void OnAfterDeserialize()
        {
            Clear();

            _collection.ForEach(item => Add(item.Key, item.Value));
        }

        public void OnBeforeSerialize()
        {
            _collection = new List<KeyValuePair>();

            foreach (var item in this)
            {
                _collection.Add(new KeyValuePair(item.Key, item.Value));
            }
        }

        [Serializable]
        private struct KeyValuePair
        {
            [SerializeField] private TKey _key;
            [SerializeField] private TValue _value;

            public TKey Key => _key;
            public TValue Value => _value;

            public KeyValuePair(TKey key, TValue value)
            {
                _key = key;
                _value = value;
            }
        }
    }
}