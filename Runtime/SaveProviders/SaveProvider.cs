using UnityEngine;

namespace CozyDragon.Saves
{
    public abstract class SaveProvider : MonoBehaviour
    {
        public abstract void Save<T>(T data);

        public abstract T Load<T>();
    }
}