using System;
using UnityEngine;

namespace CozyDragon.Saves
{
    public abstract class SaveableBehaviour<T> : MonoBehaviour where T : class
    {
        public static event Action<SaveableBehaviour<T>> OnEnabled;
        public static event Action<SaveableBehaviour<T>> OnDisabled;

        protected virtual void OnEnable()
        {
            OnEnabled?.Invoke(this);
        }

        protected virtual void OnDisable()
        {
            OnDisabled?.Invoke(this);
        }

        public abstract void CaptureState(T saveData);

        public abstract void RestoreState(T saveData);
    }
}