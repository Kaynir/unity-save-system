using System;
using UnityEngine;

namespace Kaynir.Saves.Tools
{
    public static class ParseHelper
    {
        public static int ParseInt(string s, int defaultValue)
        {
            return int.TryParse(s, out int value)
            ? value
            : defaultValue;
        }
        
        public static int ParseInt(string s) => ParseInt(s, 0);

        public static float ParseFloat(string s, float defaultValue)
        {
            return float.TryParse(s, out float value)
            ? value
            : defaultValue;
        }

        public static float ParseFloat(string s) => ParseFloat(s, 0f);

        public static T ParseJson<T>(string s, T defaultValue)
        {
            try
            {
                if (string.IsNullOrEmpty(s))
                {
                    DebugService.ThrowEmptyStringParseException();
                }

                return JsonUtility.FromJson<T>(s);
            }
            catch (Exception ex)
            {
                DebugService.LogJsonParseException(ex);
                return defaultValue;
            }
        }

        public static T ParseJson<T>(string s) where T : new() => ParseJson<T>(s, new T());
    }
}