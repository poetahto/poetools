using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace poetools.Multiplayer
{
    public class RelayStartup
    {
        private UnityTransport _transport;
        private NetworkManager _netManager;

        public RelayStartup(UnityTransport transport, NetworkManager networkManager)
        {
            _transport = transport;
            _netManager = networkManager;
        }

        public async Task RelayStartClient(string joinCode)
        {
            RelayUtil.RelayJoinData info = await RelayUtil.JoinGame(joinCode);
            _transport.SetRelayServerData(info.IPv4Address, info.Port, info.AllocationIDBytes, info.Key, info.ConnectionData, info.HostConnectionData);
            _netManager.StartClient();
        }

        public async Task<string> RelayStartHost(int maxPlayers)
        {
            RelayUtil.RelayHostData info = await RelayUtil.HostGame(Mathf.Max(1, maxPlayers - 1));
            _transport.SetRelayServerData(info.IPv4Address, info.Port, info.AllocationIDBytes, info.Key, info.ConnectionData);
            _netManager.StartHost();

            return info.JoinCode;
        }
    }
}
