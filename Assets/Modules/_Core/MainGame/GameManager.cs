using System;
using MirrorMultiplayerPong;
using UnityEngine;

namespace MainGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MyNetworkManager networkManager;
        [SerializeField] private PaddleSpawner paddleSpawner;
        [SerializeField] private BallController ballController;

        public void Init(bool isMultiplayer, Action OnCancel = null)
        {
            
            if (isMultiplayer)
                networkManager.Init(OnCancel);
            else
                StartSinglePlayer();
        }

        private void StartSinglePlayer()
        {
            var paddleSpawnerPrefab = Resources.Load<PaddleSpawner>("Prefabs/PaddleSpawner");
            paddleSpawner = Instantiate(paddleSpawnerPrefab);
            paddleSpawner.SpawnPaddleSinglePlayer();
        }
    }
}