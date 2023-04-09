using poetools.player.Player;
using poetools.player.Player.Interaction;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Integrations
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

            if (IsLocalPlayer && Active)
            {
                if (container.PollWantsToInteract())
                    InteractServerRpc(viewRay);

                if (container.PollWantsToStopInteracting())
                    StopInteractingServerRpc();
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

        [ServerRpc]
        private void StopInteractingServerRpc()
        {
            container.InteractionLogic.StopInteracting(gameObject);
        }

        [ClientRpc]
        private void RunInteractEventClientRpc()
        {
            clientInteracted.Invoke();
        }
    }
}
