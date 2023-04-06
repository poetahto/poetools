using UnityEngine;
using UnityEngine.Events;

namespace poetools.player.Player.Interaction
{
    public class InteractableUnityEvents : MonoBehaviour, IInteractable
    {
        [SerializeField] private UnityEvent<GameObject> interacted;
        
        public void HandleInteract(GameObject grabber)
        {
            interacted.Invoke(grabber);
        }
    }
}