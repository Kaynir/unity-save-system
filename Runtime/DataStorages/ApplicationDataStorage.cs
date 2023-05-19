using System;
using System.IO;
using Kaynir.Saves.Saveables;
using Kaynir.Saves.Tools;
using UnityEngine;

namespace Kaynir.Saves.DataStorages
{
    public class ApplicationDataStorage : IDataStorage
    {
        private const string DEFAULT_FILE_NAME = "data";
        private const string FILE_EXTENSION = ".json";
        private const string BACKUP_SUFFIX = "-bak";

        private string _fileName;
        private bool _enableBackup;

        public ApplicationDataStorage(string fileName, bool enableBackup)
        {
            _fileName = fileName;
            _enableBackup = enableBackup;
        }

        public ApplicationDataStorage(bool enableBackup) : this(DEFAULT_FILE_NAME, enableBackup) { }
        public ApplicationDataStorage() : this(DEFAULT_FILE_NAME, false) { }

        public void GetData(Action<SaveState> onComplete)
        {
            GetData(GetFilePath(_fileName), _enableBackup, onComplete);
        }

        public void SetData(SaveState data, Action onComplete)
        {
            SetData(data, GetFilePath(_fileName), _enableBackup, onComplete);
        }

        private void GetData(string filePath, bool enableBackup, Action<SaveState> onComplete)
        {
            SaveState data = new SaveState();

            try
            {
                string json = File.ReadAllText(filePath);
                data = SaveState.FromJson(json);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to read {filePath} with exception: {ex}.");

                if (enableBackup)
                {
                    Debug.LogWarning($"Trying to restore data from backup file.");
                    GetData(GetBackupFilePath(_fileName), false, onComplete);
                    return;
                }
            }

            onComplete?.Invoke(data);
        }

        private void SetData(SaveState data, string filePath, bool enableBackup, Action onComplete)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllText(filePath, data.ToJson());
                WebGLService.UpdateDatabase();

                onComplete?.Invoke();

                if (!enableBackup) return;

                SetData(data, GetBackupFilePath(_fileName), false, null);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to write {filePath} with exception: {ex}.");
            }
        }

        private string GetFilePath(string fileName)
        {
            return $"{Application.persistentDataPath}/{fileName}{FILE_EXTENSION}";
        }

        private string GetBackupFilePath(string fileName)
        {
            return GetFilePath($"{fileName}{BACKUP_SUFFIX}");
        }
    }
}