using System;
using Kaynir.Saves.Tools;
using Kaynir.Yandex;
using UnityEngine;

namespace Kaynir.Saves.Providers
{
    public class YandexSaveProvider : SaveProvider
    {
        [SerializeField] private SaveProvider _offlineSaveProvider = null;

        public override void Load<T>(Action<T> onComplete)
        {
            if (YandexSDK.Instance.ConnectionMode != ConnectionMode.Online)
            {
                _offlineSaveProvider.Load(onComplete);
                return;
            }

            YandexSDK.Instance.LoadData((jsonData) =>
            {
                T data = ParseHelper.ParseJson(jsonData, new T());
                onComplete?.Invoke(data);
            });
        }

        public override void Save<T>(T data, Action onComplete)
        {
            if (YandexSDK.Instance.ConnectionMode != ConnectionMode.Online)
            {
                _offlineSaveProvider.Save(data, onComplete);
                return;
            }

            YandexSDK.Instance.SaveData(JsonUtility.ToJson(data),
                                        onComplete);
        }
    }
}