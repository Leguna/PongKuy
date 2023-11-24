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
        private BallComponent ballComponent;

        [SerializeField] private Button backButton;
        private Action onCancel;

        public void Init(bool isMultiplayer, Action onCancel = null)
        {
            this.onCancel = onCancel;
            if (isMultiplayer)
                MyNetworkManager.Sin.Init(onCancel);
            else
                StartSinglePlayer();
        }

        private void StartSinglePlayer()
        {
            var paddleSpawnerPrefab = Resources.Load<PaddleSpawner>("Prefabs/PaddleSpawner");
            paddleSpawner = Instantiate(paddleSpawnerPrefab);
            ballComponent = Instantiate(Resources.Load<BallComponent>("Prefabs/Ball"));

            paddleSpawner.SpawnPaddleSinglePlayer(ballComponent.StartBallService);
            ballComponent.Init(false, paddleSpawner.paddles[0].transform, PlayerType.Left);

            backButton.gameObject.SetActive(true);
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(OnExitSinglePlayer);
        }

        private void OnExitSinglePlayer()
        {
            onCancel?.Invoke();
            paddleSpawner.DestroyAll();
            Destroy(ballComponent.gameObject);
            backButton.gameObject.SetActive(false);
        }
    }
}