using System;

namespace Kaynir.Saves.Storages
{
    public interface ICloudService
    {
        event Action<string> DataLoaded;
        event Action<bool> DataSaved;

        bool IsAvailable { get; }

        void LoadData();
        void SaveData(string data);
    }
}