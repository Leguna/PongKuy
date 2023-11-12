using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LobbyMultiplayer
{
    public class LobbyRoomComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_InputField lobbyName;
        [SerializeField] private TMP_InputField password;
        [SerializeField] private Image isPublicIcon;
        [SerializeField] private Button isPublicButton;
        [SerializeField] private Button updateButton;

        [SerializeField] private TMP_Text leftPlayerName;
        [SerializeField] private TMP_Text rightPlayerName;
        [SerializeField] private Button playButton;
        [SerializeField] private Button leaveButton;

        [SerializeField] private Sprite publicIcon;
        [SerializeField] private Sprite privateIcon;
        private bool isPublic;

        public delegate void OnUpdate(string lobbyName, string password, bool isPublic);

        public void Open(LobbyData lobbyData, OnUpdate onUpdate, Action onPlay,
            Action onLeave)
        {
            updateButton.onClick.AddListener(() => { onUpdate?.Invoke(lobbyName.text, password.text, isPublic); });
            playButton.onClick.AddListener(() => onPlay?.Invoke());
            leaveButton.onClick.AddListener(() => onLeave?.Invoke());
            isPublicButton.onClick.AddListener(() =>
            {
                isPublic = !isPublic;
                isPublicIcon.sprite = isPublic ? publicIcon : privateIcon;
            });

            UpdateData(lobbyData);

            gameObject.SetActive(true);
        }

        private void UpdateData(LobbyData lobbyData)
        {
            title.text = $"Lobby ({lobbyData?.lobbyCode})";
            lobbyName.text = lobbyData?.roomName;
            password.text = lobbyData?.password;
            leftPlayerName.text = lobbyData?.hostName;
            rightPlayerName.text = lobbyData?.guestName;
            isPublic = lobbyData?.isPublic ?? true;
            isPublicIcon.sprite = isPublic ? publicIcon : privateIcon;

            playButton.gameObject.SetActive(lobbyData?.isPlaying ?? true);
            updateButton.gameObject.SetActive(!lobbyData?.isPlaying ?? true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}