using Assets.Scripts.InputManager.Interfaces;
using Platformer.Mechanics;

namespace Assets.Scripts.InputManager.Actions
{

    public class JumpStartAction : IPlayerAction
    {
        public void Execute(PlayerController player)
        {
            player.JumpStarted();
        }
    }
}
