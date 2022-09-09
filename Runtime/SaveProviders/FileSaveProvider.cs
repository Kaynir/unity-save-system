using System;
using System.IO;
using UnityEngine;

namespace CozyDragon.Saves
{
    public class FileSaveProvider : SaveProvider
    {
        [Header("File Storage Settings:")]
        [SerializeField] private string _fileName = "data.json";
        [SerializeField] private string _backupFileName = "data-bak.json";
        [SerializeField] private bool _enableBackup = true;

        public override T Load<T>(T defaultData = default)
        {
            return Load<T>(defaultData, GetFullPath(_fileName), _enableBackup);
        }

        public override void Save<T>(T data)
        {
            Save(data, GetFullPath(_fileName), _enableBackup);
        }

        private T Load<T>(T defaultData, string fullPath, bool enableBackup)
        {
            try
            {
                string json = File.ReadAllText(fullPath);
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to read {fullPath} with exception: {ex}.");

                if (enableBackup)
                {
                    Debug.LogWarning($"Trying to restore data from backup file: {_backupFileName}.");
                    return Load<T>(defaultData, GetFullPath(_backupFileName), false);
                }

                return defaultData;
            }
        }

        private void Save<T>(T data, string fullPath, bool enableBackup)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                File.WriteAllText(fullPath, JsonUtility.ToJson(data, true));

                if (enableBackup)
                {
                    Save(data, GetFullPath(_backupFileName), false);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to write {fullPath} with exception: {ex}.");
            }
        }

        private string GetFullPath(string filePath) => $"{Application.persistentDataPath}/{filePath}";
    }
}