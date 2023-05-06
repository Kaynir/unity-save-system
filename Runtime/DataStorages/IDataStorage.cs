using System;
using Kaynir.Saves.Saveables;

namespace Kaynir.Saves.DataStorages
{
    public interface IDataStorage
    {
        void GetData(Action<SaveState> onComplete);
        void SetData(SaveState data, Action onComplete);
    }
}