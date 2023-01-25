namespace Kaynir.Saves.Saveables
{
    public class SaveableFloat : SaveableVariable<float>
    {
        public override void CaptureState(SaveState state)
        {
            state.SetData(_uniqueID, Value);
        }

        public override void RestoreState(SaveState state)
        {
            LoadValue(state.GetData(_uniqueID, _defaultValue));
        }
    }
}