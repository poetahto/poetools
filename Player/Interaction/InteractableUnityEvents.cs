using UnityEngine;
using UnityEngine.Events;

namespace poetools.player.Player.Interaction
{
    public class InteractableUnityEvents : MonoBehaviour, IInteractable
    {
        [SerializeField] private UnityEvent<GameObject> interacted;
        [SerializeField] private UnityEvent<GameObject> stopped;

        public void HandleInteractStart(GameObject grabber)
        {
            interacted.Invoke(grabber);
        }

        public void HandleInteractStop(GameObject grabber)
        {
            stopped.Invoke(grabber);
        }
    }
}
