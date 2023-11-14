using System.Collections.Generic;
using MainGame;
using Mirror;
using UnityEngine;

public class PaddleSpawner : NetworkBehaviour
{
    public List<PaddleComponent> paddles;

    public override void OnStartServer()
    {
        SpawnPaddle(Random.Range(0, 2) == 0 ? PlayerType.Left : PlayerType.Right);
    }

    public void SpawnPaddle(PlayerType playerType)
    {
        if (!isServer) return;
        var paddle = Instantiate(Resources.Load<PaddleComponent>("Prefabs/Paddle"));
        paddle.SetType(playerType);
        paddles.Add(paddle);
    }
}