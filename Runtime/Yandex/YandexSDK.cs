using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Kaynir.Yandex
{
    public class YandexSDK : MonoBehaviour
    {
        #region JSLib Methods
        [DllImport("__Internal")]
        private static extern string GetDeviceExtern();

        [DllImport("__Internal")]
        private static extern string GetLanguageExtern();

        [DllImport("__Internal")]
        private static extern void SaveDataExtern(string data);

        [DllImport("__Internal")]
        private static extern void LoadDataExtern();

        [DllImport("__Internal")]
        private static extern void SetLeaderboardExtern(string boardID, int value);

        [DllImport("__Internal")]
        private static extern void ShowFullscreenAdvExtern();

        [DllImport("__Internal")]
        private static extern void ShowRewardedAdvExtern();
        #endregion

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

        public PlayerInfo PlayerInfo { get; private set; }

        private Action _onDataSaved;
        private Action<string> _onDataLoaded;
        private Action<int> _onAdvRewarded;

        public void SaveData(string data, Action onComplete)
        {
            _onDataSaved = onComplete;
            SaveDataExtern(data);
        }

        public void LoadData(Action<string> onComplete)
        {
            _onDataLoaded = onComplete;
            LoadDataExtern();
        }

        public void SetLeaderboard(string boardID, int value)
        {
            SetLeaderboardExtern(boardID, value);
        }

        public void ShowFullscreenAdv()
        {
            ShowFullscreenAdvExtern();
        }

        public void ShowRewardedAdv(Action<int> onRewarded)
        {
            _onAdvRewarded = onRewarded;
            ShowRewardedAdvExtern();
        }

        private void Initialize()
        {
            PlayerInfo = new PlayerInfo()
            {
                deviceType = YandexConsts.GetDevice(GetDeviceExtern()),
                language = YandexConsts.GetLanguage(GetLanguageExtern())
            };
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