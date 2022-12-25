using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kaynir.Saves
{
    public class SaveSystem : MonoBehaviour
    {
        public delegate void OnStateChanged(SaveState state);

        public static event OnStateChanged OnStateSaved;
        public static event OnStateChanged OnStateLoaded;

        [SerializeField] protected SaveProvider _offlineSaveProvider = null;

        private SaveState _saveState;
        private List<SaveableEntity> _saveableList;

        private void Awake()
        {
            _saveState = new SaveState();
            _saveableList = new List<SaveableEntity>();
            _saveableList.AddRange(FindObjectsOfType<SaveableEntity>());

            SaveableEntity.OnEnabled += SubscribeSaveable;
            SaveableEntity.OnDisabled += UnsubscribeSaveable;
        }

        private void OnDestroy()
        {
            SaveableEntity.OnEnabled -= SubscribeSaveable;
            SaveableEntity.OnDisabled -= UnsubscribeSaveable;
        }
        
        public virtual void SaveState(Action onCompleted)
        {
            _saveState.UpdatePlayTime();
            _saveableList.ForEach(e => e.CaptureState(_saveState));
            _offlineSaveProvider.Save(_saveState, () =>
            {
                OnSaveCompleted(onCompleted);
            });
        }

        [ContextMenu("Save State")]
        public void SaveState() => SaveState(null);

        public virtual void LoadState(Action onCompleted)
        {
            _offlineSaveProvider.Load<SaveState>((state) =>
            {
                OnLoadCompleted(state, onCompleted);
            });
        }

        [ContextMenu("Load State")]
        public void LoadState() => LoadState(null);

        private void OnLoadCompleted(SaveState state, Action callback)
        {
            _saveState = state;
            _saveableList.ForEach(e => e.RestoreState(state));

            Debug.Log("Game state is loaded.");
            callback?.Invoke();
            OnStateLoaded?.Invoke(_saveState);
        }

        private void OnSaveCompleted(Action callback)
        {
            Debug.Log("Game state is saved.");
            callback?.Invoke();
            OnStateSaved?.Invoke(_saveState);
        }

        #region Saveable Subscriptions
        private void SubscribeSaveable(SaveableEntity saveable)
        {
            if (_saveableList.Contains(saveable)) return;

            _saveableList.Add(saveable);

            RestoreSaveableState(saveable);
        }

        private void UnsubscribeSaveable(SaveableEntity saveable) => _saveableList.Remove(saveable);

        private void CaptureSaveableState(SaveableEntity saveable) => saveable.CaptureState(_saveState);

        private void RestoreSaveableState(SaveableEntity saveable) => saveable.RestoreState(_saveState);
        #endregion
    }
}