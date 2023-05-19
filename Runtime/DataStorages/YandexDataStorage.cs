using System;
using Kaynir.Saves.Saveables;
using Kaynir.Yandex;
using UnityEngine;

namespace Kaynir.Saves.DataStorages
{
    public class YandexDataStorage : IDataStorage
    {
        private const string PLAY_TIME_KEY = "_playTime";

        private IDataStorage _localStorage;

        private Action<SaveState> _onGetDataComplete;
        private Action _onSetDataComplete;

        public YandexDataStorage(IDataStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public YandexDataStorage() : this(new ApplicationDataStorage()) { }

        public void GetData(Action<SaveState> onComplete)
        {
            if (!YandexSDK.Player.isAuthorized)
            {
                _localStorage.GetData(onComplete);
                return;
            }

            _onGetDataComplete = onComplete;

            YandexSDK.DataLoaded += OnCloudDataLoaded;
            YandexSDK.LoadData();
        }

        public void SetData(SaveState data, Action onComplete)
        {
            UpdatePlayTime(data);

            if (!YandexSDK.Player.isAuthorized)
            {
                _localStorage.SetData(data, onComplete);
                return;
            }

            _localStorage.SetData(data, null);
            _onSetDataComplete = onComplete;

            YandexSDK.DataSaved += OnCloudDataSaved;
            YandexSDK.SaveData(data.ToJson());
        }

        private void OnCloudDataLoaded(string data)
        {
            YandexSDK.DataLoaded -= OnCloudDataLoaded;

            _localStorage.GetData((SaveState localData) =>
            {
                SaveState recentData = GetRecentData(localData, SaveState.FromJson(data));

                _onGetDataComplete?.Invoke(recentData);
                _onGetDataComplete = null;
            });
        }

        private void OnCloudDataSaved()
        {
            YandexSDK.DataSaved -= OnCloudDataSaved;

            _onSetDataComplete?.Invoke();
            _onSetDataComplete = null;
        }

        private void UpdatePlayTime(SaveState data)
        {
            float playTime = data.GetFloat(PLAY_TIME_KEY);
            data.SetFloat(PLAY_TIME_KEY, playTime + Time.time);
        }

        private SaveState GetRecentData(SaveState localData, SaveState cloudData)
        {
            return localData.GetFloat(PLAY_TIME_KEY) > cloudData.GetFloat(PLAY_TIME_KEY)
            ? localData
            : cloudData;
        }
    }
}