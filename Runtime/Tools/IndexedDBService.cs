using System.Runtime.InteropServices;

namespace Kaynir.WebGLPlugins.IndexedDB
{
    public static class IndexedDBService
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void RefreshDatabase();
#else
        public static void RefreshDatabase() { }
#endif
    }
}