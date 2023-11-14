using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MainGame
{
    public class PaddleComponent : NetworkBehaviour
    {
        private PaddleInputAction inputAction;
        private NetworkTransformReliable transformIdentity;
        private PlayerType playerType;
        private SpriteRenderer spriteRenderer;
        private new Rigidbody2D rigidbody2D;

        private void Awake()
        {
            transformIdentity = GetComponent<NetworkTransformReliable>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            SetUpController();
        }

        private void SetUpController()
        {
            inputAction = new PaddleInputAction();
            inputAction.Enable();
            inputAction.Player.PaddleLeft.performed += MovePaddle;
            inputAction.Player.PaddleRight.performed += MovePaddle;
        }

        private void MovePaddle(InputAction.CallbackContext obj)
        {
            if (isLocalPlayer) return;
            var direction = obj.ReadValue<float>();
            rigidbody2D.velocity = Vector2.up * direction * 10;
        }

        public void SetType(PlayerType playerType)
        {
            this.playerType = playerType;
            spriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/paddle{playerType}");
        }
    }
}