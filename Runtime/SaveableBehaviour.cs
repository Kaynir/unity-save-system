using System;
using UnityEngine;

namespace Kaynir.AdvancedSaveSystem
{
    public abstract class SaveableBehaviour<T> : MonoBehaviour where T : class
    {
        public static event Action<SaveableBehaviour<T>> OnEnabled;

        protected virtual void OnEnable()
        {
            OnEnabled?.Invoke(this);
        }

        public abstract void CaptureState(T saveData);

        public abstract void RestoreState(T saveData);
    }
}