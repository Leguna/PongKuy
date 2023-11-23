using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorMultiplayerPong
{
    public class JoinPanel : MonoBehaviour
    {
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button leaveServerButton;
        [SerializeField] private TMP_InputField ipAddressField;
        [SerializeField] private TMP_Text ipAddressView;
        [SerializeField] private GameObject panel;

        private Action<string> OnJoinServer;
        private Action OnHostServer;
        private Action OnLeaveServer;
        private Action OnCancel;

        public void Init(Action<string> OnJoinServer, Action OnHostServer, Action OnLeaveServer, Action OnCancel)
        {
            this.OnCancel = OnCancel;
            this.OnJoinServer = OnJoinServer;
            this.OnHostServer = OnHostServer;
            this.OnLeaveServer = OnLeaveServer;
            hostButton.onClick.RemoveAllListeners();
            joinButton.onClick.RemoveAllListeners();
            leaveServerButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
            hostButton.onClick.AddListener(OnHostServer.Invoke);
            joinButton.onClick.AddListener(() => this.OnJoinServer?.Invoke(ipAddressField.text));
            leaveServerButton.onClick.AddListener(OnLeaveServer.Invoke);
            cancelButton.onClick.AddListener(OnCancel.Invoke);
        }

        public void SetIP(string ip)
        {
            ipAddressView.gameObject.SetActive(true);
            ipAddressView.text = $"Local IP Address: {ip}";
        }

        public void Show()
        {
            panel.SetActive(true);
            ipAddressField.text = "";
            // #if UNITY_EDITOR
            ipAddressField.text = "localhost";
            // #endif
            ipAddressView.gameObject.SetActive(false);
            leaveServerButton.gameObject.SetActive(false);
        }

        public void Hide()
        {
            panel.SetActive(false);
            leaveServerButton.gameObject.SetActive(true);
        }

        public void HideAll()
        {
            Hide();
            ipAddressView.gameObject.SetActive(false);
            leaveServerButton.gameObject.SetActive(false);
        }
    }
}