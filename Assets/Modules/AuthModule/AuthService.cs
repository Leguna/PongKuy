using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Utilities.ToastModal;

namespace AuthModule
{
    public class AuthService
    {
        private Action onSignedIn;
        private Action onSignedOut;

        private bool isLoading;

        public async Task Init(Action onSignedIn, Action onSignedOut)
        {
            this.onSignedIn = onSignedIn;
            this.onSignedOut = onSignedOut;

            AuthenticationService.Instance.SignedIn += OnSignedIn;

            AuthenticationService.Instance.SignInFailed += (err) => { ToastSystem.Show(err.Message); };

            AuthenticationService.Instance.SignedOut += () =>
            {
                ToastSystem.Show("Player signed out.");
                this.onSignedOut?.Invoke();
            };

            AuthenticationService.Instance.Expired += () =>
            {
                ToastSystem.Show("Player session could not be refreshed and expired.");
            };

            if (AuthenticationService.Instance.IsSignedIn)
            {
                OnSignedIn();
                return;
            }

            await TrySignedInCached();
        }

        private async void OnSignedIn()
        {
            try
            {
                if (isLoading) return;
                isLoading = true;
                var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
                ToastSystem.Show($"Player signed in. Welcome! {playerName}");
                onSignedIn?.Invoke();
                isLoading = false;
            }
            catch (Exception e)
            {
                ToastSystem.Show(e.Message);
                isLoading = false;
            }
        }

        public async void SignIn(string username, string password)
        {
            if (isLoading) return;
            isLoading = true;
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            }
            catch (AuthenticationException ex)
            {
                ToastSystem.Show(ex.Message);
            }
            catch (RequestFailedException ex)
            {
                ToastSystem.Show(ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task TrySignedInCached()
        {
            if (!AuthenticationService.Instance.SessionTokenExists) return;
            if (AuthenticationService.Instance.IsSignedIn) return;

            try
            {
                if (isLoading) return;
                isLoading = true;
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
                ToastSystem.Show($"Player signed in. Welcome! {playerName}");
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
                onSignedIn?.Invoke();
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }

            isLoading = false;
        }

        public async void SignUp(string displayName, string username, string password)
        {
            if (isLoading) return;
            isLoading = true;
            try
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
                await AuthenticationService.Instance.UpdatePlayerNameAsync(displayName);
            }
            catch (AuthenticationException ex)
            {
                ToastSystem.Show(ex.Message);
            }
            catch (RequestFailedException ex)
            {
                ToastSystem.Show(ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }

        public string GetPlayerName()
        {
            return AuthenticationService.Instance.PlayerName;
        }

        public void SignOut()
        {
            try
            {
                AuthenticationService.Instance.SignOut();
                AuthenticationService.Instance.ClearSessionToken();
                onSignedOut?.Invoke();
            }
            catch (Exception e)
            {
                ToastSystem.Show(e.Message);
            }
        }
    }
}