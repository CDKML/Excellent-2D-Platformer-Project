using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class SignalBusUser : MonoBehaviour
    {
        protected SignalBus SignalBus {  get; private set; }

        [Inject]
        public void Init(SignalBus signalBus)
        {
            SignalBus = signalBus;
        }
    }
}
