using Assets.Scripts.Enums;
using Assets.Scripts.Mechanics.Interfaces;
using Assets.Scripts.Signals;
using Platformer.Core;
using Platformer.Gameplay;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using UnityEngine.InputSystem;
using static Platformer.Core.Simulation;

namespace Assets.Scripts.Mechanics
{
    public class JumpHandler : SignalBusUser, IJumpHandler
    {
        public float JumpTakeOffSpeed = 9;
        private readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        [SerializeField] private PlayerController playerController;
        public JumpStateEnum JumpState = JumpStateEnum.Grounded;

        public bool JumpReleased = false;
        public bool JumpStarted = false;
        public float CoyoteTimeCounter = 0f;
        public float JumpBufferTimeCounter = 0f;

        [Header("Jump Timers")]
        public float CoyoteTime = 0.075f;
        public float JumpBufferTime = 0.1f;

        private bool jump;
        private bool stopJump;

        public bool Jump
        {
            get { return jump; }
            set { jump = value; }
        }

        public bool StopJumping
        {
            get { return stopJump; }
            set { stopJump = value; }
        }

        public JumpStateEnum GetJumpState()
        {
            return JumpState;
        }

        public void SetupJumpAction(InputActionReference jumpAction)
        {
            jumpAction.action.started += ctx => FireJump();
            jumpAction.action.canceled += ctx => CancelJump();
        }

        public void JumpInputLogic()
        {
            if (CoyoteTimeCounter > 0f && JumpBufferTimeCounter > 0f && JumpState != JumpStateEnum.Jumping)
            {
                SetJumpState(JumpStateEnum.PrepareToJump);
                JumpBufferTimeCounter = 0f;
            }
            else if (JumpReleased)
            {
                StopJump();
            }
        }

        private void FireJump()
        {
            SignalBus.Fire(new JumpStartSignal(this));
        }

        private void CancelJump()
        {
            SignalBus.Fire(new JumpCancelSignal(this));
        }

        public void JumpStartedAction()
        {
            JumpStarted = true;
            JumpReleased = false;
        }

        public void JumpCanceledAction()
        {
            JumpReleased = true;
        }

        public void SetJumpState(JumpStateEnum state)
        {
            JumpState = state;
        }

        public void PerformJump()
        {
            if (JumpState == JumpStateEnum.Jumping)
            {
                playerController.velocity.y = JumpTakeOffSpeed * model.jumpModifier;
                Jump = false;
            }
        }

        public void UpdateCoyoteTimer()
        {
            CoyoteTimeCounter = playerController.IsGrounded ? CoyoteTime : CoyoteTimeCounter - Time.deltaTime;
        }

        public void UpdateJumpBuffer()
        {
            JumpBufferTimeCounter = JumpStarted ? JumpBufferTime : JumpBufferTimeCounter - Time.deltaTime;
            if (JumpStarted)
            {
                JumpStarted = false;
            }
        }

        public Vector2 DecelerateJump()
        {
            StopJumping = false;

            if (playerController.velocity.y > 0)
            {
                playerController.velocity.y *= model.jumpDeceleration;
            }
            return playerController.velocity;
        }

        public void UpdateJumpState()
        {
            jump = false;
            switch (JumpState)
            {
                case JumpStateEnum.PrepareToJump:
                    PrepareToJumpStateUpdate();
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

        private void PrepareToJumpStateUpdate()
        {
            JumpState = JumpStateEnum.Jumping;
            Jump = true;
            StopJumping = false;
        }

        private void JumpingStateUpdate()
        {
            if (!playerController.IsGrounded)
            {
                Schedule<PlayerJumped>().player = playerController;
                JumpState = JumpStateEnum.InFlight;
            }
        }

        private void InFlightStateUpdate()
        {
            if (playerController.IsGrounded)
            {
                Schedule<PlayerLanded>().player = playerController;
                SetJumpState(JumpStateEnum.Landed);
            }
            if (playerController.velocity.y <= 0)
            {
                SetJumpState(JumpStateEnum.Falling);
            }
        }

        private void FallingStateUpdate()
        {
            if (playerController.IsGrounded)
            {
                Schedule<PlayerLanded>().player = playerController;
                SetJumpState(JumpStateEnum.Landed);
            }
        }

        private void LandedStateUpdate()
        {
            SetJumpState(JumpStateEnum.Grounded);
        }

        public void StopJump()
        {
            StopJumping = true;
            Schedule<PlayerStopJump>().player = playerController;
        }
    }
}
