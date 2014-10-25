using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class NetworkManager : ScriptableObject
    {
        public GameObject PlayerGameObject;


        private HostData[] _hostData;
        private const string GameName = "Galaga_Networking";
        private bool _refreshing;


        void OnServerInitialized()
        {
            Debug.Log("Server Initialized");
            spawnPlayer();

        }
        void OnConnectedToServer()
        {
            spawnPlayer();
        }

        void OnPlayerDisconnected(NetworkPlayer player)
        {
            Debug.Log("Clean up after player " + player);
            Network.RemoveRPCs(player);
            Network.DestroyPlayerObjects(player);
        }

        void OnMasterServerEvent(MasterServerEvent msEvent)
        {

            if (msEvent == MasterServerEvent.RegistrationSucceeded)
            {
                Debug.Log("Registered");
            }
        }
        // Called on the client when a connection attempt fails for some reason (Since v2.0)
        void OnFailedToConnect(NetworkConnectionError error)
        {
            switch (error)
            {
                case NetworkConnectionError.NoError:
                    break;
                case NetworkConnectionError.RSAPublicKeyMismatch:
                    break;
                case NetworkConnectionError.InvalidPassword:
                    break;
                case NetworkConnectionError.ConnectionFailed:
                    break;
                case NetworkConnectionError.TooManyConnectedPlayers:
                    break;
                case NetworkConnectionError.ConnectionBanned:
                    break;
                case NetworkConnectionError.AlreadyConnectedToServer:
                    break;
                case NetworkConnectionError.AlreadyConnectedToAnotherServer:
                    break;
                case NetworkConnectionError.CreateSocketOrThreadFailure:
                    break;
                case NetworkConnectionError.IncorrectParameters:
                    break;
                case NetworkConnectionError.EmptyConnectTarget:
                    break;
                case NetworkConnectionError.InternalDirectConnectFailed:
                    break;
                case NetworkConnectionError.NATTargetNotConnected:
                    break;
                case NetworkConnectionError.NATTargetConnectionLost:
                    break;
                case NetworkConnectionError.NATPunchthroughFailed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("error");
            }


        }

        // Called on clients or servers when there is a problem connecting to the MasterServer (Since v2.0)
        void OnFailedToConnectToMasterServer(NetworkConnectionError info)
        {
            switch (info)
            {
                case NetworkConnectionError.NoError:
                    break;
                case NetworkConnectionError.RSAPublicKeyMismatch:
                    break;
                case NetworkConnectionError.InvalidPassword:
                    break;
                case NetworkConnectionError.ConnectionFailed:
                    break;
                case NetworkConnectionError.TooManyConnectedPlayers:
                    break;
                case NetworkConnectionError.ConnectionBanned:
                    break;
                case NetworkConnectionError.AlreadyConnectedToServer:
                    break;
                case NetworkConnectionError.AlreadyConnectedToAnotherServer:
                    break;
                case NetworkConnectionError.CreateSocketOrThreadFailure:
                    break;
                case NetworkConnectionError.IncorrectParameters:
                    break;
                case NetworkConnectionError.EmptyConnectTarget:
                    break;
                case NetworkConnectionError.InternalDirectConnectFailed:
                    break;
                case NetworkConnectionError.NATTargetNotConnected:
                    break;
                case NetworkConnectionError.NATTargetConnectionLost:
                    break;
                case NetworkConnectionError.NATPunchthroughFailed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("info");
            }
        }


        public void RefreshHost()
        {
            Debug.Log("Refresh Host");
            _refreshing = true;
            MasterServer.RequestHostList(GameName);
        }

        public void StartServer()
        {
            try
            {
                var useNat = !Network.HavePublicAddress();
                Network.InitializeServer(1, 25001, useNat);
                MasterServer.RegisterHost(GameName, "Galaga Network", "Galaga multiplayer");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        // Use this for initialization
        void Start()
        {
            MasterServer.ipAddress = "127.0.0.1";
        }

        // Update is called once per frame
        void Update()
        {
            if (!_refreshing || MasterServer.PollHostList().Length <= 0) return;

            _refreshing = false;
            _hostData = MasterServer.PollHostList();
            Debug.Log(_hostData.Length);
        }

        public void GetAvailableGames(float btnX, float btnY, float btnW, float btnH)
        {

            if (_hostData == null) return;

            foreach (var hostData in _hostData)
            {
                if (GUI.Button(new Rect(btnX + 100f, btnY, btnW, btnH), hostData.gameName))
                    Network.Connect(hostData);
            }
        }


        private void spawnPlayer()
        {

            Network.Instantiate(PlayerGameObject, new Vector3(0, 0), Quaternion.identity, 0);

        }

    }
}
