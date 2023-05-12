using Kaynir.Yandex.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Kaynir.Yandex.Modules
{
    public class VideoAdvListener : MonoBehaviour
    {
        [SerializeField] private UnityEvent _advOpened = new UnityEvent();
        [SerializeField] private UnityEvent _advClosed = new UnityEvent();
        [SerializeField] private UnityEvent<RewardResult> _advRewarded = new UnityEvent<RewardResult>();

        public UnityEvent AdvOpened => _advOpened;
        public UnityEvent AdvClosed => _advClosed;
        public UnityEvent<RewardResult> AdvRewarded => _advRewarded;

        private void OnEnable()
        {
            YandexSDK.VideoAdvOpened += _advOpened.Invoke;
            YandexSDK.VideoAdvClosed += _advClosed.Invoke;
            YandexSDK.VideoAdvRewarded += _advRewarded.Invoke;
        }

        private void OnDisable()
        {
            YandexSDK.VideoAdvOpened -= _advOpened.Invoke;
            YandexSDK.VideoAdvClosed -= _advClosed.Invoke;
            YandexSDK.VideoAdvRewarded -= _advRewarded.Invoke;
        }
    }
}