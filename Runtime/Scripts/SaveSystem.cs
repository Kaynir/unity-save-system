using System;
using Kaynir.Saves.Storages;

namespace Kaynir.Saves
{
    public class SaveSystem
    {
        public event Action<DataState> StateLoaded;
        public event Action<DataState> StateSaveRequested;
        public event Action<DataState, bool> StateSaved;

        private DataState dataState;
        private IStorageService storageService;

        public SaveSystem(IStorageService storage)
        {
            dataState = new DataState();
            storageService = storage;
        }

        public void SubscribeSaveable(ISaveableEntity saveable)
        {
            saveable.RestoreState(dataState);

            StateLoaded += saveable.RestoreState;
            StateSaveRequested += saveable.CaptureState;
        }

        public void UnsubscribeSaveable(ISaveableEntity saveable)
        {
            StateLoaded -= saveable.RestoreState;
            StateSaveRequested -= saveable.CaptureState;
        }

        public void LoadState(DataState state, Action<DataState> onComplete)
        {
            dataState = state;
            onComplete?.Invoke(state);
            StateLoaded?.Invoke(state);
        }

        public void LoadState(Action<DataState> onComplete)
        {
            storageService.GetData((data) =>
            {
                DataState state = DataState.FromJson(data);
                LoadState(state, onComplete);
            });
        }

        public void LoadState() => LoadState(null);

        public void SaveState(DataState state, Action<bool> onComplete)
        {
            StateSaveRequested?.Invoke(state);

            dataState = state;
            storageService.SetData(state.ToJson(true), (result) =>
            {
                onComplete?.Invoke(result);
                StateSaved?.Invoke(state, result);
            });
        }

        public void SaveState(Action<bool> onComplete) => SaveState(dataState, onComplete);
        public void SaveState() => SaveState(null);
    }
}