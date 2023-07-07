using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kaynir.Saves.Tools
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<KeyValuePair> collection;

        public void OnAfterDeserialize()
        {
            Clear();
            collection.ForEach(p => this[p.key] = p.value);
        }

        public void OnBeforeSerialize()
        {
            collection = this.Select(p => new KeyValuePair(p.Key, p.Value)).ToList();
        }

        [Serializable]
        private struct KeyValuePair
        {
            public TKey key;
            public TValue value;

            public KeyValuePair(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}