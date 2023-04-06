using UnityEngine;

namespace poetools.player.Player.Interaction
{
    public interface IInteractable
    {
        void HandleInteract(GameObject grabber);
    }
}