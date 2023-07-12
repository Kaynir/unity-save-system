using System;
using Kaynir.Saves.Tools;
using UnityEngine;

namespace Kaynir.Saves.Storages
{
    public class CloudStorage : IStorageService
    {
        private IStorageService localStorage;
        private ICloudService cloudService;

        private Action<string> onGetDataComplete;
        private Action<bool> onSetDataComplete;

        public CloudStorage(IStorageService localStorage, ICloudService cloudService)
        {
            this.localStorage = localStorage;
            this.cloudService = cloudService;
        }

        public void GetData(Action<string> onComplete)
        {
            if (!cloudService.IsAvailable)
            {
                localStorage.GetData(onComplete);
                return;
            }

            onGetDataComplete = onComplete;
            cloudService.DataLoaded += OnCloudDataLoaded;
            cloudService.LoadData();
        }

        public void SetData(string data, Action<bool> onComplete)
        {
            data = UpdatePlayTime(data);

            if (!cloudService.IsAvailable)
            {
                localStorage.SetData(data, onComplete);
                return;
            }

            onSetDataComplete = onComplete;
            cloudService.DataSaved += OnCloudDataSaved;
            cloudService.SaveData(data);
            
            localStorage.SetData(data, null);
        }

        private void OnCloudDataLoaded(string cloudData)
        {
            cloudService.DataLoaded -= OnCloudDataLoaded;

            localStorage.GetData((localData) =>
            {
                string recentData = GetRecentData(cloudData, localData);

                onGetDataComplete?.Invoke(recentData);
                onGetDataComplete = null;
            });
        }

        private void OnCloudDataSaved(bool result)
        {
            cloudService.DataSaved -= OnCloudDataSaved;

            onSetDataComplete?.Invoke(result);
            onSetDataComplete = null;
        }

        private string UpdatePlayTime(string data)
        {
            DataState state = DataState.FromJson(data);

            int playTime = state.GetInt(StorageTools.PLAY_TIME_DATA_KEY);
            state.SetInt(StorageTools.PLAY_TIME_DATA_KEY, playTime + (int)Time.time);

            return state.ToJson(true);
        }

        private string GetRecentData(string cloudData, string localData)
        {
            DataState cloudState = DataState.FromJson(cloudData);
            DataState localState = DataState.FromJson(localData);
            
            string playTimeKey = StorageTools.PLAY_TIME_DATA_KEY;

            return cloudState.GetInt(playTimeKey) > localState.GetInt(playTimeKey)
            ? cloudData
            : localData;
        }
    }
}