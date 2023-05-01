using UnityEngine;
using UnityEngine.Events;

namespace Kaynir.Yandex.Modules
{
    public class FullscreenAdvListener : MonoBehaviour
    {
        [SerializeField] private UnityEvent _advOpened = new UnityEvent();
        [SerializeField] private UnityEvent _advClosed = new UnityEvent();

        public UnityEvent AdvOpened => _advOpened;
        public UnityEvent AdvClosed => _advClosed;

        private void OnEnable()
        {
            YandexSDK.FullscreenAdvOpened += _advOpened.Invoke;
            YandexSDK.FullscreenAdvClosed += _advClosed.Invoke;
        }

        private void OnDisable()
        {
            YandexSDK.FullscreenAdvOpened -= _advOpened.Invoke;
            YandexSDK.FullscreenAdvClosed -= _advClosed.Invoke;
        }
    }
}