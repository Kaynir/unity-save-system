using System;
using Kaynir.Saves.DataStorages;
using Kaynir.Saves.Saveables;

namespace Kaynir.Saves
{
    public static class SaveSystem
    {
        public delegate void StateChanged(SaveState state);

        public static event StateChanged SaveRequested;
        public static event StateChanged StateSaved;
        public static event StateChanged StateLoaded;

        public static SaveState State { get; private set; }
        public static IDataStorage Storage { get; private set; }

        public static void Initialize(IDataStorage storage)
        {
            State = new SaveState();
            Storage = storage;
        }

        public static void Load(SaveState state, Action onComplete)
        {
            OnStateLoaded(state, onComplete);
        }

        public static void Load(Action onComplete)
        {
            Storage.GetData((data) => 
            {
                Load(data, onComplete);
            });
        }

        public static void Load() => Load(null);

        public static void Save(SaveState state, Action onComplete)
        {
            Storage.SetData(state, () =>
            {
                OnStateSaved(onComplete);
            });
        }

        public static void Save(ISaveable saveable, Action onComplete)
        {
            saveable.CaptureState(State);
            Save(State, onComplete);
        }

        public static void Save(Action onComplete = null)
        {
            SaveRequested?.Invoke(State);
            Save(State, onComplete);
        }

        private static void OnStateLoaded(SaveState state, Action onComplete)
        {
            State = state;
            StateLoaded?.Invoke(State);
            onComplete?.Invoke();
        }

        private static void OnStateSaved(Action onComplete)
        {
            StateSaved?.Invoke(State);
            onComplete?.Invoke();
        }
    }
}