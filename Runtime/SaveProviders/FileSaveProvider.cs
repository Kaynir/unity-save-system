using System;
using System.IO;
using UnityEngine;

namespace Kaynir.Saves
{
    public class FileSaveProvider : SaveProvider
    {
        [SerializeField] private string _fileName = "data.json";
        [SerializeField] private string _backupFileName = "data-bak.json";
        [SerializeField] private bool _enableBackup = true;

        public override void Save<T>(T data, OnSaveCompleted onCompleted)
        {
            Save(data, GetFullPath(_fileName), _enableBackup, onCompleted);
        }

        public override void Load<T>(OnLoadCompleted<T> onCompleted)
        {
            Load(GetFullPath(_fileName), _enableBackup, onCompleted);
        }

        private void Load<T>(string fullPath, bool enableBackup, OnLoadCompleted<T> onCompleted) where T : new()
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
                    Load<T>(GetFullPath(_backupFileName), false, onCompleted);
                    return;
                }
            }
            
            onCompleted?.Invoke(data);
        }

        private void Save<T>(T data, string fullPath, bool enableBackup, OnSaveCompleted onCompleted)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                File.WriteAllText(fullPath, JsonUtility.ToJson(data, true));

                if (enableBackup)
                {
                    Save(data, GetFullPath(_backupFileName), false, null);
                }

                onCompleted?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to write {fullPath} with exception: {ex}.");
            }
        }

        private string GetFullPath(string filePath) => $"{Application.persistentDataPath}/{filePath}";
    }
}