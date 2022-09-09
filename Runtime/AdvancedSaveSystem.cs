using UnityEngine;

namespace CozyDragon.Saves
{
    public class AdvancedSaveSystem<T> : MonoBehaviour where T : class
    {
        [SerializeField] private SaveProvider _saveProvider = null;

        private T _saveData;

        private void Awake()
        {
            SaveableBehaviour<T>.OnEnabled += LoadState;
            SaveableBehaviour<T>.OnDisabled += SaveState;

            _saveData = GetDefaultSaveData();
        }

        private void OnDestroy()
        {
            SaveableBehaviour<T>.OnEnabled -= LoadState;
            SaveableBehaviour<T>.OnDisabled -= SaveState;
        }

        public void SaveState()
        {
            foreach (SaveableBehaviour<T> saveable in FindSaveables())
            {
                SaveState(saveable);
            }
        }

        public void LoadState()
        {
            foreach (SaveableBehaviour<T> saveable in FindSaveables())
            {
                LoadState(saveable);
            }
        }

        public void SaveData()
        {
            SaveState();

            _saveProvider.Save(_saveData);
        }

        public void LoadData()
        {
            _saveData = _saveProvider.Load<T>(GetDefaultSaveData());

            LoadState();
        }

        private void LoadState(SaveableBehaviour<T> saveable) => saveable.RestoreState(_saveData);

        private void SaveState(SaveableBehaviour<T> saveable) => saveable.CaptureState(_saveData);

        protected virtual T GetDefaultSaveData()
        {
            return default;
        }

        private SaveableBehaviour<T>[] FindSaveables()
        {
            return FindObjectsOfType<SaveableBehaviour<T>>();
        }
    }
}