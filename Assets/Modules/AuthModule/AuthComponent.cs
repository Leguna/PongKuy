using System;
using System.Threading.Tasks;
using UnityEngine;

namespace AuthModule
{
    public class AuthComponent : MonoBehaviour
    {
        [SerializeField] private SignInComponent signInComponent;
        [SerializeField] private SignUpComponent signUpComponent;
        [SerializeField] private GameObject authBg;

        public AuthState authState = AuthState.SignIn;
        private AuthService authService;
        private Action<string> onLoggedIn;
        private Action cancelAuth;

        public async Task Init(Action<string> onLoggedIn, Action cancelAuth)
        {
            this.cancelAuth = cancelAuth;
            authBg.SetActive(true);
            signInComponent.Init();
            signUpComponent.Init();
            signInComponent.SetListener(Login, () =>
            {
                SetState(AuthState.None);
                cancelAuth?.Invoke();
            }, () => SetState(AuthState.SignUp));
            signUpComponent.SetListener(Register, () => SetState(AuthState.SignIn));
            this.onLoggedIn = onLoggedIn;
            SetState(AuthState.SignIn);
            authService = new AuthService();
            await authService.Init(OnSignedIn, OnLoggedOut);
        }

        private void OnLoggedOut()
        {
            cancelAuth?.Invoke();
        }

        private void OnSignedIn()
        {
            onLoggedIn?.Invoke(authService.GetPlayerName());
            SetState(AuthState.None);
        }

        private void OnGUI()
        {
            // Logout
            if (GUI.Button(new Rect(10, 10, 100, 30), "Logout"))
            {
                authService.SignOut();
            }
        }


        public void SetState(AuthState newAuthState)
        {
            authState = newAuthState;
            switch (authState)
            {
                case AuthState.SignIn:
                    signInComponent.Open();
                    signUpComponent.Close();
                    break;
                case AuthState.SignUp:
                    signInComponent.Close();
                    signUpComponent.Open();
                    break;
                default:
                case AuthState.None:
                    signInComponent.Close();
                    signUpComponent.Close();
                    authBg.SetActive(false);
                    break;
            }
        }

        private void Login(string username, string password)
        {
            authService.SignIn(username, password);
        }

        private void Register(string displayName, string username, string password)
        {
            authService.SignUp(displayName, username, password);
        }

        public enum AuthState
        {
            None,
            SignIn,
            SignUp
        }
    }
}