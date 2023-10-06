using Assets.Scripts.Signals;
using System;
using Zenject;

namespace Assets.Scripts.InputManager.Handlers
{
    public class JumpActionHandler : IInitializable, IDisposable
    {
        [Inject]
        private SignalBus signalBus;

        public void Initialize()
        {
            signalBus.Subscribe<JumpStartSignal>(OnJumpStart);
            signalBus.Subscribe<JumpCancelSignal>(OnJumpCancel);
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<JumpStartSignal>(OnJumpStart);
            signalBus.Unsubscribe<JumpCancelSignal>(OnJumpCancel);
        }

        private void OnJumpStart(JumpStartSignal signal)
        {
            signal.Player.JumpStarted();
        }

        private void OnJumpCancel(JumpCancelSignal signal)
        {
            signal.Player.JumpCanceled();
        }
    }
}