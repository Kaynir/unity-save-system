using System.Collections.Generic;
using UnityEngine;

namespace Kaynir.Yandex
{
    public static class YandexConsts
    {
        private static Dictionary<string, SystemLanguage> _languages = new Dictionary<string, SystemLanguage>()
        {
            { "ru", SystemLanguage.Russian },
            { "en", SystemLanguage.English },
            { "tr", SystemLanguage.Turkish }
        };

        private static Dictionary<string, DeviceType> _devices = new Dictionary<string, DeviceType>()
        {
            { "desktop", DeviceType.Desktop },
            { "mobile", DeviceType.Mobile },
            { "tablet", DeviceType.Tablet },
            { "tv", DeviceType.TV }
        };

        public static SystemLanguage GetLanguage(string languageID)
        {
            return _languages.ContainsKey(languageID)
            ? _languages[languageID]
            : SystemLanguage.English;
        }

        public static DeviceType GetDevice(string deviceID)
        {
            return _devices.ContainsKey(deviceID)
            ? _devices[deviceID]
            : DeviceType.Desktop;
        }
    }
}