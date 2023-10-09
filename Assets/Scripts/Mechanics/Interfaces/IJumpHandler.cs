using UnityEngine.InputSystem;

namespace Assets.Scripts.Mechanics.Interfaces
{
    public interface IJumpHandler
    {
        void SetupJumpAction(InputActionReference jumpAction);
        void PerformJump();
        void UpdateJumpState();
    }
}