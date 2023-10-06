using Platformer.Mechanics;

namespace Assets.Scripts.Signals
{
    public class JumpCancelSignal
    {
        public PlayerController Player { get; }

        public JumpCancelSignal(PlayerController player)
        {
            Player = player;
        }
    }
}
