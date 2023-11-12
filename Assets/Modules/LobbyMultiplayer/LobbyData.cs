using System;

namespace LobbyMultiplayer
{
    [Serializable]
    public class LobbyData
    {
        public string id;
        public string roomName;
        public string hostName;
        public string guestName;
        public int playerCount;
        public int maxPlayerCount;
        public bool isLocked;
        public bool isPlaying;
        public string password;
        public int hostScore;
        public int guestScore;
    }
}