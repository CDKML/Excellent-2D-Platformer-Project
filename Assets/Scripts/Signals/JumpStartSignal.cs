using Assets.Scripts.Mechanics;

namespace Assets.Scripts.Signals
{
    public class JumpStartSignal
    {
        public JumpHandler JumpHandler { get; }

        public JumpStartSignal(JumpHandler jumpHandler)
        {
            JumpHandler = jumpHandler;
        }
    }
}
