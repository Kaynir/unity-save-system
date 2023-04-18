using System;
using System.IO;
using UnityEngine;

namespace Kaynir.Saves.Providers
{
    public class FileSaveProvider : SaveProvider
    {
        [SerializeField] private string _fileName = "data.json";
        [SerializeField] private string _backupFileName = "data-bak.json";
        [SerializeField] private bool _enableBackup = true;

        public override void Save<T>(T data, Action onComplete)
        {
            Save(data, GetFullPath(_fileName), _enableBackup, onComplete);
        }

        public override void Load<T>(Action<T> onComplete)
        {
            Load(GetFullPath(_fileName), _enableBackup, onComplete);
        }

        private void Load<T>(string fullPath, bool enableBackup, Action<T> onComplete) where T : new()
        {
            T data = new T();

            try
            {
                string json = File.ReadAllText(fullPath);
                data = JsonUtility.FromJson<T>(json);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to read {fullPath} with exception: {ex}.");

                if (enableBackup)
                {
                    Debug.LogWarning($"Trying to restore data from backup file: {_backupFileName}.");
                    Load<T>(GetFullPath(_backupFileName), false, onComplete);
                    return;
                }
            }
            
            onComplete?.Invoke(data);
        }

        private void Save<T>(T data, string fullPath, bool enableBackup, Action onComplete)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                File.WriteAllText(fullPath, JsonUtility.ToJson(data, true));

                if (enableBackup)
                {
                    Save(data, GetFullPath(_backupFileName), false, null);
                }

                onComplete?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to write {fullPath} with exception: {ex}.");
            }
        }

        private string GetFullPath(string filePath) => $"{Application.persistentDataPath}/{filePath}";
    }
}