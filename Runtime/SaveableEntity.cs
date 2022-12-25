using System;
using UnityEngine;

namespace Kaynir.Saves
{
    public class SaveableEntity : MonoBehaviour
    {
        public static event Action<SaveableEntity> OnEnabled;

        private ISaveable[] _saveables;

        private void Awake()
        {
            _saveables = GetComponentsInChildren<ISaveable>();
        }

        private void Start()
        {
            SaveSystem.OnSaveRequested += CaptureState;
            SaveSystem.OnLoadCompleted += RestoreState;
            OnEnabled?.Invoke(this);
        }

        private void OnDestroy()
        {
            SaveSystem.OnSaveRequested -= CaptureState;
            SaveSystem.OnLoadCompleted -= RestoreState;
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