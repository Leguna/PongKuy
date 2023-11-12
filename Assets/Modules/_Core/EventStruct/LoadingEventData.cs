using System;
using System.Threading.Tasks;
using static LoadingModule.LoadingManager;

namespace EventStruct
{
    public class LoadingEventData
    {
        public Task Task;
        public string Message;
        public LoadingType LoadingType;
        public readonly Action OnComplete;

        public LoadingEventData(Task task, LoadingType loadingType = LoadingType.Overlay,
            Action onComplete = null, string message = "")
        {
            Task = task;
            Message = message;
            LoadingType = loadingType;
            OnComplete = onComplete;
        }
    }

    public class LoadingEventData<T> : LoadingEventData
    {
        public new readonly Task<T> Task;
        public new readonly Action<T> OnComplete;

        public LoadingEventData(Task<T> task, LoadingType loadingType = LoadingType.Overlay,
            Action<T> onComplete = null, string message = "") : base(task, loadingType)
        {
            Task = task;
            OnComplete = onComplete;
            Message = message;
            LoadingType = LoadingType.Overlay;
        }
    }
}