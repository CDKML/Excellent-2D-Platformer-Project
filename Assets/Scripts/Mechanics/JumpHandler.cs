using Assets.Scripts.Enums;
using Platformer.Model;
using UnityEngine;

namespace Assets.Scripts.Mechanics
{
    public class JumpHandler
    {
        private float jumpTakeOffSpeed;
        private PlatformerModel model;
        private JumpStateEnum jumpState;
        private Vector2 velocity;

        public JumpHandler(float jumpTakeOffSpeed, PlatformerModel model)
        {
            this.jumpTakeOffSpeed = jumpTakeOffSpeed;
            this.model = model;
        }

        public void SetJumpState(JumpStateEnum state)
        {
            this.jumpState = state;
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        public Vector2 PerformJump()
        {
            if (jumpState == JumpStateEnum.PrepareToJump)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
            }
            return velocity;
        }

        public Vector2 DecelerateJump()
        {
            if (velocity.y > 0)
            {
                velocity.y *= model.jumpDeceleration;
            }
            return velocity;
        }
    }
}
