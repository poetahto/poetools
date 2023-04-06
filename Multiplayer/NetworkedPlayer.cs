using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

namespace poetools.Multiplayer
{
    public class NetworkedPlayer : NetworkBehaviour
    {
        [SerializeField]
        private Renderer bodyRenderer;

        [SerializeField]
        private Behaviour[] localPlayerOnlyBehaviours;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            foreach (var behaviour in localPlayerOnlyBehaviours)
                behaviour.enabled = IsLocalPlayer;

            bodyRenderer.shadowCastingMode = IsLocalPlayer ? ShadowCastingMode.ShadowsOnly : ShadowCastingMode.TwoSided;
        }
    }
}
