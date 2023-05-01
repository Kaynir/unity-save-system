using System;
using Kaynir.Yandex;
using Kaynir.Yandex.Enums;

namespace Kaynir.Saves.DataStorages
{
    public class YandexDataStorage : IDataStorage
    {
        private IDataStorage _defaultStorage;

        private Action<string> _onGetDataComplete;
        private Action _onSetDataComplete;

        public YandexDataStorage(IDataStorage defaultStorage)
        {
            _defaultStorage = defaultStorage;
        }

        public YandexDataStorage() : this(new ApplicationDataStorage()) { }

        public void GetData(Action<string> onComplete)
        {
            if (YandexSDK.Status != SDKStatus.Active)
            {
                _defaultStorage.GetData(onComplete);
                return;
            }

            _onGetDataComplete = onComplete;

            YandexSDK.DataLoaded += OnDataLoaded;
            YandexSDK.LoadData();
        }

        public void SetData(string data, Action onComplete)
        {
            if (YandexSDK.Status != SDKStatus.Active)
            {
                _defaultStorage.SetData(data, onComplete);
                return;
            }

            _onSetDataComplete = onComplete;

            YandexSDK.DataSaved += OnDataSaved;
            YandexSDK.SaveData(data);
        }

        private void OnDataLoaded(string data)
        {
            YandexSDK.DataLoaded -= OnDataLoaded;

            _onGetDataComplete?.Invoke(data);
            _onGetDataComplete = null;
        }

        private void OnDataSaved()
        {
            YandexSDK.DataSaved -= OnDataSaved;

            _onSetDataComplete?.Invoke();
            _onSetDataComplete = null;
        }
    }
}