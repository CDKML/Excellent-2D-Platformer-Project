using Platformer.Mechanics;

namespace Assets.Scripts.Signals
{
    public class JumpStartSignal
    {
        public PlayerController Player { get; }

        public JumpStartSignal(PlayerController player)
        {
            Player = player;
        }
    }
}
