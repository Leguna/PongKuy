// using System;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace AuthModule
// {
//     public class SignUpComponent : MonoBehaviour
//     {
//         [SerializeField] private TMP_InputField displayNameField;
//         [SerializeField] private TMP_InputField usernameField;
//         [SerializeField] private TMP_InputField passwordField;
//         [SerializeField] private Button submitButton;
//         [SerializeField] private Button closeButton;
//         [SerializeField] private GameObject panel;
//
//         private Action closeSignUp;
//
//         public delegate void SignUpSubmit(string displayName, string username, string password);
//
//         private SignUpSubmit signUpSubmit;
//
//         public void Init()
//         {
//             submitButton.onClick.AddListener(() =>
//                 signUpSubmit?.Invoke(displayNameField.text, usernameField.text, passwordField.text));
//             closeButton.onClick.AddListener(() => closeSignUp?.Invoke());
//             Close();
//         }
//
//         public void Open() => panel.SetActive(true);
//         public void Close() => panel.SetActive(false);
//
//         public void SetListener(SignUpSubmit signUpSubmit, Action closeSignUp)
//         {
//             this.signUpSubmit = signUpSubmit;
//             this.closeSignUp = closeSignUp;
//         }
//     }
// }