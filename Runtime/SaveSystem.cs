using System.Collections.Generic;
using UnityEngine;

namespace Kaynir.Saves
{
    public class SaveSystem : MonoBehaviour
    {
        public delegate void OnStateChanged(SaveState state);

        public static event OnStateChanged OnGameSaved;
        public static event OnStateChanged OnGameLoaded;

        [SerializeField] protected SaveProvider _offlineSaveProvider = null;

        public bool IsInitialized => _saveState != null;

        private SaveState _saveState;
        private List<SaveableEntity> _saveableList;

        private void Awake()
        {
            _saveState = null;
            _saveableList = new List<SaveableEntity>();

            SaveableEntity.OnEnabled += SubscribeSaveable;
            SaveableEntity.OnDisabled += UnsubscribeSaveable;
        }

        private void OnDestroy()
        {
            SaveableEntity.OnEnabled -= SubscribeSaveable;
            SaveableEntity.OnDisabled -= UnsubscribeSaveable;
        }

        [ContextMenu("Save Game")]
        public virtual void SaveGame()
        {
            _saveState.UpdatePlayTime();
            _saveableList.ForEach(e => e.CaptureState(_saveState));
            _offlineSaveProvider.Save(_saveState, OnSaveCompleted);
        }

        [ContextMenu("Load Game")]
        public virtual void LoadGame()
        {
            _offlineSaveProvider.Load<SaveState>(OnLoadCompleted);
        }

        private void OnLoadCompleted(SaveState state)
        {
            _saveState = state;
            _saveableList.ForEach(e => e.RestoreState(state));

            Debug.Log("Game state is loaded.");
            OnGameLoaded?.Invoke(_saveState);
        }

        private void OnSaveCompleted()
        {
            Debug.Log("Game state is saved.");
            OnGameSaved?.Invoke(_saveState);
        }

        #region Saveable Subscriptions
        private void SubscribeSaveable(SaveableEntity saveable)
        {
            _saveableList.Add(saveable);

            if (!IsInitialized) return;
            RestoreSaveableState(saveable);
        }

        private void UnsubscribeSaveable(SaveableEntity saveable) => _saveableList.Remove(saveable);

        private void CaptureSaveableState(SaveableEntity saveable) => saveable.CaptureState(_saveState);

        private void RestoreSaveableState(SaveableEntity saveable) => saveable.RestoreState(_saveState);
        #endregion
    }
}