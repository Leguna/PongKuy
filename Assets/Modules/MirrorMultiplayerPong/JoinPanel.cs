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

        private Action<string> onJoinServer;

        public void Init(Action<string> onJoinServer, Action onHostServer, Action onLeaveServer, Action onCancel)
        {
            this.onJoinServer = onJoinServer;
            hostButton.onClick.RemoveAllListeners();
            joinButton.onClick.RemoveAllListeners();
            leaveServerButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
            hostButton.onClick.AddListener(onHostServer.Invoke);
            joinButton.onClick.AddListener(() => this.onJoinServer?.Invoke(ipAddressField.text));
            leaveServerButton.onClick.AddListener(onLeaveServer.Invoke);
            cancelButton.onClick.AddListener(onCancel.Invoke);
        }

        public void SetIP(string ip)
        {
            ipAddressView.gameObject.SetActive(true);
            ipAddressView.text = $"Local IP Address: {ip}:7777";
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