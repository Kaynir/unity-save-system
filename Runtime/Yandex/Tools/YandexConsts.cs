using System.Collections.Generic;
using UnityEngine;
using Kaynir.Yandex.Enums;

namespace Kaynir.Yandex.Tools
{
    public static class YandexConsts
    {
        private static Dictionary<string, SystemLanguage> _languages = new Dictionary<string, SystemLanguage>()
        {
            { "ru", SystemLanguage.Russian },
            { "en", SystemLanguage.English },
            { "tr", SystemLanguage.Turkish }
        };

        private static Dictionary<string, YaDeviceType> _devices = new Dictionary<string, YaDeviceType>()
        {
            { "desktop", YaDeviceType.Desktop },
            { "mobile", YaDeviceType.Mobile },
            { "tablet", YaDeviceType.Tablet },
            { "tv", YaDeviceType.TV }
        };

        public static SystemLanguage GetLanguage(string languageID)
        {
            return _languages.ContainsKey(languageID)
            ? _languages[languageID]
            : SystemLanguage.English;
        }

        public static YaDeviceType GetDevice(string deviceID)
        {
            return _devices.ContainsKey(deviceID)
            ? _devices[deviceID]
            : YaDeviceType.Desktop;
        }
    }
}