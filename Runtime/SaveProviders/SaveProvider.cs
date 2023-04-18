using System;
using UnityEngine;

namespace Kaynir.Saves.Providers
{
    public abstract class SaveProvider : MonoBehaviour
    {
        public abstract void Save<T>(T data, Action onComplete);
        public abstract void Load<T>(Action<T> onComplete) where T : new();
    }
}