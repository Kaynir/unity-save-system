namespace Kaynir.Saves
{
    public interface ISaveable
    {
        void CaptureState(SaveState state);
        void RestoreState(SaveState state);
    }
}