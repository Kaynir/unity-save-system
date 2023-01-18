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
        public static event OnStateChanged OnSaveCompleted;
        public static event OnStateChanged OnLoadCompleted;

        [SerializeField] private SaveProvider _saveProvider = null;

        private static SaveState _state = new SaveState();

        private void Awake() => SaveableListener.OnLoaded += LoadListenerState;
        
        private void OnDestroy() => SaveableListener.OnLoaded -= LoadListenerState;

        public void SaveState(Action onCompleted)
        {
            OnSaveRequested?.Invoke(_state);

            _saveProvider.Save(_state, () =>
            {
                CompleteSave(onCompleted);
            });
        }

        [ContextMenu("Save State")]
        public void SaveState() => SaveState(null);

        public void LoadState(Action onCompleted)
        {
            _saveProvider.Load<SaveState>((state) =>
            {
                CompleteLoad(state, onCompleted);
            });
        }

        [ContextMenu("Load State")]
        public void LoadState() => LoadState((Action)null);

        private void CompleteLoad(SaveState state, Action callback)
        {
            _state = state;

            OnLoadCompleted?.Invoke(_state);
            callback?.Invoke();
        }

        private void CompleteSave(Action callback)
        {
            OnSaveCompleted?.Invoke(_state);
            callback?.Invoke();
        }

        private void LoadListenerState(SaveableListener listener)
        {
            listener.RestoreState(_state);
        }
    }
}