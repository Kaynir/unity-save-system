using System.Runtime.InteropServices;

namespace Kaynir.Saves.Tools
{
    public static class WebGLService
    {
        [DllImport("__Internal")]
        private static extern void UpdateIndexedDB();

        public static void UpdateDatabase()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            UpdateIndexedDB();
#endif
        }
    }
}