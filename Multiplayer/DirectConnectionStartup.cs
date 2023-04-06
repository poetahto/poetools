using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace poetools.Multiplayer
{
    public class DirectConnectionStartup
    {
        public const string DefaultIP = "127.0.0.1";
        public const ushort DefaultPort = 14236;
        public const string DefaultListenAddress = "0.0.0.0";

        private UnityTransport _transport;
        private NetworkManager _netManager;

        public DirectConnectionStartup(UnityTransport transport, NetworkManager networkManager)
        {
            _transport = transport;
            _netManager = networkManager;
        }

        public void StartClient(string ip = DefaultIP, ushort port = DefaultPort)
        {
            _transport.SetConnectionData(ip, port);
            _netManager.StartClient();
        }

        public void StartHost(string ip = DefaultIP, ushort port = DefaultPort, string listenAddress = DefaultListenAddress)
        {
            _transport.SetConnectionData(ip, port, listenAddress);
            _netManager.StartHost();
        }

        public void StartServer(string ip = DefaultIP, ushort port = DefaultPort, string listenAddress = DefaultListenAddress)
        {
            _transport.SetConnectionData(ip, port, listenAddress);
            _netManager.StartServer();
        }
    }
}
