using UnityEngine;

namespace Kaynir.AdvancedSaveSystem
{
    public class AdvancedSaveSystem<T> : MonoBehaviour where T : class
    {
        [SerializeField] private SaveProvider _saveProvider = null;

        private T _saveData;

        protected virtual void Awake()
        {
            SaveableBehaviour<T>.OnEnabled += LoadState;
        }

        protected virtual void OnDestroy()
        {
            SaveableBehaviour<T>.OnEnabled -= LoadState;
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

        private SaveableBehaviour<T>[] FindSaveables()
        {
            return FindObjectsOfType<SaveableBehaviour<T>>();
        }

        protected virtual T GetDefaultSaveData()
        {
            return default;
        }
    }
}