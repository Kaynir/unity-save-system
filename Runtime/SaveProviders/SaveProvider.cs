using UnityEngine;

namespace Kaynir.AdvancedSaveSystem
{
    public abstract class SaveProvider : MonoBehaviour
    {
        public abstract void Save<T>(T data);

        public abstract T Load<T>(T defaultData = default);
    }
}