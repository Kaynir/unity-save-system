namespace Kaynir.Saves
{
    public interface ISaveableEntity
    {
        void CaptureState(DataState state);
        void RestoreState(DataState state);
    }
}