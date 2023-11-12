using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LobbyMultiplayer
{
    public class RoomItemComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text roomName;
        [SerializeField] private TMP_Text hostName;
        [SerializeField] private TMP_Text status;
        [SerializeField] private TMP_Text playerCount;
        [SerializeField] private Image lockIcon;
        [SerializeField] private Button joinButton;

        public void SetData(LobbyData lobbyData, Action<string> onJoin)
        {
            joinButton.onClick.AddListener(() => onJoin?.Invoke(lobbyData.id));
            roomName.text = lobbyData.roomName;
            hostName.text = lobbyData.hostName;
            playerCount.text = $"{lobbyData.playerCount}/{lobbyData.maxPlayerCount}";
            status.text = lobbyData.isPlaying ? "Playing" : "Waiting";
            lockIcon.gameObject.SetActive(lobbyData.isLocked);
        }
    }
}