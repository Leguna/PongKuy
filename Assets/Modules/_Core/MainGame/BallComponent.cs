using System;
using System.Collections;
using Mirror;
using MirrorMultiplayerPong;
using UnityEngine;

namespace MainGame
{
    public class BallComponent : NetworkBehaviour
    {
        public Vector2 offset = new(2, 0);

        public float startSpeed = 2f;
        public float speed = 4f;
        private Rigidbody2D rigidbody2d;

        private int wallBounceCount;
        private const int wallBounceLimit = 3;

        private PlayerType lastPlayerHit;
        private Transform lastPlayerHitTransform;
        private float lastBallTouchTime;
        private const float lastBallTouchTimeLimit = 3f;
        private const float delayService = 3;

        private Action<PlayerType> OnGoal;

        public void Init(bool isMultiplayer, Transform paddleTransform, PlayerType playerType)
        {
            TryGetComponent(out rigidbody2d);
            rigidbody2d.simulated = !isMultiplayer || isServer;
            ResetBallPosition(paddleTransform, playerType);
        }

        private void Update()
        {
            CheckBallPosition();
            MoveBallWithPaddle();
        }

        private void MoveBallWithPaddle()
        {
            if (!transform.parent) return;
            var paddleTransform = transform.parent;
            var newOffset = new Vector3(offset.x * (paddleTransform.position.x < 0 ? -1 : 1), offset.y);
            transform.position = paddleTransform.position + newOffset;
        }

        private void CheckBallPosition()
        {
            if (transform.parent) return;
            lastBallTouchTime += Time.deltaTime;
            if (lastBallTouchTime < lastBallTouchTimeLimit) return;
            lastBallTouchTime = 0;
            OnGoal?.Invoke(lastPlayerHit);

            ResetBallPosition(lastPlayerHitTransform, lastPlayerHit);
        }

        public void ResetBallPosition(Transform paddleTransform, PlayerType playerType)
        {
            if (!isServer && MyNetworkManager.Sin.isMultiplayer) return;
            lastPlayerHit = playerType;
            lastPlayerHitTransform = paddleTransform;
            var ballTransform = transform;
            ballTransform.SetParent(paddleTransform);
            var newOffset = new Vector3(offset.x * (playerType == PlayerType.Left ? -1 : 1), offset.y);
            ballTransform.position = paddleTransform.position + newOffset;
            StartCoroutine(DelayedStartBallService(playerType));
        }

        private IEnumerator DelayedStartBallService(PlayerType playerType)
        {
            yield return new WaitForSeconds(delayService);
            StartBallService(playerType);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<ServiceMessage>(RpcServiceBall);
        }

        private void RpcServiceBall(NetworkConnectionToClient conn, ServiceMessage msg)
        {
            if (msg.playerType != lastPlayerHit) return;
            StartBallService(msg.playerType);
        }

        public void StartBallService(PlayerType playerType)
        {
            if (lastPlayerHit != playerType) return;
            if (transform.parent == null) return;
            var isLeft = transform.parent.position.x < 0;
            var startVelocity = new Vector2(isLeft ? 1 : -1, 0);
            rigidbody2d.velocity = startVelocity * startSpeed;
            transform.SetParent(null);
        }

        public void SetSpeed(float speed) => rigidbody2d.velocity *= speed;

        private static float HitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight) =>
            (ballPos.y - racketPos.y) / racketHeight;

        private void OnCollisionEnter2D(Collision2D col)
        {
            lastBallTouchTime = 0;
            OnCollisionWithWall(col);
            OnCollisionWithPaddle(col);
        }

        private void OnCollisionWithWall(Collision2D col)
        {
            if (!col.gameObject.CompareTag("Wall")) return;
            wallBounceCount++;
            if (wallBounceCount < wallBounceLimit) return;
            wallBounceCount = 0;
            rigidbody2d.velocity = Vector2.zero;
            ResetBallPosition(lastPlayerHitTransform, lastPlayerHit);
        }

        private void OnCollisionWithPaddle(Collision2D col)
        {
            if (!col.transform.TryGetComponent<PaddleComponent>(out var paddleTouch)) return;
            wallBounceCount = 0;
            lastPlayerHit = paddleTouch.playerType;
            lastPlayerHitTransform = paddleTouch.transform;
            var y = HitFactor(transform.position,
                col.transform.position,
                col.collider.bounds.size.y);
            float x = col.relativeVelocity.x > 0 ? 1 : -1;
            var dir = new Vector2(x, y).normalized;
            rigidbody2d.velocity = dir * speed;
        }
    }
}