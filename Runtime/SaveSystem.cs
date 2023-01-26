using System;
using Kaynir.Saves.Providers;
using Kaynir.Saves.Saveables;
using UnityEngine;

namespace Kaynir.Saves
{
    public class SaveSystem : MonoBehaviour
    {
        public delegate void OnStateChanged(SaveState state);

        public static event OnStateChanged OnSaveRequested;
        public static event OnStateChanged OnStateSaved;
        public static event OnStateChanged OnStateLoaded;

        [SerializeField] private SaveProvider _saveProvider = null;

        public static SaveState State { get; private set; } = new SaveState();

        public void Save(Action onCompleted)
        {
            OnSaveRequested?.Invoke(State);

            _saveProvider.Save(State, () =>
            {
                OnSaveCompleted(onCompleted);
            });
        }

        [ContextMenu("Save")]
        public void Save() => Save(null);

        public void Load(Action onCompleted)
        {
            _saveProvider.Load<SaveState>((state) =>
            {
                OnLoadCompleted(state, onCompleted);
            });
        }

        [ContextMenu("Load")]
        public void Load() => Load(null);

        private void OnLoadCompleted(SaveState state, Action callback)
        {
            State = state;

            OnStateLoaded?.Invoke(State);
            callback?.Invoke();
        }

        private void OnSaveCompleted(Action callback)
        {
            OnStateSaved?.Invoke(State);
            callback?.Invoke();
        }
    }
}