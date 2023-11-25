using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace MainGame
{
    public class PaddleSpawner : NetworkBehaviour
    {
        public List<PaddleComponent> paddles;
        public Transform[] PaddleTransform => paddles.ConvertAll(paddle => paddle.transform).ToArray();

        public override void OnStartServer()
        {
            paddles = new List<PaddleComponent>();
            paddles.Clear();
        }

        public PlayerType GetSpawnType()
        {
            var playerCount = NetworkServer.connections.Count;
            return playerCount switch
            {
                1 => PlayerType.Left,
                2 => PlayerType.Right,
                _ => PlayerType.Spectator
            };
        }

        public PaddleComponent SpawnPaddle(PlayerType type, bool isSinglePlayer, Action<PlayerType> onService = null)
        {
            var paddle = Instantiate(Resources.Load<PaddleComponent>("Prefabs/Paddle"));
            paddles.Add(paddle);
            paddle.Init(type, isSinglePlayer, onService, Remove);
            return paddle;
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            foreach (var paddle in paddles) Destroy(paddle.gameObject);
            paddles.Clear();
        }

        public void SpawnPaddleSinglePlayer(Action<PlayerType> onService = null)
        {
            SpawnPaddle(PlayerType.Left, true, onService);
            SpawnPaddle(PlayerType.Right, true, onService);
            foreach (var paddle in paddles) paddle.SetUpController();
        }

        public void DestroyAll()
        {
            foreach (var paddle in paddles) Destroy(paddle.gameObject);
            paddles.Clear();
            if (gameObject) Destroy(gameObject);
        }

        public void Remove(PaddleComponent paddle)
        {
            paddles.Remove(paddle);
            Destroy(paddle.gameObject);
        }
    }

    public struct CreatePaddleMessage : NetworkMessage
    {
        public readonly PlayerType playerType;

        public CreatePaddleMessage(PlayerType playerType)
        {
            this.playerType = playerType;
        }
    }
}