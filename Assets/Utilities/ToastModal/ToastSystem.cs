using UnityEngine;

namespace Utilities.ToastModal
{
    public class ToastSystem : SingletonMonoBehaviour<ToastSystem>
    {
        [SerializeField] private Toast toast;
        [SerializeField] private ToastPool toastPool;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            toastPool = Instantiate(toastPool, transform);
            toastPool.Init(toast, 5);
        }

        public static void Show(string message, float duration = 1f) =>
            Instance.toastPool.GetObject().Show(message, duration);
    }
}