// using System;
// using TMPro;
// using Unity.Services.Lobbies.Models;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace LobbyMultiplayer
// {
//     public class LobbyList : MonoBehaviour
//     {
//         [SerializeField] private RoomItemComponent roomItemPrefab;
//         [SerializeField] private Transform roomListContainer;
//
//         [SerializeField] private Button backButton;
//         [SerializeField] private Button refreshButton;
//         [SerializeField] private Button quickJoinButton;
//         [SerializeField] private Button createButton;
//
//         [SerializeField] private TMP_InputField lobbyNameInput;
//         [SerializeField] private TMP_InputField passwordInput;
//         [SerializeField] private Button joinButton;
//
//         public delegate void OnJoin(string lobbyId, string password);
//
//         private OnJoin onJoinByCode;
//         private OnJoin onJoinById;
//
//         public void Init(OnJoin onJoinById, OnJoin onJoinByCode, Action onRefresh, Action onQuickJoin, Action onCreate,
//             Action onBack)
//         {
//             this.onJoinByCode = onJoinByCode;
//             this.onJoinById = onJoinById;
//
//             backButton.onClick.RemoveAllListeners();
//             refreshButton.onClick.RemoveAllListeners();
//             quickJoinButton.onClick.RemoveAllListeners();
//             createButton.onClick.RemoveAllListeners();
//             joinButton.onClick.RemoveAllListeners();
//
//             backButton.onClick.AddListener(() => onBack?.Invoke());
//             refreshButton.onClick.AddListener(() => onRefresh?.Invoke());
//             quickJoinButton.onClick.AddListener(() => onQuickJoin?.Invoke());
//             createButton.onClick.AddListener(() => onCreate?.Invoke());
//             joinButton.onClick.AddListener(() => onJoinByCode?.Invoke(lobbyNameInput.text, passwordInput.text));
//         }
//
//         public void UpdateRoomList(Lobby[] lobbyDatas)
//         {
//             foreach (Transform child in roomListContainer)
//             {
//                 Destroy(child.gameObject);
//             }
//
//             foreach (var lobbyData in lobbyDatas)
//             {
//                 var roomItem = Instantiate(roomItemPrefab, roomListContainer);
//                 roomItem.SetData(new LobbyData().FromLobby(lobbyData),
//                     (lobbyId) => onJoinById?.Invoke(lobbyId, passwordInput.text));
//             }
//         }
//
//         public void Close()
//         {
//             gameObject.SetActive(false);
//         }
//
//         public void Open()
//         {
//             gameObject.SetActive(true);
//         }
//     }
// }