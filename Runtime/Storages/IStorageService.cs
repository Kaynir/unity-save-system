using System;

namespace Kaynir.Saves.Storages
{
    public interface IStorageService
    {
        void GetData(Action<string> onComplete);
        void SetData(string data, Action<bool> onComplete);
    }
}