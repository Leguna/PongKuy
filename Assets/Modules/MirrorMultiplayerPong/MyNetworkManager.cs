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
        public static MyNetworkManager Sin => singleton as MyNetworkManager;

        private BallComponent ball;
        public PaddleSpawner paddleSpawner;
        public bool isMultiplayer;

        [SerializeField] private JoinPanel joinPanel;

        private Action onCancel;
        [SerializeField] private ScoreManager scoreManager;

        public void Init(Action onCancel, bool isMultiplayer = true)
        {
            this.isMultiplayer = isMultiplayer;
            this.onCancel = onCancel;
            joinPanel.Init(OnJoinServer, OnHostServer, OnLeaveServer, OnCancelEvent);
            ShowPanel();
        }

        private void OnCancelEvent()
        {
            joinPanel.Hide();
            onCancel?.Invoke();
            NetworkServer.Shutdown();
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            NetworkServer.UnregisterHandler<CreatePaddleMessage>();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            if (scoreManager) scoreManager.ShowScore();
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            if (ball) Destroy(ball.gameObject);
            if (scoreManager)
            {
                scoreManager.ResetScore();
                scoreManager.HideScore();
            }
        }

        public override void OnStartHost()
        {
            base.OnStartHost();
            joinPanel.Hide();
            var ipv4 = IPManager.GetLocalIPAddress();
            joinPanel.SetIP(ipv4);
        }

        private void ShowPanel() => joinPanel.Show();

        public override void OnStartServer()
        {
            var paddleSpawnerPrefab = Resources.Load<PaddleSpawner>("Prefabs/PaddleSpawner");
            paddleSpawner = Instantiate(paddleSpawnerPrefab);
            NetworkServer.Spawn(paddleSpawner.gameObject);
            NetworkServer.RegisterHandler<CreatePaddleMessage>(OnCreatePaddle);
            scoreManager = Instantiate(Resources.Load<ScoreManager>("Prefabs/ScoreManager"));
            NetworkServer.Spawn(scoreManager.gameObject);
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
            isMultiplayer = false;
            joinPanel.Show();
        }

        private void OnCreatePaddle(NetworkConnectionToClient conn, CreatePaddleMessage msg)
        {
            var paddle = paddleSpawner.SpawnPaddle(paddleSpawner.GetSpawnType(), false);
            NetworkServer.Spawn(paddle.gameObject);
            NetworkServer.AddPlayerForConnection(conn, paddle.gameObject);
            SpawnBall();
        }

        private void SpawnBall()
        {
            if (paddleSpawner.paddles.Count != 2) return;
            ball = Instantiate(Resources.Load<BallComponent>("Prefabs/Ball"));
            NetworkServer.Spawn(ball.gameObject);
            ball.Init(true, paddleSpawner.PaddleTransform, PlayerType.Left, OnGoal);
        }

        private void OnGoal(PlayerType playerGoal)
        {
            scoreManager.AddScore(playerGoal);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            var createPaddleMessage = new CreatePaddleMessage(paddleSpawner.GetSpawnType());
            NetworkClient.Send(createPaddleMessage);
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            joinPanel.Show();
        }

        private void OnScoreBoardMessage(ScoreBoardMessage obj)
        {
            scoreManager.RpcSyncScore(scoreManager.scoreBoard, obj.scoreBoard);
        }
    }
}