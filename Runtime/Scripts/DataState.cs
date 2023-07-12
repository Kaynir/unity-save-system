using System;
using Kaynir.Saves.Tools;
using UnityEngine;

namespace Kaynir.Saves
{
    [Serializable]
    public class DataState
    {
        [SerializeField] private SerializableDictionary<string, string> data;

        public DataState()
        {
            data = new SerializableDictionary<string, string>();
        }

        #region String Data
        public string GetString(string key, string defaultValue)
        {
            return data.TryGetValue(key, out string value)
            ? value
            : defaultValue;
        }

        public string GetString(string key) => GetString(key, string.Empty);
        public void SetString(string key, string value) => data[key] = value;
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
        public T GetData<T>(string key) where T : new() => GetData<T>(key, new T());
        public void SetData<T>(string key, T data) => SetString(key, JsonUtility.ToJson(data));
        #endregion

        #region Json Conversions
        public string ToJson(bool prettyPrint) => JsonUtility.ToJson(this, prettyPrint);
        public string ToJson() => ToJson(false);
        public static DataState FromJson(string json) => ParseHelper.ParseJson(json, new DataState());
        #endregion
    }
}