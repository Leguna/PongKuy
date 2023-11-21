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
        private new Rigidbody2D rigidbody2D;
        public bool isSinglePlayer = true;

        private Camera camera;
        private const int Speed = 10;
        private PlayerType keyPlayerPressed;

        [SerializeField] private float xPosition = 7.5f;

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
            transformIdentity.enabled = true;
            inputAction = new PaddleInputAction();
            inputAction.Enable();

            inputAction.Player.PaddleLeft.performed += MovePaddleLeft;
            inputAction.Player.PaddleLeft.canceled += MovePaddleLeft;
            inputAction.Player.Touch.performed += TouchMove;
            inputAction.Player.Touch.canceled += TouchMove;

            if (isSinglePlayer)
                if (playerType != PlayerType.Right)
                    return;

            inputAction.Player.PaddleRight.performed += MovePaddleRight;
            inputAction.Player.PaddleRight.canceled += MovePaddleRight;
        }

        private void Update()
        {
            // Touch move read all touches 
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

        private void ServiceRight(InputAction.CallbackContext obj)
        {
        }

        private void ServiceLeft(InputAction.CallbackContext obj)
        {
        }

        private void Service()
        {
            Debug.Log("Service");
        }

        private void TouchMove(InputAction.CallbackContext obj)
        {
            // if (obj.valueType != typeof(Vector2) || obj.valueType != typeof(Single))
            // {
            //     rigidbody2D.velocity = Vector2.zero;
            //     return;
            // }
            //
            // var touchPositionVec2 = obj.ReadValue<Vector2>();
            // var touchPosition = Camera.main.ScreenToWorldPoint(touchPositionVec2);
            // var place = touchPosition.x < 0;
            // keyPlayerPressed = place ? PlayerType.Left : PlayerType.Right;
            // var isUp = touchPosition.y > 0;
            //
            // if (!isLocalPlayer && isSinglePlayer)
            //     if (keyPlayerPressed != playerType)
            //         return;
            //
            // if (obj.phase == InputActionPhase.Canceled) rigidbody2D.velocity = Vector2.zero;
            // else rigidbody2D.velocity = isUp ? Vector2.up * Speed : Vector2.down * Speed;
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
            if (!isLocalPlayer && isSinglePlayer)
                if (!isSinglePlayer || playerType != keyPlayerPressed)
                    return;
            rigidbody2D.velocity = Vector2.up * direction * Speed;
        }

        public void SetType(PlayerType oldPlayerType, PlayerType newPlayerType)
        {
            this.playerType = newPlayerType;
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
            base.OnStopClient();
            DisableInput();
            Destroy(gameObject);
        }

        private void DisableInput()
        {
            inputAction.Player.PaddleRight.performed -= MovePaddleRight;
            inputAction.Player.PaddleRight.canceled -= MovePaddleRight;
            inputAction.Player.PaddleLeft.performed -= MovePaddleLeft;
            inputAction.Player.PaddleLeft.canceled -= MovePaddleLeft;
            inputAction.Player.Touch.performed -= TouchMove;
            inputAction.Player.Touch.canceled -= TouchMove;
            inputAction.Disable();
        }

        private void OnDisable()
        {
            DisableInput();
        }
    }
}