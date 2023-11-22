using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

namespace MainGame
{
    public class PaddleSpawner : NetworkBehaviour
    {
        public List<PaddleComponent> paddles;

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

        public PaddleComponent SpawnPaddle(PlayerType type)
        {
            var paddle = Instantiate(Resources.Load<PaddleComponent>("Prefabs/Paddle"));
            paddle.name = $"{type} Paddle";
            paddle.SetType(type, type);
            paddle.SetUpController();
            paddles.Add(paddle);
            return paddle;
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            foreach (var paddle in paddles) Destroy(paddle.gameObject);
            paddles.Clear();
        }

        public void SpawnPaddleSinglePlayer()
        {
            SpawnPaddle(PlayerType.Left);
            SpawnPaddle(PlayerType.Right);
        }

        public void SetServePlayer(PlayerType playerType)
        {
            
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