using UnityEngine;

namespace Kaynir.Saves.Saveables
{
    public abstract class SaveableBehaviour : MonoBehaviour, ISaveable
    {
        [SerializeField] protected string _uniqueID = string.Empty;

        public abstract void CaptureState(SaveState state);
        public abstract void RestoreState(SaveState state);
    }
}