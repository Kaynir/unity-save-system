using System;
using UnityEngine;

namespace Kaynir.Saves
{
    public class SaveSystem : MonoBehaviour
    {
        public delegate void OnStateChanged(SaveState state);

        public static event OnStateChanged OnSaveRequested;
        public static event OnStateChanged OnSaveCompleted;
        public static event OnStateChanged OnLoadCompleted;

        [SerializeField] protected SaveProvider _offlineSaveProvider = null;

        private SaveState _saveState;

        private void Awake()
        {
            SaveableEntity.OnEnabled += RestoreSaveableState;
        }

        private void OnDestroy()
        {
            SaveableEntity.OnEnabled -= RestoreSaveableState;
        }

        public virtual void SaveState(Action onCompleted)
        {
            _saveState.UpdatePlayTime();

            OnSaveRequested?.Invoke(_saveState);

            _offlineSaveProvider.Save(_saveState, () =>
            {
                CompleteSave(onCompleted);
            });
        }

        [ContextMenu("Save State")]
        public void SaveState() => SaveState(null);

        public virtual void LoadState(Action onCompleted)
        {
            _offlineSaveProvider.Load<SaveState>((state) =>
            {
                CompleteLoad(state, onCompleted);
            });
        }

        [ContextMenu("Load State")]
        public void LoadState() => LoadState(null);

        private void CompleteLoad(SaveState state, Action callback)
        {
            _saveState = state;

            OnLoadCompleted?.Invoke(_saveState);
            callback?.Invoke();
        }

        private void CompleteSave(Action callback)
        {
            OnSaveCompleted?.Invoke(_saveState);
            callback?.Invoke();
        }

        private void RestoreSaveableState(SaveableEntity saveable)
        {
            saveable.RestoreState(_saveState);
        }
    }
}