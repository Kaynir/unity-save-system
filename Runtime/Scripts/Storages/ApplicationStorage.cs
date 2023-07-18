using System;
using System.IO;
using Kaynir.Saves.Tools;
using Kaynir.WebGLPlugins.IndexedDB;
using UnityEngine;

namespace Kaynir.Saves.Storages
{
    public class ApplicationStorage : IStorageService
    {
        private string filePath;
        private string folderPath;

        public ApplicationStorage(string fileName)
        {
            filePath = StorageTools.GetPersistentFilePath(fileName);
            folderPath = Path.GetDirectoryName(fileName);
        }

        public ApplicationStorage() : this(StorageTools.DEFAULT_FILE_NAME) { }

        public void GetData(Action<string> onComplete)
        {
            string data = string.Empty;

            try
            {
                data = File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to read {filePath} with exception: {ex}.");
            }

            onComplete?.Invoke(data);
        }

        public void SetData(string data, Action<bool> onComplete)
        {
            try
            {
                Directory.CreateDirectory(folderPath);
                File.WriteAllText(filePath, data);
                IndexedDBService.RefreshDatabase();

                onComplete?.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to write {filePath} with exception: {ex}.");
                onComplete?.Invoke(false);
            }
        }
    }
}