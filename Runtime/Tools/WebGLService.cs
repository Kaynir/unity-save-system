using System.Runtime.InteropServices;

namespace Kaynir.Saves.Tools
{
    public static class WebGLService
    {
        [DllImport("__Internal")]
        public static extern void UpdateIndexedDB();
    }
}