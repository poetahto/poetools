using System.Threading.Tasks;
using poetools.Core;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace poetools.Multiplayer
{
    public class MultiplayerSceneLoader
    {
        private const string ClientFadeOutCommand = "ClientFadeOut";
        private const string ClientFinishedFadingMessage = "ClientFinishedFading";

        private bool _loadEventCompleted;
        private float _remainingClients;
        private NetworkManager _netManager;
        private ColorFadeEffect _fadeEffect;

        public MultiplayerSceneLoader(NetworkManager networkManager, ColorFadeEffect fadeEffect)
        {
            _fadeEffect = fadeEffect;
            _netManager = networkManager;

            // Registering custom event handlers.
            _netManager.CustomMessagingManager.RegisterNamedMessageHandler(ClientFadeOutCommand, HandleClientFadeOut);
            _netManager.CustomMessagingManager.RegisterNamedMessageHandler(ClientFinishedFadingMessage, HandleClientFinishedFading);
        }

        public async Task Reload()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            await Load(currentScene);
        }

        public async Task Load(string sceneName)
        {
            if (_netManager.IsServer)
            {
                NetworkLog.LogInfo("Server started loading scene.");

                _remainingClients = _netManager.ConnectedClients.Count;
                _netManager.CustomMessagingManager.SendNamedMessageToAll(ClientFadeOutCommand, new FastBufferWriter(0, Allocator.Temp));

                while (_remainingClients > 0)
                    await Task.Yield();

                _netManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }

            else NetworkLog.LogError("A client tried to load a scene! Only the server can do this.");
        }

        // Send from the server to clients when its getting ready to change scenes.
        private async void HandleClientFadeOut(ulong senderId, FastBufferReader payload)
        {
            Assert.IsTrue(_netManager.IsClient);
            NetworkLog.LogInfo("Client started fading out.");

            _fadeEffect.SetVisible(true);
            await _fadeEffect.Await();

            _netManager.CustomMessagingManager.SendNamedMessage(ClientFinishedFadingMessage, NetworkManager.ServerClientId, new FastBufferWriter(0, Allocator.Temp));
            _netManager.SceneManager.OnSceneEvent += HandleOnSceneEvent;
            _loadEventCompleted = false;

            while (!_loadEventCompleted)
                await Task.Yield();

            _fadeEffect.SetVisible(false);
            await _fadeEffect.Await();
        }

        // Check on the client if we are done loading the scene.
        private void HandleOnSceneEvent(SceneEvent sceneEvent)
        {
            Assert.IsTrue(_netManager.IsClient);
            NetworkLog.LogInfo("Client got scene event.");

            if (sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted)
            {
                NetworkLog.LogInfo("Client finished loading");
                _netManager.SceneManager.OnSceneEvent -= HandleOnSceneEvent;
                _loadEventCompleted = true;
            }
        }

        // Sent from clients to the server when they finish playing their fade-out animation.
        private void HandleClientFinishedFading(ulong senderId, FastBufferReader payload)
        {
            Assert.IsTrue(_netManager.IsServer);
            NetworkLog.LogInfo("Server heard that client finished fading.");
            _remainingClients = Mathf.Max(0, _remainingClients - 1);
        }
    }
}
