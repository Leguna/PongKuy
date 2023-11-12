using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Constant;
using Cysharp.Threading.Tasks;
using EventStruct;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using Utilities.ToastModal;

namespace LoadingModule
{
    public class LoadingManager : MonoBehaviour
    {
        [SerializeField] private GameObject overlayView;
        [SerializeField] private GameObject fullScreenView;

        private readonly List<LoadingEventData> _tasks = new();
        private Task _loadingTask;

        private bool IsShowing { get; set; }

        private void OnEnable()
        {
            EventManager.AddEventListener<LoadingEventData>(OnAddTask);
        }

        private void OnDisable()
        {
            EventManager.RemoveEventListener<LoadingEventData>(OnAddTask);
        }

        private void OnAddTask(LoadingEventData data)
        {
            Task.Run(async () => { await AddTask(data); });
        }

        private void Show(LoadingType loadingType = LoadingType.None)
        {
            IsShowing = true;
            switch (loadingType)
            {
                case LoadingType.FullScreen:
                    fullScreenView.SetActive(true);
                    overlayView.SetActive(false);
                    break;
                case LoadingType.Overlay:
                    fullScreenView.SetActive(false);
                    overlayView.SetActive(true);
                    break;
                case LoadingType.None:
                default:
                    fullScreenView.SetActive(false);
                    overlayView.SetActive(false);
                    break;
            }
        }

        private void Hide()
        {
            IsShowing = false;
            overlayView.SetActive(false);
            fullScreenView.SetActive(false);
        }

        public async Task<bool> AddTask(LoadingEventData task)
        {
            if (!IsShowing) Show(task.LoadingType);
            var isSuccessful = false;

            try
            {
                _tasks.Add(task);
                await task.Task;
                _tasks.Remove(task);

                task.OnComplete?.Invoke();
                isSuccessful = true;
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogException(e);
#endif
                ToastSystem.Show("Failed to do task " + task.Message);
                _tasks.Remove(task);
            }

            if (_tasks.Count == 0) Hide();
            return isSuccessful;
        }

        public async Task<bool> AddTask<T>(LoadingEventData<T> task)
        {
            if (!IsShowing) Show(task.LoadingType);
            var isSuccessful = false;
            try
            {
                _tasks.Add(task);
                var result = await task.Task;
                _tasks.Remove(task);
                task.OnComplete?.Invoke(result);
                isSuccessful = true;
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogException(e);
#endif
                ToastSystem.Show("Failed to do task " + task.Message);
                _tasks.Remove(task);
            }

            if (_tasks.Count == 0) Hide();
            return isSuccessful;
        }

        public Task LoadScene(SceneConst.SceneName sceneName, LoadingType loadingType = LoadingType.FullScreen)
        {
            var task = SceneManager.LoadSceneAsync((int)sceneName, LoadSceneMode.Additive).ToUniTask().AsTask();
            AddTask(new LoadingEventData(
                task,
                loadingType,
                message: $"Loading {sceneName}"
            ));
            return task;
        }

        public Task UnloadScene(SceneConst.SceneName sceneName, LoadingType loadingType = LoadingType.FullScreen)
        {
            var task = SceneManager.UnloadSceneAsync((int)sceneName).ToUniTask().AsTask();
            AddTask(new LoadingEventData
            (
                task,
                loadingType,
                message: $"Unloading {sceneName}"
            ));
            return task;
        }

        public enum LoadingType
        {
            FullScreen,
            Overlay,
            None
        }

        public List<LoadingEventData> GetTasks()
        {
            return _tasks;
        }
    }
}