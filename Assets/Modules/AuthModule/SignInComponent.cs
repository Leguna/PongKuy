using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AuthModule
{
    public class SignInComponent : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameField;
        [SerializeField] private TMP_InputField passwordField;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button signUpButton;
        [SerializeField] private GameObject panel;

        private Action closeSignIn;
        private Action signUp;

        public delegate void SignInSubmit(string username, string password);

        private SignInSubmit signInSubmit;

        public void Init()
        {
#if UNITY_EDITOR
            usernameField.text = "arksana";
            passwordField.text = "Testing!23";
#endif
            submitButton.onClick.AddListener(() =>
                signInSubmit?.Invoke(usernameField.text, passwordField.text));
            closeButton.onClick.AddListener(() => closeSignIn?.Invoke());
            signUpButton.onClick.AddListener(() => signUp?.Invoke());
            Close();
        }

        public void Open() => panel.SetActive(true);
        public void Close() => panel.SetActive(false);

        public void SetListener(SignInSubmit signInSubmit, Action closeSignIn, Action signUp)
        {
            this.signInSubmit = signInSubmit;
            this.closeSignIn = closeSignIn;
            this.signUp = signUp;
        }
    }
}