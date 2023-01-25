using System;
using UnityEngine;

namespace Kaynir.Saves.Saveables
{
    public abstract class SaveableVariable<T> : SaveableBehaviour
    {
        public event Action<T> OnValueLoaded;

        [SerializeField] protected T _defaultValue = default;

        public T Value { get; set; }

        protected void LoadValue(T value)
        {
            Value = value;
            OnValueLoaded?.Invoke(value);
        }
    }
}