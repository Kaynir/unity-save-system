using System;
using UnityEngine;

namespace Kaynir.Saves
{
    public class SaveableEntity : MonoBehaviour
    {
        public static event Action<SaveableEntity> OnEnabled;
        public static event Action<SaveableEntity> OnDisabled;

        private ISaveable[] _saveables;

        private void Awake()
        {
            _saveables = GetComponentsInChildren<ISaveable>();
        }

        private void OnEnable()
        {
            OnEnabled?.Invoke(this);
        }

        private void OnDisable()
        {
            OnDisabled?.Invoke(this);
        }

        public void CaptureState(SaveState state)
        {
            for (int i = 0; i < _saveables.Length; i++)
            {
                _saveables[i].CaptureState(state);
            }
        }

        public void RestoreState(SaveState state)
        {
            for (int i = 0; i < _saveables.Length; i++)
            {
                _saveables[i].RestoreState(state);
            }
        }
    }
}