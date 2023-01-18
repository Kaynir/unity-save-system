namespace Kaynir.Saves.Saveables
{
    public interface ISaveable
    {
        void CaptureState(SaveState state);
        void RestoreState(SaveState state);
    }
}