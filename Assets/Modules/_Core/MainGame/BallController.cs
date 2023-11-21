using Mirror;
using UnityEngine;

namespace MainGame
{
    public class BallController : NetworkBehaviour
    {
        public float startSpeed = 2f;
        public Vector2 offset = new(0, 0);

        public float speed = 2f;
        private Rigidbody2D rigidbody2d;

        public override void OnStartServer()
        {
            TryGetComponent(out rigidbody2d);
            rigidbody2d.simulated = true;
        }

        public void ResetBallPosition(Transform parentTransform)
        {
            transform.SetParent(parentTransform);
            transform.position = parentTransform.position + (Vector3)offset;
        }

        public void StartBallService()
        {
            var startVelocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            rigidbody2d.velocity = startVelocity * startSpeed;
            transform.SetParent(null);
        }

        public void SetSpeed(float speed) => rigidbody2d.velocity *= speed;

        private static float HitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight) =>
            (ballPos.y - racketPos.y) / racketHeight;

        [ServerCallback]
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!col.transform.GetComponent<PaddleComponent>()) return;
            var y = HitFactor(transform.position,
                col.transform.position,
                col.collider.bounds.size.y);
            float x = col.relativeVelocity.x > 0 ? 1 : -1;
            var dir = new Vector2(x, y).normalized;
            rigidbody2d.velocity = dir * speed;
        }
    }
}