using UnityEngine;

namespace CozyDragon.Saves
{
    public class AdvancedSaveSystem : MonoBehaviour
    {
        [SerializeField] private SaveProvider _saveProvider = null;

        private SerializableDictionary<string, SaveableState> _gameData;

        private void Awake()
        {
            SaveableBehaviour.OnEnabled += LoadState;
            SaveableBehaviour.OnDisabled += SaveState;
        }

        private void OnDestroy()
        {
            SaveableBehaviour.OnEnabled -= LoadState;
            SaveableBehaviour.OnDisabled -= SaveState;
        }

        [ContextMenu("Save State")]
        public void SaveState()
        {
            foreach (SaveableBehaviour saveable in FindSaveables())
            {
                SaveState(saveable);
            }
        }

        [ContextMenu("Load State")]
        public void LoadState()
        {
            foreach (SaveableBehaviour saveable in FindSaveables())
            {
                LoadState(saveable);
            }
        }

        [ContextMenu("Save Data")]
        public void SaveData()
        {
            SaveState();
            _saveProvider.Save(_gameData);
        }

        [ContextMenu("Load Data")]
        public void LoadData()
        {
            _gameData = _saveProvider.Load<SerializableDictionary<string, SaveableState>>();
            LoadState();
        }

        private void LoadState(SaveableBehaviour saveable)
        {
            if (_gameData == null) return;
            
            if (_gameData.TryGetValue(saveable.UniqueID, out SaveableState state))
            {
                saveable.RestoreState(state);
            }
        }

        private void SaveState(SaveableBehaviour saveable)
        {
            if (_gameData.ContainsKey(saveable.UniqueID))
            {
                _gameData[saveable.UniqueID] = saveable.CaptureState();
                return;
            }

            _gameData.Add(saveable.UniqueID, saveable.CaptureState());
        }

        private SaveableBehaviour[] FindSaveables()
        {
            return FindObjectsOfType<SaveableBehaviour>();
        }
    }
}