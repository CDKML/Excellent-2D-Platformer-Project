using Assets.Scripts.InputManager.Interfaces;
using Platformer.Mechanics;

namespace Assets.Scripts.InputManager.Actions
{
    public class JumpCancelAction : IPlayerAction
    {
        public void Execute(PlayerController player)
        {
            player.JumpCanceled();
        }
    }
}
