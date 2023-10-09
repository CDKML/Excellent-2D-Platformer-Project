using Assets.Scripts.Mechanics;

namespace Assets.Scripts.Signals
{
    public class JumpCancelSignal
    {
        public JumpHandler JumpHandler { get; }

        public JumpCancelSignal(JumpHandler jumpHandler)
        {
            JumpHandler = jumpHandler;
        }
    }
}
