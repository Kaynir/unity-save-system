using System;
using UnityEngine;

namespace CozyDragon.Saves
{
    public abstract class SaveableBehaviour : MonoBehaviour
    {
        public static event Action<SaveableBehaviour> OnEnabled;
        public static event Action<SaveableBehaviour> OnDisabled;

        [field: SerializeField] public string UniqueID { get; private set; } = string.Empty;

        protected virtual void OnEnable()
        {
            OnEnabled?.Invoke(this);
        }

        protected virtual void OnDisable()
        {
            OnDisabled?.Invoke(this);
        }

        public abstract SaveableState CaptureState();

        public abstract void RestoreState(SaveableState state);

        [ContextMenu("Generate ID")]
        public void GenerateID()
        {
            UniqueID = Guid.NewGuid().ToString();
        }
    }
}