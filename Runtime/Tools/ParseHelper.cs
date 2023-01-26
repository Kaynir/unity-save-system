using System;
using UnityEngine;

namespace Kaynir.Saves.Tools
{
    public static class ParseHelper
    {
        public static int ParseInt(string s, int defaultValue)
        {
            if (int.TryParse(s, out int value)) return value;
            return defaultValue;
        }

        public static float ParseFloat(string s, float defaultValue)
        {
            if (float.TryParse(s, out float value)) return value;
            return defaultValue;
        }

        public static T ParseJson<T>(string s, T defaultValue)
        {
            try { return JsonUtility.FromJson<T>(s); }
            catch (Exception ex)
            {
                Debug.Log($"Retriving default value with exception: {ex}.");
                return defaultValue;
            }
        }
    }
}