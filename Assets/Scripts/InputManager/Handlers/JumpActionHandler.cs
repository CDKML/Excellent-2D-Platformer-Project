using Assets.Scripts.Signals;
using System;
using Zenject;

namespace Assets.Scripts.InputManager.Handlers
{
    public class JumpActionHandler : SignalBusUser, IDisposable
    {
        public void Start()
        {
            SignalBus.Subscribe<JumpStartSignal>(OnJumpStart);
            SignalBus.Subscribe<JumpCancelSignal>(OnJumpCancel);
        }

        public void Dispose()
        {
            SignalBus.Unsubscribe<JumpStartSignal>(OnJumpStart);
            SignalBus.Unsubscribe<JumpCancelSignal>(OnJumpCancel);
        }

        private void OnJumpStart(JumpStartSignal signal)
        {
            signal.JumpHandler.JumpStartedAction();
        }

        private void OnJumpCancel(JumpCancelSignal signal)
        {
            signal.JumpHandler.JumpCanceledAction();
        }
    }
}