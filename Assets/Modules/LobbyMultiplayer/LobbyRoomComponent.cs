using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LobbyMultiplayer
{
    public class LobbyRoomComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text lobbyName;
        [SerializeField] private TMP_Text password;
        [SerializeField] private Image passwordIcon;
        [SerializeField] private Button updateButton;

        [SerializeField] private TMP_Text leftPlayerName;
        [SerializeField] private TMP_Text rightPlayerName;
        [SerializeField] private Button playButton;

        public void Open(LobbyData lobbyData)
        {
            gameObject.SetActive(true);
            title.text = lobbyData.roomName;
            lobbyName.text = lobbyData.roomName;
            password.text = lobbyData.password;
            passwordIcon.gameObject.SetActive(!string.IsNullOrEmpty(lobbyData.password));
            leftPlayerName.text = lobbyData.hostName;
            rightPlayerName.text = lobbyData.guestName;
            playButton.gameObject.SetActive(lobbyData.isPlaying);
            updateButton.gameObject.SetActive(!lobbyData.isPlaying);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}