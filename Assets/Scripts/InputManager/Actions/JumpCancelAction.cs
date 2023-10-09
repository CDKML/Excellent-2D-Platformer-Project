using Assets.Scripts.InputManager.Interfaces;
using Assets.Scripts.Mechanics;

namespace Assets.Scripts.InputManager.Actions
{
    public class JumpCancelAction : IPlayerAction<JumpHandler>
    {
        public void Execute(JumpHandler jumpHandler)
        {
            jumpHandler.JumpCanceledAction();
        }
    }
}
