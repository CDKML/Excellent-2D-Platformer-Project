using Assets.Scripts.InputManager.Handlers;
using Assets.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            try
            {
                InstallSignals();

                // Bind the handlers
                Container.BindInterfacesAndSelfTo<JumpActionHandler>().AsSingle().NonLazy();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(" - InstallBindings exception " + ex);
                throw ex;
            }
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);

            // Bind your signals
            Container.DeclareSignal<JumpStartSignal>();
            Container.DeclareSignal<JumpCancelSignal>();
        }
    }
}
