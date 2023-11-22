using System;
using MainGame;
using Mirror;
using UnityEngine;
using Utilities;
using Utilities.ToastModal;

namespace MirrorMultiplayerPong
{
    public class MyNetworkManager : NetworkManager
    {
        // Singleton
        public static MyNetworkManager sin => singleton as MyNetworkManager;

        public PaddleSpawner paddleSpawner;
        [SerializeField] private JoinPanel joinPanel;

        private Action OnCancel;

        public void Init(Action OnCancel)
        {
            this.OnCancel = OnCancel;
            joinPanel.Init(OnJoinServer, OnHostServer, OnLeaveServer, OnCancelEvent);
            ShowPanel();
        }

        private void OnCancelEvent()
        {
            joinPanel.Hide();
            OnCancel?.Invoke();
            NetworkServer.Shutdown();
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            joinPanel.Show();
        }

        public override void OnStartHost()
        {
            base.OnStartHost();
            joinPanel.Hide();
            var ipv4 = IPManager.GetIP(ADDRESSFAM.IPv4);
            joinPanel.SetIP(ipv4);
        }

        private void ShowPanel() => joinPanel.Show();

        public override void OnStartServer()
        {
            var paddleSpawnerPrefab = Resources.Load<PaddleSpawner>("Prefabs/PaddleSpawner");
            paddleSpawner = Instantiate(paddleSpawnerPrefab);
            NetworkServer.Spawn(paddleSpawner.gameObject);
            NetworkServer.RegisterHandler<CreatePaddleMessage>(OnCreatePaddle);
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            if (paddleSpawner.paddles.Count != 2) return;
            var ball = Instantiate(Resources.Load<BallController>("Prefabs/Ball"));
            NetworkServer.Spawn(ball.gameObject);
            NetworkServer.AddPlayerForConnection(conn, ball.gameObject);
        }

        private void OnHostServer()
        {
            StartHost();
        }

        private void OnJoinServer(string ip)
        {
            if (ip.Length == 0)
            {
                ToastSystem.Show("Please enter an IP address");
                return;
            }

            joinPanel.Hide();
            networkAddress = ip;
            try
            {
                StartClient();
            }
            catch (Exception e)
            {
                ToastSystem.Show(e.Message);
            }
        }

        private void OnLeaveServer()
        {
            StopClient();
            StopHost();
            joinPanel.Show();
        }

        private void OnCreatePaddle(NetworkConnectionToClient conn, CreatePaddleMessage msg)
        {
            var paddle = paddleSpawner.SpawnPaddle(paddleSpawner.GetSpawnType());
            NetworkServer.Spawn(paddle.gameObject);
            NetworkServer.AddPlayerForConnection(conn, paddle.gameObject);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            var createPaddleMessage = new CreatePaddleMessage(paddleSpawner.GetSpawnType());
            NetworkClient.Send(createPaddleMessage);
        }
    }
}