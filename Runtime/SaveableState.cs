using System;
using UnityEngine;

namespace CozyDragon.Saves
{
    [Serializable]
    public struct SaveableState
    {
        [SerializeField] private string _data;

        public SaveableState Save<T>(T data)
        {
            _data = JsonUtility.ToJson(data);
            return this;
        }

        public T Load<T>()
        {
            try
            {
                return JsonUtility.FromJson<T>(_data);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to deserialize {_data} with exception: {ex}.");
                return default;
            }
        }
    }
}