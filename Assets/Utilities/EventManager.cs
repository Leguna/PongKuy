using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public delegate void EventCallback<in T>(T data);

    public class EventManager : MonoBehaviour
    {
        private static Dictionary<Type, List<Delegate>> _eventDictionary = new();
        private static string _eventSceneName;

        private void Awake()
        {
            _eventSceneName = SceneManager.GetActiveScene().name;
            _eventDictionary = new Dictionary<Type, List<Delegate>>();
        }

        private static void CheckSceneMatches()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName != _eventSceneName)
                throw new Exception("EventManager has not been initialized for the current scene");
        }

        public static void AddEventListener<T>(EventCallback<T> listener)
        {
            CheckSceneMatches();
            if (_eventDictionary.TryGetValue(typeof(T), out var listeners))
            {
                if (!listeners.Contains(listener)) listeners.Add(listener);
            }
            else
            {
                listeners = new List<Delegate>();
                listeners.Add(listener);
                _eventDictionary.Add(typeof(T), listeners);
            }
        }

        public static List<Delegate> GetListenersOfEvent<T>()
        {
            CheckSceneMatches();
            if (_eventDictionary.TryGetValue(typeof(T), out var listeners)) return listeners;

            return null;
        }

        public static void RemoveEventListener<T>(EventCallback<T> listener)
        {
            CheckSceneMatches();
            if (_eventDictionary.TryGetValue(typeof(T), out var listeners)) listeners.Remove(listener);
        }

        public static void RemoveEvent<T>()
        {
            CheckSceneMatches();
            _eventDictionary.Remove(typeof(T));
        }

        public static void RemoveAllEvents()
        {
            CheckSceneMatches();
            _eventDictionary = new Dictionary<Type, List<Delegate>>();
        }

        public static void TriggerEvent<T>(T data)
        {
            CheckSceneMatches();
            if (!_eventDictionary.TryGetValue(typeof(T), out var listeners)) return;
            foreach (var listener in listeners.ToArray())
                listener.DynamicInvoke(data);
        }
    }
}