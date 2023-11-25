using System;
using MirrorMultiplayerPong;
using UnityEngine;
using UnityEngine.UI;

namespace MainGame
{
    public class GameManager : MonoBehaviour
    {
        private PaddleSpawner paddleSpawner;
        private BallComponent ballComponent;
        [SerializeField] private ScoreManager scoreManager;

        [SerializeField] private Button backButton;
        private Action backToMenu;

        public void Init(bool isMultiplayer, Action backToMenu = null)
        {
            this.backToMenu = backToMenu;
            MyNetworkManager.Sin.isMultiplayer = isMultiplayer;
            if (isMultiplayer)
                MyNetworkManager.Sin.Init(backToMenu);
            else
                StartSinglePlayer();
        }

        private void StartSinglePlayer()
        {
            var paddleSpawnerPrefab = Resources.Load<PaddleSpawner>("Prefabs/PaddleSpawner");
            paddleSpawner = Instantiate(paddleSpawnerPrefab);
            ballComponent = Instantiate(Resources.Load<BallComponent>("Prefabs/Ball"));
            scoreManager = Instantiate(Resources.Load<ScoreManager>("Prefabs/ScoreManager"));

            scoreManager.Init(OnGameOver);
            paddleSpawner.SpawnPaddleSinglePlayer(ballComponent.StartBallService);
            ballComponent.Init(false, paddleSpawner.PaddleTransform, PlayerType.Left, scoreManager.AddScore);

            backButton.gameObject.SetActive(true);
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(OnExitSinglePlayer);
        }

        private void OnGameOver(ScoreBoard obj)
        {
            if (MyNetworkManager.Sin.isMultiplayer)
            {
                Debug.Log("Multiplayer Game Over");
            }
            else
            {
                Debug.Log("Single Player Game Over");
            }

            Debug.Log("Game Over P1: " + obj.leftScore.score + " P2: " + obj.rightScore.score);
        }

        private void OnExitSinglePlayer()
        {
            backToMenu?.Invoke();
            paddleSpawner.DestroyAll();
            Destroy(ballComponent.gameObject);
            Destroy(scoreManager.gameObject);
            backButton.gameObject.SetActive(false);
        }
    }
}