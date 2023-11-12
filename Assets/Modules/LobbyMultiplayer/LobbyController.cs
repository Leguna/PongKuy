using System;
using System.Threading.Tasks;
using AuthModule;
using ToC;
using UnityEngine;
using Utilities;

namespace LobbyMultiplayer
{
    public class LobbyController : SingletonMonoBehaviour<LobbyController>
    {
        [SerializeField] private ToCController toCController;
        [SerializeField] private AuthComponent authComponent;
        [SerializeField] private GameObject lobbyPanel;
        private Action closeLobby;

        private void Close()
        {
            lobbyPanel.SetActive(false);
        }

        public void Init(Action closeLobby)
        {
            this.closeLobby = () =>
            {
                closeLobby?.Invoke();
                Close();
            };
            toCController.Init(OnToCAgreement);
        }

        private async void OnToCAgreement(bool result)
        {
            if (result)
            {
                await authComponent.Init(OnLoggedIn, closeLobby);
            }
            else
            {
                closeLobby?.Invoke();
            }
        }

        private void OnLoggedIn(string obj)
        {
            lobbyPanel.SetActive(true);
            authComponent.SetState(AuthComponent.AuthState.None);
        }

        private void OnLoggedOut()
        {
            lobbyPanel.SetActive(false);
            authComponent.SetState(AuthComponent.AuthState.SignIn);
        }
    }
}