using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kaynir.Saves
{
    public class SaveableEntity : MonoBehaviour
    {
        public static event Action<SaveableEntity> OnInitialized;

        private List<ISaveable> _saveables;

        private void Awake()
        {
            _saveables = GetComponentsInChildren<ISaveable>().ToList();
        }

        private void Start()
        {
            OnInitialized?.Invoke(this);
            SaveSystem.OnSaveRequested += CaptureState;
            SaveSystem.OnLoadCompleted += RestoreState;
        }

        private void OnDestroy()
        {
            SaveSystem.OnSaveRequested -= CaptureState;
            SaveSystem.OnLoadCompleted -= RestoreState;
        }

        public void CaptureState(SaveState state)
        {
            _saveables.ForEach(s => s.CaptureState(state));
        }

        public void RestoreState(SaveState state)
        {
            _saveables.ForEach(s => s.RestoreState(state));
        }
    }
}