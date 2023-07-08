using System.IO;
using UnityEngine;

namespace Kaynir.Saves.Tools
{
    public static class StorageTools
    {
        public const string DEFAULT_FILE_NAME = "saveData.json";
        public const string PLAY_TIME_DATA_KEY = "playTime";

        public static string GetPersistentFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}