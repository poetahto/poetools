using System;
using poetools.Core;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace poetools.Multiplayer
{
    /// <summary>
    /// Unity point of access to the multiplayer API.
    /// </summary>
    public class MultiplayerController : PreparedSingleton<MultiplayerController>
    {
        [SerializeField]
        private ColorFadeContainer container;

        [SerializeField]
        private NetworkManager networkManager;

        /// <summary>
        /// Called when multiplayer systems become activated.
        /// </summary>
        public event Action Initialized;

        /// <summary>
        /// Called when multiplayer systems become deactivated.
        /// </summary>
        public event Action Disconnected;

        public MultiplayerSceneLoader SceneLoader { get; private set; }
        public RelayStartup RelayStartup { get; private set; }
        public DirectConnectionStartup DirectStartup { get; private set; }
        public ChatSystem ChatSystem { get; private set; }

        public void Shutdown()
        {
            networkManager.Shutdown();
        }

        private void Start()
        {
            // Startup managers are unique in that they don't need the server to be started.
            // This is kind of obvious, but important to initialize right away.
            var transport = GetComponent<UnityTransport>();
            RelayStartup = new RelayStartup(transport, networkManager);
            DirectStartup = new DirectConnectionStartup(transport, networkManager);
        }

        private void OnEnable()
        {
            networkManager.OnServerStarted += HandleServerStarted;
            networkManager.OnClientConnectedCallback += HandleClientConnected;
            networkManager.OnClientDisconnectCallback += HandleClientDisconnected;

        }

        private void OnDisable()
        {
            networkManager.OnServerStarted -= HandleServerStarted;
            networkManager.OnClientConnectedCallback -= HandleClientConnected;
            networkManager.OnClientDisconnectCallback -= HandleClientDisconnected;
        }

        private void HandleServerStarted() => SetupNetworkSystems();

        private void HandleClientConnected(ulong obj)
        {
            // We don't want to set things up twice, and a host / server has already handled the above callback.
            if (!networkManager.IsServer)
                SetupNetworkSystems();
        }

        // We can only initialize multiplayer-dependent systems once we have been connected.
        private void SetupNetworkSystems()
        {
            SceneLoader = new MultiplayerSceneLoader(networkManager, container.FadeEffect);
            ChatSystem = new ChatSystem(networkManager);

            NetworkLog.LogInfo("Initialized network systems.");
            Initialized?.Invoke();
        }

        private void HandleClientDisconnected(ulong obj)
        {
            if (!networkManager.IsServer)
                Disconnected?.Invoke();
        }
    }
}
