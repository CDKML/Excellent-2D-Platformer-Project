using Assets.Scripts.Enums;
using Assets.Scripts.Mechanics;
using Platformer.Core;
using Platformer.Gameplay;
using Platformer.Model;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class PlayerController : KinematicObject
    {
        [Header("Player Parameters")]
        [SerializeField] private float maxSpeed = 5;
        [SerializeField] private bool controlEnabled = true;


        [Header("Controls")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference toggleInventoryAction;

        [Header("Debugging")]
        [SerializeField] private TextMeshProUGUI lastKeyPressed;
        [SerializeField] private Color lineColor;
        [SerializeField] private GameObject inventoryUIGO;   // Reference to your inventory UI

        private Vector2 move;
        private KeyCode lastKeyCode;

        private Collider2D collider2d;
        private AudioSource audioSource;
        private Health health;
        private SpriteRenderer spriteRenderer;

        private readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private Bounds Bounds => collider2d.bounds;

        [SerializeField] private JumpHandler jumpHandler;

        void Awake()
        {
            InitializeComponents();
            jumpHandler.SetupJumpAction(jumpAction);
            SetupToggleInventoryAction();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                HandleInput();
            }
            else
            {
                move = Vector2.zero;
            }

            jumpHandler.UpdateJumpState();
            base.Update();
            base.FixedUpdate();
        }

        private void InitializeComponents()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void SetupToggleInventoryAction()
        {
            toggleInventoryAction.action.Enable();
            toggleInventoryAction.action.started += ctx => { ToggleInventory(); };
        }

        private void ToggleInventory()
        {
            if (inventoryUIGO.activeSelf)
            {
                inventoryUIGO.SetActive(false);
            }
            else
            {
                inventoryUIGO.SetActive(true);
            }
        }

        private void HandleInput()
        {
            UpdateLastKeyPressed();

            move = moveAction.action.ReadValue<Vector2>();

            jumpHandler.UpdateCoyoteTimer();
            jumpHandler.UpdateJumpBuffer();
            CheckLedgeHop();

            jumpHandler.JumpInputLogic();
        }

        private void UpdateLastKeyPressed()
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        lastKeyCode = keyCode;
                    }
                }

                lastKeyPressed.text = "Last pressed: " + lastKeyCode.ToString();
            }
        }

        private void CheckLedgeHop()
        {
            Vector3 lineStart = Bounds.center;

            if (move.y < 0f && IsGrounded && jumpHandler.JumpState == JumpStateEnum.Grounded)
            {
                var raycastHit = Physics2D.Raycast(Bounds.center, Vector2.down, Bounds.extents.y + 1f);
                var ledgeHopLine = new Vector3(raycastHit.point.x, raycastHit.point.y, -1);
                Debug.DrawLine(lineStart, ledgeHopLine, lineColor);

                if (raycastHit.collider != null && raycastHit.fraction > 0f && raycastHit.fraction < 0.5f)
                {
                    PerformLedgeHop();
                }
            }
        }

        private void PerformLedgeHop()
        {
            jumpHandler.JumpState = JumpStateEnum.Jumping;
            velocity = new Vector2(move.x * maxSpeed, jumpHandler.JumpTakeOffSpeed * model.jumpModifier);
            Schedule<PlayerJumped>().player = this;
            Schedule<PlayerLanded>().player = this;
        }

        protected override void ComputeVelocity()
        {
            if (jumpHandler.Jump && (IsGrounded || jumpHandler.CoyoteTime > 0f))
            {
                jumpHandler.PerformJump();
            }
            else if (jumpHandler.StopJumping)
            {
                jumpHandler.DecelerateJump();
            }

            SetSpriteDirection();
            targetVelocity = move * maxSpeed;
        }

        private void SetSpriteDirection()
        {
            if (move.x > 0.01f)
            {
                spriteRenderer.flipX = false;
            }
            else if (move.x < -0.01f)
            {
                spriteRenderer.flipX = true;
            }
        }

    }
}
