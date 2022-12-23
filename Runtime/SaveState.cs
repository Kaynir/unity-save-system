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

        public T GetData<T>(string id) where T : new()
        {
            try
            {
                return JsonUtility.FromJson<T>(_data[id]);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to retrieve data from [{id}] with exception: {ex}.");
                return new T();
            }
        }

        public T GetData<T>() where T : new() => GetData<T>(GetHash<T>());

        public void SetData<T>(T data, string id)
        {
            _data[id] = JsonUtility.ToJson(data);
        }

        public void SetData<T>(T data) => SetData(data, GetHash<T>());

        private string GetHash<T>() => Hash128.Compute(typeof(T).FullName).ToString();
    }
}