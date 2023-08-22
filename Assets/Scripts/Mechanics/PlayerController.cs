using Assets.Scripts.Enums;
using Platformer.Core;
using Platformer.Gameplay;
using Platformer.Model;
using System;
using TMPro;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class PlayerController : KinematicObject
    {
        [Header("Player Parameters")]
        [SerializeField] private float maxSpeed = 5;
        [SerializeField] private float jumpTakeOffSpeed = 7;
        [SerializeField] private bool controlEnabled = true;

        [Header("Jump Timers")]
        [SerializeField] private float coyoteTime = 0.1f;
        [SerializeField] private float jumpBufferTime = 0.1f;

        [Header("Controls")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference toggleInventoryAction;

        [Header("Debugging")]
        [SerializeField] private TextMeshProUGUI lastKeyPressed;
        [SerializeField] private Color lineColor;
        [SerializeField] private GameObject inventoryUI;   // Reference to your inventory UI

        private Vector2 move;
        public JumpStateEnum jumpState = JumpStateEnum.Grounded;
        private bool jump;
        private bool stopJump;
        private bool jumpReleased = false;
        private bool jumpStarted = false;
        private float coyoteTimeCounter;
        private float jumpBufferTimeCounter;
        private KeyCode lastKeyCode;

        private Collider2D collider2d;
        private AudioSource audioSource;
        private Health health;
        private SpriteRenderer spriteRenderer;

        private readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            InitializeComponents();
            SetupJumpAction();
            SetupToggleInventoryAction();
        }

        private void SetupToggleInventoryAction()
        {
            toggleInventoryAction.action.Enable();
            toggleInventoryAction.action.started += ctx => { ToggleInventory(); };
        }

        private void ToggleInventory()
        {
            if (inventoryUI.activeSelf)
            {
                inventoryUI.SetActive(false);
            }
            else
            {
                inventoryUI.SetActive(true);
            }
        }

        protected override void FixedUpdate()
        {
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

            UpdateJumpState();
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

        private void SetupJumpAction()
        {
            jumpAction.action.started += ctx => JumpStarted();
            jumpAction.action.canceled += ctx => JumpCanceled();
        }

        private void HandleInput()
        {
            UpdateLastKeyPressed();

            move = moveAction.action.ReadValue<Vector2>();

            UpdateCoyoteTimer();
            UpdateJumpBuffer();
            CheckLedgeHop();

            if (coyoteTimeCounter > 0f && jumpBufferTimeCounter > 0f && jumpState != JumpStateEnum.Jumping)
            {
                jumpState = JumpStateEnum.PrepareToJump;
                jumpBufferTimeCounter = 0f;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                StopJump();
            }
        }

        private void JumpStarted()
        {
            jumpStarted = true;
            jumpReleased = false;
        }

        private void JumpCanceled()
        {
            jumpReleased = true;
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

        private void UpdateCoyoteTimer()
        {
            coyoteTimeCounter = IsGrounded ? coyoteTime : coyoteTimeCounter - Time.deltaTime;
        }

        private void UpdateJumpBuffer()
        {
            jumpBufferTimeCounter = jumpStarted ? jumpBufferTime : jumpBufferTimeCounter - Time.deltaTime;
            if (jumpStarted) jumpStarted = false;
        }

        private void CheckLedgeHop()
        {
            Vector3 lineStart = Bounds.center;

            if (move.y < 0f && IsGrounded && jumpState == JumpStateEnum.Grounded)
            {
                var bounds = collider2d.bounds;
                var raycastHit = Physics2D.Raycast(bounds.center, Vector2.down, bounds.extents.y + 1f);
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
            jumpState = JumpStateEnum.Jumping;
            velocity = new Vector2(move.x * maxSpeed, jumpTakeOffSpeed * model.jumpModifier);
            Schedule<PlayerJumped>().player = this;
            Schedule<PlayerLanded>().player = this;
        }

        private void StopJump()
        {
            stopJump = true;
            Schedule<PlayerStopJump>().player = this;
            coyoteTimeCounter = 0f;
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpStateEnum.PrepareToJump:
                    PrepareToJump();
                    break;
                case JumpStateEnum.Jumping:
                    JumpingStateUpdate();
                    break;
                case JumpStateEnum.InFlight:
                    InFlightStateUpdate();
                    break;
                case JumpStateEnum.Falling:
                    FallingStateUpdate();
                    break;
                case JumpStateEnum.Landed:
                    LandedStateUpdate();
                    break;
            }
        }

        private void PrepareToJump()
        {
            jumpState = JumpStateEnum.Jumping;
            jump = true;
            stopJump = false;
        }

        private void JumpingStateUpdate()
        {
            if (!IsGrounded)
            {
                Schedule<PlayerJumped>().player = this;
                jumpState = JumpStateEnum.InFlight;
            }
        }

        private void InFlightStateUpdate()
        {
            if (velocity.y < 0)
            {
                jumpState = JumpStateEnum.Falling;
            }
        }

        private void FallingStateUpdate()
        {
            if (IsGrounded)
            {
                Schedule<PlayerLanded>().player = this;
                jumpState = JumpStateEnum.Landed;
            }
        }

        private void LandedStateUpdate()
        {
            jumpState = JumpStateEnum.Grounded;
        }

        protected override void ComputeVelocity()
        {
            if (jump && (IsGrounded || coyoteTime > 0f))
            {
                PerformJump();
            }
            else if (stopJump)
            {
                DecelerateJump();
            }

            SetSpriteDirection();
            targetVelocity = move * maxSpeed;
        }

        private void PerformJump()
        {
            velocity.y = jumpTakeOffSpeed * model.jumpModifier;
            jump = false;
        }

        private void DecelerateJump()
        {
            stopJump = false;
            if (velocity.y > 0)
            {
                velocity.y *= model.jumpDeceleration;
            }
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

        public JumpStateEnum GetJumpState()
        {
            return jumpState;
        }
    }
}
