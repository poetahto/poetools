using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine.Assertions;

// This type is defined here so we can quickly change it if we want to increase / decrease chat message payload size.
using FixedString = Unity.Collections.FixedString64Bytes;

namespace poetools.Multiplayer
{
    /// <summary>
    /// A system for broadcasting global messages from clients or the server.
    /// </summary>
    public class ChatSystem
    {
        private const string ClientReceiveChatMessage = "ClientReceiveChat";
        private const string ServerBroadcastChatCommand = "ServerBroadcastChat";

        /// <summary>
        /// Called on clients when a global message is received.
        /// </summary>
        public event Action<string> MessageReceived;

        private NetworkManager _networkManager;

        public ChatSystem(NetworkManager networkManager)
        {
            _networkManager = networkManager;
            _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(ClientReceiveChatMessage, HandleClientReceiveChat);
            _networkManager.CustomMessagingManager.RegisterNamedMessageHandler(ServerBroadcastChatCommand, HandleServerBroadcastChat);
        }

        /// <summary>
        /// Broadcast a message to all members of the server.
        /// <remarks>The maximum message size is <see cref="FixedString"/>.</remarks>
        /// </summary>
        /// <param name="message">The message to be sent. Note: if it is too long, it will not be fully serialized.</param>
        public void SendMessage(string message)
        {
            var fixedMessage = new FixedString(message);

            if (_networkManager.IsClient)
            {
                NetworkLog.LogInfo("Client sending message.");

                using var writer = new FastBufferWriter(64, Allocator.Temp);
                writer.WriteValueSafe(fixedMessage);
                _networkManager.CustomMessagingManager.SendNamedMessage(ServerBroadcastChatCommand, NetworkManager.ServerClientId, writer);
            }
            else if (_networkManager.IsServer)
            {
                NetworkLog.LogInfo("Server sending message.");
                ServerBroadcast(fixedMessage);
            }
        }

        // Called on a server when a client wants to globally broadcast a message.
        private void HandleServerBroadcastChat(ulong senderId, FastBufferReader payload)
        {
            Assert.IsTrue(_networkManager.IsServer);
            NetworkLog.LogInfo("Server received command to broadcast a message.");

            // This would probably be where you make sure the message is appropriate / safe.
            payload.ReadValueSafe(out FixedString message);
            ServerBroadcast(message);
        }

        // Called on a server to send a message to clients.
        private void ServerBroadcast(FixedString message)
        {
            Assert.IsTrue(_networkManager.IsServer);
            NetworkLog.LogInfo("Server is broadcasting message.");

            using var writer = new FastBufferWriter(64, Allocator.Temp);
            writer.WriteValueSafe(message);
            _networkManager.CustomMessagingManager.SendNamedMessageToAll(ClientReceiveChatMessage, writer);
        }

        // Received on clients when a global message has been broadcast.
        private void HandleClientReceiveChat(ulong senderId, FastBufferReader payload)
        {
            Assert.IsTrue(_networkManager.IsClient);
            NetworkLog.LogInfo("Client received message from server.");

            payload.ReadValueSafe(out FixedString message);
            MessageReceived?.Invoke(message.ToString());
        }
    }
}
