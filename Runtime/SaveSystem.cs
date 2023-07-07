using System;
using Kaynir.Saves.Storages;

namespace Kaynir.Saves
{
    public class SaveSystem
    {
        public delegate void StateAction(DataState state);

        public event StateAction StateLoaded;
        public event StateAction StateSaveRequested;
        public event StateAction StateSaved;

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

        public void LoadState(DataState state, StateAction onComplete)
        {
            dataState = state;
            onComplete?.Invoke(state);
            StateLoaded?.Invoke(state);
        }

        public void LoadState(StateAction onComplete)
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
                StateSaved?.Invoke(state);
            });
        }

        public void SaveState(Action<bool> onComplete) => SaveState(dataState, onComplete);
        public void SaveState() => SaveState(null);
    }
}