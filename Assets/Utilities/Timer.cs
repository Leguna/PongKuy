using UnityEngine;

namespace Utilities
{
    public class Timer : MonoBehaviour
    {
        public bool isRunning;
        private float timeElapsed;

        private void Update()
        {
            if (isRunning) timeElapsed += Time.deltaTime;
        }

        public void StartTimer()
        {
            isRunning = true;
        }

        public void StopTimer()
        {
            isRunning = false;
        }

        public void ResetTimer()
        {
            timeElapsed = 0;
        }

        public float GetTimeElapsed()
        {
            return timeElapsed;
        }

        public string GetTimeElapsedFormatted()
        {
            return $"{timeElapsed / 3600:00}:{timeElapsed / 60:00}:{timeElapsed % 60:00}";
        }
    }
}