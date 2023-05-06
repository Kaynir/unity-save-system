using System;
using Kaynir.Yandex.Enums;
using Kaynir.Yandex.Tools;
using UnityEngine;

namespace Kaynir.Yandex
{
    public class YandexSDK : MonoBehaviour
    {
        #region Initialization
        private static YandexSDK _instance;

        public static void Initialize()
        {
            CreateInstance();

            Status = Debug.isDebugBuild
            ? SDKStatus.Debug
            : (SDKStatus)YandexService.GetSDKStatus();

            IsAuthorized = Debug.isDebugBuild ? false : YandexService.GetAuthStatus() == 1;

            PlayerInfo = Status == SDKStatus.Active
            ? new PlayerInfo(YandexService.GetDevice(), YandexService.GetLanguage())
            : new PlayerInfo(Application.isMobilePlatform, Application.systemLanguage);
        }

        private static void CreateInstance()
        {
            _instance = new GameObject().AddComponent<YandexSDK>();
            _instance.name = nameof(YandexSDK);
            DontDestroyOnLoad(_instance.gameObject);
        }
        #endregion

        public static event Action<string> DataLoaded;
        public static event Action DataSaved;

        public static event Action VideoAdvOpened;
        public static event Action<RewardResult> VideoAdvRewarded;

        public static event Action FullscreenAdvOpened;
        public static event Action FullscreenAdvClosed;

        public static SDKStatus Status { get; private set; }
        public static bool IsAuthorized { get; private set; }
        public static PlayerInfo PlayerInfo { get; private set; }

        public static void LoadData() => YandexService.LoadData();
        public static void SaveData(string data) => YandexService.SaveData(data);

        public static void SetLeaderboard(string id, int value)
        {
            if (Status != SDKStatus.Active) return;
            YandexService.SetLeaderboard(id, value);
        }

        public static void ShowFullscreenAdv()
        {
            if (Status != SDKStatus.Active) return;
            YandexService.ShowFullscreenAdv();
        }

        public static void ShowRewardedAdv()
        {
            switch (Status)
            {
                case SDKStatus.Debug: _instance.OnVideoAdvRewarded(1); break;
                case SDKStatus.Inactive: _instance.OnVideoAdvRewarded(-1); break;
                case SDKStatus.Active: YandexService.ShowRewardedAdv(); break;
            }
        }

        private void OnDataLoaded(string data) => DataLoaded?.Invoke(data);
        private void OnDataSaved() => DataSaved?.Invoke();

        private void OnVideoAdvOpened() => VideoAdvOpened?.Invoke();
        private void OnVideoAdvRewarded(int result) => VideoAdvRewarded?.Invoke((RewardResult)result);

        private void OnFullscreenAdvOpened() => FullscreenAdvOpened?.Invoke();
        private void OnFullscreenAdvClosed() => FullscreenAdvClosed?.Invoke();
    }
}