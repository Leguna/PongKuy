using System;
using MirrorMultiplayerPong;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MyNetworkManager networkManager;
        private PaddleSpawner paddleSpawner;
        private BallController ballController;

        [SerializeField] private Button backButton;

        public void Init(bool isMultiplayer, Action OnCancel = null)
        {
            if (isMultiplayer)
                MyNetworkManager.sin.Init(OnCancel);
            else
                StartSinglePlayer(OnCancel);
        }

        private void StartSinglePlayer(Action onCancel)
        {
            backButton.onClick.AddListener(() =>
            {
                onCancel?.Invoke();
                Destroy(paddleSpawner.gameObject);
                Destroy(ballController.gameObject);
            });
            var paddleSpawnerPrefab = Resources.Load<PaddleSpawner>("Prefabs/PaddleSpawner");
            paddleSpawner = Instantiate(paddleSpawnerPrefab);
            paddleSpawner.SpawnPaddleSinglePlayer();
            ballController = Instantiate(Resources.Load<BallController>("Prefabs/Ball"));
            ballController.Init(false);
            ballController.ResetBallPosition(paddleSpawner.paddles[0].transform, PlayerType.Left);
            paddleSpawner.paddles[0].onService += ballController.StartBallService;
        }
    }
}