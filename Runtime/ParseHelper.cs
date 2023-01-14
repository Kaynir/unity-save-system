using System;
using UnityEngine;

namespace Kaynir.Saves
{
    public static class ParseHelper
    {
        public static int Parse(string s, int defaultValue)
        {
            if (int.TryParse(s, out int value)) return value;

            LogWarning(typeof(int), s);
            return defaultValue;
        }

        public static float Parse(string s, float defaultValue)
        {
            if (float.TryParse(s, out float value)) return value;

            LogWarning(typeof(float), s);
            return defaultValue;
        }

        private static void LogWarning(Type type, string s)
        {
            Debug.LogWarning($"Failed to parse {type} from {s}.");
        }
    }
}