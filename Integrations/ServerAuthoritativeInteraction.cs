using poetools.player.Player;
using poetools.player.Player.Interaction;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Examples
{
    public class ServerAuthoritativeInteraction : NetworkBehaviour, IInputProvider
    {
        [SerializeField]
        private FPSInteractionLogicContainer container;

        [SerializeField]
        private UnityEvent clientInteracted;

        public bool Active { get; set; }

        private void Update()
        {
            var viewRay = new Ray(container.viewDirection.position, container.viewDirection.forward);

            if (IsLocalPlayer && Active && container.PollWantsToInteract())
            {
                InteractServerRpc(viewRay);
            }

            container.InteractionLogic.ViewRay = viewRay;
        }

        [ServerRpc]
        private void InteractServerRpc(Ray ray)
        {
            container.InteractionLogic.ViewRay = ray;
            container.InteractionLogic.Interact(gameObject);

            if (container.InteractionLogic.HasFacingObject)
                RunInteractEventClientRpc();
        }

        [ClientRpc]
        private void RunInteractEventClientRpc()
        {
            clientInteracted.Invoke();
        }
    }
}
