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
            AuthenticationService.Instance.SignedIn += async () =>
            {
                try
                {
                    if (isLoading) return;
                    isLoading = true;
                    var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
                    ToastSystem.Show($"Player signed in. Welcome! {playerName}", 3f);
                    onSignedIn?.Invoke();
                    isLoading = false;
                }
                catch (Exception e)
                {
                    ToastSystem.Show(e.Message, 3f);
                    isLoading = false;
                }
            };

            AuthenticationService.Instance.SignInFailed += (err) => { ToastSystem.Show(err.Message, 3f); };

            AuthenticationService.Instance.SignedOut += () =>
            {
                ToastSystem.Show("Player signed out.", 3f);
                this.onSignedOut?.Invoke();
            };

            AuthenticationService.Instance.Expired += () =>
            {
                ToastSystem.Show("Player session could not be refreshed and expired.", 3f);
            };

            await IsSignedIn();
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
                ToastSystem.Show(ex.Message, 3f);
            }
            catch (RequestFailedException ex)
            {
                ToastSystem.Show(ex.Message, 3f);
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task IsSignedIn()
        {
            if (!AuthenticationService.Instance.SessionTokenExists) return;

            try
            {
                if (isLoading) return;
                isLoading = true;
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
                ToastSystem.Show($"Player signed in. Welcome! {playerName}", 3f);
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
                onSignedIn?.Invoke();
                isLoading = false;
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
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
                ToastSystem.Show(ex.Message, 3f);
            }
            catch (RequestFailedException ex)
            {
                ToastSystem.Show(ex.Message, 3f);
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
                ToastSystem.Show(e.Message, 3f);
            }
        }
    }
}