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
        private Action onCancel;

        public void Init(bool isMultiplayer, Action OnCancel = null)
        {
            onCancel = OnCancel;
            if (isMultiplayer)
                MyNetworkManager.sin.Init(OnCancel);
            else
                StartSinglePlayer(OnCancel);
        }

        private void StartSinglePlayer(Action onCancel)
        {
            var paddleSpawnerPrefab = Resources.Load<PaddleSpawner>("Prefabs/PaddleSpawner");
            paddleSpawner = Instantiate(paddleSpawnerPrefab);
            paddleSpawner.SpawnPaddleSinglePlayer();
            ballController = Instantiate(Resources.Load<BallController>("Prefabs/Ball"));
            ballController.Init(false);
            ballController.ResetBallPosition(paddleSpawner.paddles[0].transform, PlayerType.Left);
            paddleSpawner.paddles[0].onService += ballController.StartBallService;

            backButton.gameObject.SetActive(true);
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(OnExitSinglePlayer);
        }

        private void OnExitSinglePlayer()
        {
            onCancel?.Invoke();
            paddleSpawner.DestroyAll();
            Destroy(ballController.gameObject);
            backButton.gameObject.SetActive(false);
        }
    }
}