using Mirror;
using UnityEngine;

namespace MainGame
{
    public class BallController : NetworkBehaviour
    {
        public Vector2 offset = new(2, 0);

        public float startSpeed = 2f;
        public float speed = 4f;
        private Rigidbody2D rigidbody2d;

        private int wallBounceCount = 0;
        private int wallBounceLimit = 3;

        private PlayerType lastPlayerHit;
        private Transform lastPlayerHitTransform;

        public void Init(bool isMultiplayer)
        {
            TryGetComponent(out rigidbody2d);
            if (isMultiplayer)
            {
                rigidbody2d.simulated = isServer;
            }
            else
            {
                rigidbody2d.simulated = true;
            }
        }

        private void Update()
        {
            // Follow paddle
            if (transform.parent)
            {
                var paddleTransform = transform.parent;
                var newOffset = new Vector3(offset.x * (paddleTransform.position.x < 0 ? -1 : 1), offset.y);
                transform.position = paddleTransform.position + newOffset;
            }
        }

        public void ResetBallPosition(Transform paddleTransform, PlayerType playerType)
        {
            lastPlayerHit = playerType;
            lastPlayerHitTransform = paddleTransform;
            var ballTransform = transform;
            ballTransform.SetParent(paddleTransform);
            var newOffset = new Vector3(offset.x * (playerType == PlayerType.Left ? -1 : 1), offset.y);
            ballTransform.position = paddleTransform.position + newOffset;
        }

        public void StartBallService()
        {
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
            OnCollisionWithWall(col);
            OnCollisionWithPaddle(col);
            OnCollisionWithGoal(col);
        }

        private void OnCollisionWithGoal(Collision2D col)
        {
            
        }

        private void OnCollisionWithWall(Collision2D col)
        {
            if (col.gameObject.CompareTag("Wall"))
            {
                wallBounceCount++;
                if (wallBounceCount >= wallBounceLimit)
                {
                    wallBounceCount = 0;
                    rigidbody2d.velocity = Vector2.zero;
                    ResetBallPosition(col.transform, PlayerType.Left);
                }
            }
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