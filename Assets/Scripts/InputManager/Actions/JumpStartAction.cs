using Assets.Scripts.InputManager.Interfaces;
using Assets.Scripts.Mechanics;

namespace Assets.Scripts.InputManager.Actions
{

    public class JumpStartAction : IPlayerAction<JumpHandler>
    {
        public void Execute(JumpHandler jumpHandler)
        {
            jumpHandler.JumpStartedAction();
        }
    }
}
