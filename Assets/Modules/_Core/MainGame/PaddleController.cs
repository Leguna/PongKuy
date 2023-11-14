using System.Collections;
using UnityEngine;

namespace MainGame
{
    public enum PlayerType
    {
        Left = 1,
        Right = 2
    };

    public class PaddleController : MonoBehaviour
    {
        private float defaultScale = 2.5f;

        private int defaultSpeed = 4;
        public int speed = 4;

        public KeyCode upKey;
        public KeyCode downKey;
        private Rigidbody2D rig;

        public PlayerType playerType;

        public int timer = 5;

        public void SetPaddleSpriteByPlayerType(PlayerType playerType)
        {
            this.playerType = playerType;
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/paddle{playerType}");
        }

        public void SetPaddleSpeedToDefault() => speed = defaultSpeed;

        public void SetPaddleLengthToDefault() => transform.localScale =
            new Vector3(transform.localScale.x, defaultScale, transform.localScale.z);

        public IEnumerator MultiplyPaddleSpeedByTwo(int timerInSeconds)
        {
            speed = defaultSpeed * 2;
            yield return new WaitForSeconds(timerInSeconds);
            SetPaddleSpeedToDefault();
        }

        public IEnumerator MultiplyPaddleLengthByTwo(int timerInSeconds)
        {
            var localScale = transform.localScale;
            localScale =
                new Vector3(localScale.x, localScale.y * 2, localScale.z);
            transform.localScale = localScale;
            yield return new WaitForSeconds(timerInSeconds);
            SetPaddleLengthToDefault();
        }

        private void Start()
        {
            defaultSpeed = speed;
            defaultScale = transform.localScale.y;
            rig = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            MoveObject(GetInput());
        }

        private Vector2 GetInput()
        {
            if (Input.GetKey(upKey))
            {
                return Vector2.up * speed;
            }
            else if (Input.GetKey(downKey))
            {
                return Vector2.down * speed;
            }

            return Vector2.zero;
        }

        private void MoveObject(Vector2 movement) => rig.velocity = movement;

        public void PowerUpLength(int playerType)
        {
            if (this.playerType == (PlayerType)playerType)
            {
                StartCoroutine(MultiplyPaddleLengthByTwo(timer));
            }
        }

        public void PowerUpSpeed(int playerType)
        {
            if (this.playerType == (PlayerType)playerType)
            {
                StartCoroutine(MultiplyPaddleSpeedByTwo(timer));
            }
        }

        public void SetTimer(int timeInSecond) => timer = timeInSecond;
    }
}