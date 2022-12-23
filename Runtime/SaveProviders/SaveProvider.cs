using UnityEngine;

namespace Kaynir.Saves
{
    public abstract class SaveProvider : MonoBehaviour
    {
        public delegate void OnSaveCompleted();
        public delegate void OnLoadCompleted<T>(T data);

        public abstract void Save<T>(T data, OnSaveCompleted onCompleted);
        public abstract void Load<T>(OnLoadCompleted<T> onCompleted) where T : new();
    }
}