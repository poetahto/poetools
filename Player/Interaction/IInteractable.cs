using UnityEngine;

namespace poetools.player.Player.Interaction
{
    public interface IInteractable
    {
        void HandleInteractStart(GameObject grabber);
        void HandleInteractStop(GameObject grabber);
    }
}
