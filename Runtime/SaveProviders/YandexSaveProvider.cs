using System;
using Kaynir.Saves.Tools;
using Kaynir.Yandex;
using UnityEngine;

namespace Kaynir.Saves.Providers
{
    public class YandexSaveProvider : SaveProvider
    {
        public override void Load<T>(Action<T> onComplete)
        {
            YandexSDK.Instance.LoadData((jsonData) => 
            {
                T data = ParseHelper.ParseJson(jsonData, new T());
                onComplete?.Invoke(data);
            });
        }

        public override void Save<T>(T data, Action onComplete)
        {
            YandexSDK.Instance.SaveData(JsonUtility.ToJson(data),
                                        onComplete);
        }
    }
}