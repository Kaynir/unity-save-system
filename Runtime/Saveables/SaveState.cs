using System;
using Kaynir.Saves.Tools;
using UnityEngine;

namespace Kaynir.Saves.Saveables
{
    [Serializable]
    public class SaveState
    {
        [SerializeField] private SerializableDictionary<string, string> _data;

        public SaveState()
        {
            _data = new SerializableDictionary<string, string>();
        }

        #region String Data
        public string GetString(string key, string defaultValue)
        {
            if (_data.TryGetValue(key, out string data)) return data;

            Debug.LogWarning($"Retriving default value for missing key: {key}.");
            return defaultValue;
        }

        public string GetString(string key) => GetString(key, string.Empty);
        public void SetString(string key, string value) => _data[key] = value;
        #endregion

        #region Integer Data
        public int GetInt(string key, int defaultValue) => ParseHelper.ParseInt(GetString(key), defaultValue);
        public int GetInt(string key) => GetInt(key, 0);
        public void SetInt(string key, int value) => SetString(key, value.ToString());
        #endregion

        #region Float Data
        public float GetFloat(string key, float defaultValue) => ParseHelper.ParseFloat(GetString(key), defaultValue);
        public float GetFloat(string key) => GetFloat(key, 0f);
        public void SetFloat(string key, float value) => SetString(key, value.ToString());
        #endregion

        #region Custom Data
        public T GetData<T>(string key, T defaultValue) => ParseHelper.ParseJson(GetString(key), defaultValue);
        public T GetData<T>(T defaultValue) => GetData<T>(GenerateKey<T>(), defaultValue);
        public T GetData<T>(string key) where T : new() => GetData<T>(key, new T());
        public T GetData<T>() where T : new() => GetData<T>(GenerateKey<T>(), new T());
        public void SetData<T>(string key, T data) => SetString(key, JsonUtility.ToJson(data));
        public void SetData<T>(T data) => SetData(GenerateKey<T>(), data);
        private string GenerateKey<T>() => typeof(T).Name;
        #endregion

        #region Json Conversions
        public string ToJson() => JsonUtility.ToJson(this);
        public static SaveState FromJson(string json) => ParseHelper.ParseJson(json, new SaveState());
        #endregion
    }
}