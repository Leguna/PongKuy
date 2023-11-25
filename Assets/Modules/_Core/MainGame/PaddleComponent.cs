using System;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace MainGame
{
    public class PaddleComponent : NetworkBehaviour
    {
        private PaddleInputAction inputAction;
        private NetworkTransformReliable transformIdentity;
        [SyncVar(hook = nameof(SetType))] public PlayerType playerType;
        private SpriteRenderer spriteRenderer;
        [SerializeField] private new Rigidbody2D rigidbody2D;
        [SyncVar] public bool isSinglePlayer;

        private new Camera camera;
        private const int Speed = 10;
        private PlayerType keyPlayerPressed;

        public Action<PlayerType> onService;
        public Action<PaddleComponent> onDisconnect;

        [SerializeField] private float xPosition = 7.5f;

        public void Init(PlayerType playerType, bool isSinglePlayer = false, Action<PlayerType> onService = null,
            Action<PaddleComponent> onDisconnect = null)
        {
            this.onService = onService;
            this.onDisconnect = onDisconnect;
            name = $"{playerType} Paddle";
            SetType(playerType, playerType);
            this.isSinglePlayer = isSinglePlayer;
        }

        private void Awake()
        {
            camera = Camera.main;
            transformIdentity = GetComponent<NetworkTransformReliable>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            EnhancedTouchSupport.Enable();
        }

        public void SetUpController()
        {
            // Don't set up controller if it's not local player
            if (!isLocalPlayer && !isSinglePlayer) return;

            transformIdentity.enabled = true;
            inputAction = new PaddleInputAction();
            inputAction.Enable();

            inputAction.Player.PaddleLeft.performed += MovePaddleLeft;
            inputAction.Player.PaddleLeft.canceled += MovePaddleLeft;
            inputAction.Player.ServiceLeft.performed += ServiceLeft;

            inputAction.Player.PaddleRight.performed += MovePaddleRight;
            inputAction.Player.PaddleRight.canceled += MovePaddleRight;
            inputAction.Player.ServiceRight.performed += ServiceRight;
        }

        private void Update()
        {
            foreach (var touch in Touch.activeTouches)
            {
                if (touch.phase is TouchPhase.Moved or TouchPhase.Stationary) continue;
                var touchPosition = camera.ScreenToWorldPoint(touch.screenPosition);
                var isUp = touchPosition.y > 0;
                var place = touchPosition.x < 0;
                keyPlayerPressed = place ? PlayerType.Left : PlayerType.Right;

                if (!isLocalPlayer && isSinglePlayer)
                    if (keyPlayerPressed != playerType)
                        return;

                if (touch.phase == TouchPhase.Ended) rigidbody2D.velocity = Vector2.zero;
                else rigidbody2D.velocity = isUp ? Vector2.up * Speed : Vector2.down * Speed;
            }
        }

        private void ServiceRight(InputAction.CallbackContext obj) => Service(PlayerType.Right);

        private void ServiceLeft(InputAction.CallbackContext obj) => Service(PlayerType.Left);

        private void Service(PlayerType playerType)
        {
            if (this.playerType != playerType && isSinglePlayer) return;
            onService?.Invoke(playerType);
            if (!isSinglePlayer) NetworkClient.Send(new ServiceMessage(this.playerType));
        }

        private void MovePaddleLeft(InputAction.CallbackContext obj)
        {
            keyPlayerPressed = PlayerType.Left;
            MovePaddle(obj.ReadValue<float>());
        }

        private void MovePaddleRight(InputAction.CallbackContext obj)
        {
            keyPlayerPressed = PlayerType.Right;
            MovePaddle(obj.ReadValue<float>());
        }

        private void MovePaddle(float direction)
        {
            if (!isLocalPlayer && !isSinglePlayer) return;
            if (isSinglePlayer && playerType != keyPlayerPressed) return;
            rigidbody2D.velocity = Vector2.up * direction * Speed;
        }

        public void SetType(PlayerType oldPlayerType, PlayerType newPlayerType)
        {
            playerType = newPlayerType;
            var path = $"Sprites/paddle{newPlayerType}";
            spriteRenderer.sprite = Resources.Load<Sprite>(path);
            spriteRenderer.size = new Vector2(1, 1);
            if (newPlayerType != PlayerType.Right) return;
            var paddleTransform = transform;
            var position = paddleTransform.position;
            position = new Vector3(xPosition, position.y, position.z);
            paddleTransform.position = position;
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            SetUpController();
        }

        public override void OnStopClient()
        {
            DisableInput();
            onDisconnect?.Invoke(this);
        }

        private void DisableInput()
        {
            if (!isLocalPlayer && !isSinglePlayer) return;
            if (inputAction == null) return;
            inputAction.Player.PaddleRight.performed -= MovePaddleRight;
            inputAction.Player.PaddleRight.canceled -= MovePaddleRight;
            inputAction.Player.PaddleLeft.performed -= MovePaddleLeft;
            inputAction.Player.PaddleLeft.canceled -= MovePaddleLeft;
            inputAction.Player.ServiceLeft.performed -= ServiceLeft;
            inputAction.Player.ServiceRight.performed -= ServiceRight;
            inputAction.Disable();
        }

        private void OnDisable() => DisableInput();
    }

    public struct ServiceMessage : NetworkMessage
    {
        public readonly PlayerType playerType;

        public ServiceMessage(PlayerType playerType)
        {
            this.playerType = playerType;
        }
    }
}