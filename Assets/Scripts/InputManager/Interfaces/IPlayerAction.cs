namespace Assets.Scripts.InputManager.Interfaces
{
    // Define a generic interface for actions that the player can perform
    public interface IPlayerAction<T>
    {
        void Execute(T jumpHandler);
    }
}
