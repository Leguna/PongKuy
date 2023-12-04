// using System;
// using System.Collections.Generic;
// using AuthModule;
// using ToC;
// using Unity.Services.Lobbies;
// using Unity.Services.Lobbies.Models;
// using UnityEngine;
// using Utilities;
// using Utilities.ToastModal;
//
// namespace LobbyMultiplayer
// {
//     public class LobbyController : SingletonMonoBehaviour<LobbyController>
//     {
//         [SerializeField] private ToCController toCController;
//         [SerializeField] private AuthComponent authComponent;
//         [SerializeField] private LobbyList lobbyList;
//         [SerializeField] private LobbyRoomComponent lobbyRoom;
//         [SerializeField] private GameObject bg;
//         private Action closeLobby;
//         private LobbyData lobbyData;
//
//         public void Init(Action closeLobby)
//         {
//             bg.SetActive(true);
//             lobbyList.gameObject.SetActive(true);
//             this.closeLobby = () =>
//             {
//                 closeLobby?.Invoke();
//                 lobbyList.Close();
//                 lobbyRoom.Close();
//                 bg.SetActive(false);
//             };
//             toCController.Init(OnToCAgreement);
//         }
//
//         public void Close()
//         {
//             closeLobby?.Invoke();
//             bg.SetActive(false);
//             lobbyRoom.Close();
//             lobbyList.Close();
//         }
//
//         private async void OnToCAgreement(bool result)
//         {
//             if (result)
//             {
//                 await authComponent.Init(OnLoggedIn, closeLobby);
//             }
//             else
//             {
//                 closeLobby?.Invoke();
//             }
//         }
//
//         private void OnLoggedIn(string obj)
//         {
//             lobbyList.Init(JoinById, JoinByCode, RefreshLobby, QuickJoin, CreateLobby, Close);
//             authComponent.SetState(AuthComponent.AuthState.None);
//             RefreshLobby();
//         }
//
//         private async void CreateLobby()
//         {
//             try
//             {
//                 const string lobbyName = "New Lobby";
//                 const int maxPlayers = 2;
//                 var options = new CreateLobbyOptions
//                 {
//                     IsPrivate = false,
//                     Data = new Dictionary<string, DataObject>
//                     {
//                         { "isPlaying", new DataObject(DataObject.VisibilityOptions.Public, "false") },
//                         {
//                             "hostScore",
//                             new DataObject(DataObject.VisibilityOptions.Public, "0", DataObject.IndexOptions.N2)
//                         },
//                         {
//                             "guestScore",
//                             new DataObject(DataObject.VisibilityOptions.Public, "0", DataObject.IndexOptions.N3)
//                         }
//                     }
//                 };
//                 var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
//                 lobbyData = new LobbyData().FromLobby(lobby);
//                 OpenLobbyRoom(lobbyData);
//                 StartHearthBeat();
//             }
//             catch (Exception e)
//             {
//                 ToastSystem.Show("Failed to create lobby");
//                 Debug.LogException(e);
//             }
//         }
//
//         private void OpenLobbyRoom(LobbyData lobbyData)
//         {
//             lobbyList.Close();
//             lobbyRoom.Open(lobbyData, UpdateLobby, PlayGame, LeaveRoom);
//         }
//
//         private void PlayGame()
//         {
//             ToastSystem.Show("Play");
//         }
//
//         private void UpdateLobby(string lobbyName, string password, bool isPublic)
//         {
//             try
//             {
//                 var options = new UpdateLobbyOptions
//                 {
//                     Name = lobbyName,
//                     IsPrivate = !isPublic,
//                     Password = password
//                 };
//                 LobbyService.Instance.UpdateLobbyAsync(lobbyData.id, options);
//             }
//             catch (Exception e)
//             {
//                 ToastSystem.Show("Failed to update lobby");
//             }
//         }
//
//         private void LeaveRoom()
//         {
//             lobbyRoom.Close();
//             lobbyList.Open();
//             LobbyService.Instance.DeleteLobbyAsync(lobbyData.id);
//             StopHearthBeat();
//         }
//
//         public async void JoinById(string id, string password = "")
//         {
//             try
//             {
//                 var options = new JoinLobbyByIdOptions();
//                 if (password != "")
//                     options.Password                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         = password;
//                 var lobby = await LobbyService.Instance.JoinLobbyByIdAsync(id, options);
//                 lobbyData = new LobbyData().FromLobby(lobby);
//                 OpenLobbyRoom(lobbyData);
//             }
//             catch (Exception e)
//             {
//                 ToastSystem.Show("Failed to join lobby");
//                 Debug.LogError(e);
//             }
//         }
//
//         public async void JoinByCode(string code, string password = "")
//         {
//             try
//             {
//                 var options = new JoinLobbyByCodeOptions();
//                 if (password != "")
//                     options.Password = password;
//                 var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
//                 lobbyData = new LobbyData().FromLobby(lobby);
//                 OpenLobbyRoom(lobbyData);
//             }
//             catch (Exception e)
//             {
//                 ToastSystem.Show("Failed to join lobby");
//                 Debug.LogError(e);
//             }
//         }
//
//         public async void QuickJoin()
//         {
//             try
//             {
//                 var options = new QuickJoinLobbyOptions();
//                 await LobbyService.Instance.QuickJoinLobbyAsync(options);
//             }
//             catch (LobbyServiceException e)
//             {
//                 ToastSystem.Show("Failed to quick join lobby");
//                 Debug.LogException(e);
//             }
//         }
//
//         public async void RefreshLobby()
//         {
//             try
//             {
//                 // Query lobbies that are not full and not started
//                 var options = new QueryLobbiesOptions
//                 {
//                     Filters = new List<QueryFilter>
//                     {
//                         new(
//                             field: QueryFilter.FieldOptions.AvailableSlots,
//                             op: QueryFilter.OpOptions.GT,
//                             value: "0")
//                     }
//                 };
//                 var lobby = await LobbyService.Instance.QueryLobbiesAsync(options);
//                 lobbyList.UpdateRoomList(lobby.Results.ToArray());
//             }
//             catch (Exception e)
//             {
//                 ToastSystem.Show("Failed to refresh lobby list");
//                 Debug.LogException(e);
//             }
//         }
//
//         public void StartHearthBeat() => InvokeRepeating(nameof(SendHeartbeat), 0, 10);
//         public void StopHearthBeat() => CancelInvoke(nameof(SendHeartbeat));
//
//         public async void SendHeartbeat()
//         {
//             try
//             {
//                 Debug.Log("Sending heartbeat");
//                 await LobbyService.Instance.SendHeartbeatPingAsync(lobbyData.id);
//             }
//             catch (Exception e)
//             {
//                 ToastSystem.Show("Failed to heartbeat");
//                 Debug.LogException(e);
//             }
//         }
//
//         public void OnApplicationQuit()
//         {
//             StopHearthBeat();
//             if (lobbyData != null)
//                 LobbyService.Instance.DeleteLobbyAsync(lobbyData.id);
//         }
//     }
// }