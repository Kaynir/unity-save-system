using System;
using UnityEngine;

namespace Kaynir.Yandex
{
    [Serializable]
    public struct PlayerInfo
    {
        public DeviceType deviceType;
        public SystemLanguage language;

        public PlayerInfo(DeviceType deviceType, SystemLanguage language)
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
            this.deviceType = isMobile ? DeviceType.Mobile : DeviceType.Desktop;
            this.language = language;
        }
    }
}