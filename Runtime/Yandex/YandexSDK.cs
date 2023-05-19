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

        public static void Initialize(Action onComplete)
        {
            CreateInstance();

            Status = Debug.isDebugBuild
            ? SDKStatus.Debug
            : (SDKStatus)YandexService.GetSDKStatus();

            IsAuthorized = Debug.isDebugBuild ? false : YandexService.GetAuthStatus() == 1;

            Player = Status == SDKStatus.Active
            ? new PlayerInfo(YandexService.GetDevice(), YandexService.GetLanguage())
            : new PlayerInfo(Application.isMobilePlatform, Application.systemLanguage);

            Debug.Log($"Yandex SDK initialized with status: {Status}.");
            Debug.Log($"Player auth status: {IsAuthorized}.");
            onComplete?.Invoke();
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
        public static event Action VideoAdvClosed;
        public static event Action<int> VideoAdvRewarded;

        public static event Action FullscreenAdvOpened;
        public static event Action FullscreenAdvClosed;

        public static SDKStatus Status { get; private set; }
        public static bool IsAuthorized { get; private set; }
        public static PlayerInfo Player { get; private set; }

        public static void LoadData() => YandexService.LoadData();
        public static void SaveData(string data) => YandexService.SaveData(data);

        public static void SetLeaderboard(string id, int value)
        {
            if (!IsAuthorized) return;
            YandexService.SetLeaderboard(id, value);
        }

        public static void ShowFullscreenAdv()
        {
            if (Status == SDKStatus.Debug) return;
            YandexService.ShowFullscreenAdv();
        }

        public static void ShowRewardedAdv(int reward)
        {
            switch (Status)
            {
                case SDKStatus.Debug:
                {
                    _instance.OnVideoAdvOpened();
                    _instance.OnVideoAdvRewarded(reward);
                    _instance.OnVideoAdvClosed();
                    break;
                }
                default: YandexService.ShowRewardedAdv(reward); break;
            }
        }

        private void OnDataLoaded(string data) => DataLoaded?.Invoke(data);
        private void OnDataSaved() => DataSaved?.Invoke();

        private void OnVideoAdvOpened() => VideoAdvOpened?.Invoke();
        private void OnVideoAdvClosed() => VideoAdvClosed?.Invoke();
        private void OnVideoAdvRewarded(int reward) => VideoAdvRewarded?.Invoke(reward);

        private void OnFullscreenAdvOpened() => FullscreenAdvOpened?.Invoke();
        private void OnFullscreenAdvClosed() => FullscreenAdvClosed?.Invoke();
    }
}