using Platformer.Mechanics;

namespace Assets.Scripts.InputManager.Interfaces
{
    // Define an interface for actions that the player can perform
    public interface IPlayerAction
    {
        void Execute(PlayerController player);
    }
}
