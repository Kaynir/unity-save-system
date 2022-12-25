using System;
using UnityEngine;

namespace Kaynir.Saves
{
    [Serializable]
    public class SaveState
    {
        [SerializeField] private float _playTime;
        [SerializeField] private SerializableDictionary<string, string> _data;

        public SaveState()
        {
            _playTime = 0f;
            _data = new SerializableDictionary<string, string>();
        }

        public void UpdatePlayTime()
        {
            _playTime += Time.time;
        }

        public T GetData<T>(string key) where T : new()
        {
            try
            {
                return JsonUtility.FromJson<T>(_data[key]);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to retrieve data from [{key}] with exception: {ex}.");
                return new T();
            }
        }

        public T GetData<T>() where T : new() => GetData<T>(GetKey<T>());

        public void SetData<T>(T data, string key)
        {
            _data[key] = JsonUtility.ToJson(data);
        }

        public void SetData<T>(T data) => SetData(data, GetKey<T>());

        private string GetKey<T>() => typeof(T).FullName;
    }
}