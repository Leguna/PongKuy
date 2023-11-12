using System;
using Unity.Services.Lobbies.Models;

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
        public bool isPublic;
        public string password;
        public bool hasPassword;
        public int hostScore;
        public int guestScore;
        public string lobbyCode;

        public LobbyData FromLobby(Lobby lobby)
        {
            id = lobby.Id;
            lobbyCode = lobby.LobbyCode;
            roomName = lobby.Name;
            if (lobby.Data?.TryGetValue("hostName", out var hostNameData) ?? false)
                hostName = hostNameData.Value;
            if (lobby.Data?.TryGetValue("guestName", out var guestNameData) ?? false)
                guestName = guestNameData.Value;
            playerCount = lobby.Players.Count;
            maxPlayerCount = lobby.MaxPlayers;
            isLocked = lobby.IsLocked;
            if (lobby.Data?.TryGetValue("isPlaying", out var isPlayingData) ?? false)
                isPlaying = bool.Parse(isPlayingData.Value);
            isPublic = !lobby.IsPrivate;
            hasPassword = lobby.HasPassword;
            if (lobby.Data?.TryGetValue("hostScore", out var hostScoreData) ?? false)
                hostScore = hostScoreData.Value == "" ? 0 : int.Parse(hostScoreData.Value);
            if (lobby.Data?.TryGetValue("guestScore", out var guestScoreData) ?? false)
                guestScore = guestScoreData.Value == "" ? 0 : int.Parse(guestScoreData.Value);
            return this;
        }
    }
}