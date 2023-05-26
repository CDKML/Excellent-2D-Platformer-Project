using UnityEngine;
using UnityEngine.Events;

public class OnObjectEnable : MonoBehaviour
{
    // Public config
    public UnityEvent onEnable;

    public void OnEnable()
    {
        onEnable?.Invoke();
    }
}
