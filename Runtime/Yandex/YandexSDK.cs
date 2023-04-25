using System;
using UnityEngine;

namespace Kaynir.Yandex
{
    public class YandexSDK : MonoBehaviour
    {
        #region Singleton
        public static YandexSDK Instance
        {
            get
            {
                if (!_instance) CreateInstance();
                return _instance;
            }
        }

        private static YandexSDK _instance;

        private static void CreateInstance()
        {
            _instance = new GameObject().AddComponent<YandexSDK>();
            _instance.name = nameof(YandexSDK);
            _instance.Initialize();
            DontDestroyOnLoad(_instance.gameObject);
        }
        #endregion

        public ConnectionMode ConnectionMode { get; private set; }
        public PlayerInfo PlayerInfo { get; private set; }

        private Action _onDataSaved;
        private Action<string> _onDataLoaded;
        private Action<int> _onAdvRewarded;

        public void SaveData(string data, Action onComplete)
        {
            _onDataSaved = onComplete;
            YandexService.SaveData(data);
        }

        public void LoadData(Action<string> onComplete)
        {
            _onDataLoaded = onComplete;
            YandexService.LoadData();
        }

        public void SetLeaderboard(string id, int value)
        {
            if (ConnectionMode != ConnectionMode.Online)
            {
                Debug.LogWarning($"Can't access leaderboards in mode: {ConnectionMode}.");
                return;
            }

            YandexService.SetLeaderboard(id, value);
        }

        public void ShowFullscreenAdv()
        {
            if (ConnectionMode != ConnectionMode.Online)
            {
                Debug.LogWarning($"Can't show fullscreen adv in mode: {ConnectionMode}.");
                return;
            }

            YandexService.ShowFullscreenAdv();
        }

        public void ShowRewardedAdv(Action<int> onRewarded)
        {
            _onAdvRewarded = onRewarded;

            switch (ConnectionMode)
            {
                default: OnAdvRewarded(1); break;
                case ConnectionMode.Offline: OnAdvRewarded(-1); break;
                case ConnectionMode.Online: YandexService.ShowRewardedAdv(); break;
            }
        }

        private void Initialize()
        {
            ConnectionMode = Debug.isDebugBuild
            ? ConnectionMode.Debug
            : (ConnectionMode)YandexService.GetConnectionStatus();

            PlayerInfo = ConnectionMode == ConnectionMode.Online
            ? new PlayerInfo(YandexService.GetDevice(), YandexService.GetLanguage())
            : new PlayerInfo(Application.isMobilePlatform, Application.systemLanguage);
        }

        private void OnDataSaved()
        {
            Debug.Log("Data saved with Yandex SDK.");
            _onDataSaved?.Invoke();
            _onDataSaved = null;
        }

        private void OnDataLoaded(string data)
        {
            Debug.Log("Data loaded with Yandex SDK.");
            _onDataLoaded?.Invoke(data);
            _onDataLoaded = null;
        }

        private void OnAdvRewarded(int value)
        {
            Debug.Log($"Video adv rewarded with value: {value}.");
            _onAdvRewarded?.Invoke(value);
            _onAdvRewarded = null;
        }
    }
}