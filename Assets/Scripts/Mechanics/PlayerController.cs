using Assets.Scripts.Enums;
using Platformer.Core;
using Platformer.Gameplay;
using Platformer.Model;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        //public AudioClip jumpAudio;
        //public AudioClip respawnAudio;
        //public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 5;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpStateEnum jumpState = JumpStateEnum.Grounded;
        private bool stopJump;
        /*internal new*/
        public Collider2D collider2d;
        /*internal new*/
        public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        public float coyoteTime = 0.1f;
        float coyoteTimeCounter;

        // Jump buffer variables
        public float jumpBufferTime = 0.1f;
        float jumpBufferTimeCounter;

        bool jump;
        Vector2 move;
        [SerializeField] InputActionReference moveAction;
        [SerializeField] InputActionReference jumpAction;

        SpriteRenderer spriteRenderer;
        //internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        public Text textCanvas;
        private KeyCode lastKeyCode;

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            //animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                LastKeyPressed();

                move = moveAction.action.ReadValue<Vector2>();

                CoyoteTimer();
                JumpBuffer();
                LedgeHop();

                if (coyoteTimeCounter > 0f && jumpBufferTimeCounter > 0f && jumpState != JumpStateEnum.Jumping)
                {
                    jumpState = JumpStateEnum.PrepareToJump;
                    jumpBufferTimeCounter = 0f;
                }
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                    coyoteTimeCounter = 0f;
                }
            }
            else
            {
                move = Vector2.zero;
            }

            //if(velocity.y < -0.1)
            //{
            //    jumpState = JumpStateEnum.Falling;
            //}

            UpdateJumpState();
            base.Update();
            base.FixedUpdate();
        }
        public Color lineColor;

        private void LedgeHop()
        {
            Vector3 lineStart = Bounds.center;

            // Check if the player is near the edge of a platform and perform a ledge hop
            if (move.y < 0f && IsGrounded && jumpState == JumpStateEnum.Grounded)
            {
                var bounds = collider2d.bounds;
                var raycastHit = Physics2D.Raycast(bounds.center, Vector2.down, bounds.extents.y + 1f);
                Debug.DrawLine(lineStart, raycastHit.point, lineColor);
                if (raycastHit.collider != null && raycastHit.fraction > 0f && raycastHit.fraction < 0.5f)
                {
                    jumpState = JumpStateEnum.Jumping;
                    velocity = new Vector2(move.x * maxSpeed, jumpTakeOffSpeed * model.jumpModifier);
                    Schedule<PlayerJumped>().player = this;
                    Schedule<PlayerLanded>().player = this;
                }
            }
        }

        private void LastKeyPressed()
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
                textCanvas.text = "Last Key Pressed: " + lastKeyCode.ToString();
            }
        }

        private void CoyoteTimer()
        {
            if (IsGrounded)
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }
        }

        private void JumpBuffer()
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferTimeCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferTimeCounter -= Time.deltaTime;
            }
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpStateEnum.PrepareToJump:
                    jumpState = JumpStateEnum.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpStateEnum.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpStateEnum.InFlight;
                    }
                    break;
                case JumpStateEnum.InFlight:
                    if (velocity.y < 0)
                    {
                        jumpState = JumpStateEnum.Falling;
                    }
                    break;
                case JumpStateEnum.Falling:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpStateEnum.Landed;
                    }
                    break;
                case JumpStateEnum.Landed:
                    jumpState = JumpStateEnum.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && (IsGrounded || coyoteTime > 0f))
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y *= model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            //animator.SetBool("grounded", IsGrounded);
            //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public JumpStateEnum GetJumpState()
        {
            return jumpState;
        }

    }
}