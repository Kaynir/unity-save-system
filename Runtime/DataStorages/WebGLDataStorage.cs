using System;
using System.IO;
using Kaynir.Saves.Saveables;
using Kaynir.Saves.Tools;
using UnityEngine;

namespace Kaynir.Saves.DataStorages
{
    public class WebGLDataStorage : IDataStorage
    {
        private const string FILE_NAME = "data.json";

        private string _filePath;

        public WebGLDataStorage()
        {
            _filePath = GetFilePath();
        }

        public void GetData(Action<SaveState> onComplete)
        {
            SaveState data = new SaveState();

            try
            {
                string json = File.ReadAllText(_filePath);
                data = SaveState.FromJson(json);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to read {_filePath} with exception: {ex}.");
            }

            onComplete?.Invoke(data);
        }

        public void SetData(SaveState data, Action onComplete)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
                File.WriteAllText(_filePath, data.ToJson());
                WebGLService.UpdateDatabase();
                
                onComplete?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to write {_filePath} with exception: {ex}.");
            }
        }

        private string GetFilePath()
        {
            return Path.Combine(Application.persistentDataPath, FILE_NAME);
        }
    }
}