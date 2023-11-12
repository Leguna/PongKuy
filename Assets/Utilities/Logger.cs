using UnityEngine;

namespace Utilities
{
    public static class Logging
    {
        public static Logger combatLogger = new(Debug.unityLogger.logHandler);
        public static Logger playerLogger = new(Debug.unityLogger.logHandler);

        public static void LoadLoggers()
        {
            // Call this function when the game starts
            combatLogger.logEnabled = true;
            playerLogger.logEnabled = true;
        }
    }
}