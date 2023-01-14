using System;
using UnityEngine;

namespace Kaynir.Saves
{
    [Serializable]
    public class SaveState
    {
        [SerializeField] private SerializableDictionary<string, string> _data;

        public SaveState()
        {
            _data = new SerializableDictionary<string, string>();
        }

        public T GetData<T>(string key) where T : new()
        {
            try
            {
                return JsonUtility.FromJson<T>(_data[key]);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to retrieve data for [{key}]. Exception: {ex}.");
                return new T();
            }
        }

        public string GetData(string key, string defaultValue)
        {
            if (!_data.TryGetValue(key, out string data))
            {
                Debug.LogWarning($"String data for [{key}] is missing.");
                data = defaultValue;
            }

            return data;
        }

        public T GetData<T>() where T : new() => GetData<T>(GenerateKey<T>());
        public int GetData(string key, int defaultValue) => ParseHelper.Parse(key, defaultValue);
        public float GetData(string key, float defaultValue) => ParseHelper.Parse(key, defaultValue);

        public void SetData(string key, string data) => _data[key] = data;
        public void SetData<T>(string key, T data) where T : new() => SetData(key, JsonUtility.ToJson(data));
        public void SetData<T>(T data) where T : new() => SetData(GenerateKey<T>(), data);
        public void SetData(string key, int data) => SetData(key, data.ToString());
        public void SetData(string key, float data) => SetData(key, data.ToString());

        private string GenerateKey<T>() => typeof(T).Name;
    }
}