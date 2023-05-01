using System;

namespace Kaynir.Saves.DataStorages
{
    public interface IDataStorage
    {
        void GetData(Action<string> onComplete);
        void SetData(string data, Action onComplete);
    }
}