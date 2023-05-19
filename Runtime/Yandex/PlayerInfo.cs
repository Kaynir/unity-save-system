using System;
using Kaynir.Yandex.Enums;
using Kaynir.Yandex.Tools;
using UnityEngine;

namespace Kaynir.Yandex
{
    [Serializable]
    public struct PlayerInfo
    {
        public YaDeviceType deviceType;
        public SystemLanguage language;

        public PlayerInfo(YaDeviceType deviceType, SystemLanguage language)
        {
            this.deviceType = deviceType;
            this.language = language;
        }

        public PlayerInfo(string deviceID, string languageID)
        {
            this.deviceType = YandexConsts.GetDevice(deviceID);
            this.language = YandexConsts.GetLanguage(languageID);
        }

        public PlayerInfo(bool isMobile, SystemLanguage language)
        {
            this.deviceType = isMobile ? YaDeviceType.Mobile : YaDeviceType.Desktop;
            this.language = language;
        }
    }
}