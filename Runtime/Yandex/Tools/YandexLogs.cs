using Kaynir.Yandex.Enums;
using UnityEngine;

namespace Kaynir.Yandex.Tools
{
    public static class YandexLogs
    {
        public static void LogStatus()
        {
            Debug.Log($"Yandex SDK status: {YandexSDK.Status}.");
        }

        public static void LogDataSaved()
        {
            Debug.Log("Data saved with Yandex SDK.");
        }

        public static void LogDataLoaded()
        {
            Debug.Log("Data loaded with Yandex SDK.");
        }

        public static void LogRewardedVideoResult(RewardResult result)
        {
            Debug.Log($"Video adv rewarded with result: {result}.");
        }

        public static void LogStorageSetWarning()
        {
            Debug.LogWarning("Using default storage to set data.");
        }

        public static void LogStorageGetWarning()
        {
            Debug.LogWarning("Using default storage to get data.");
        }

        public static void LogLeaderboardsWarning()
        {
            Debug.LogWarning($"Unable to access leaderboards.");
        }

        public static void LogFullscreenAdvWarning()
        {
            Debug.LogWarning($"Unable to access fullscreen adv.");
        }
    }
}