using System;
using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{

    float btnX, btnY, btnW, btnH;
    string GameName = "Galaga_Networking";
    private bool _refreshing;

    private string _proxyIp = "";
    private HostData[] hostData;

    // Awake is called when the script instance is being loaded (Since v1.0)
    void Awake()
    {
        StartServer();
    }

    void StartServer()
    {
        if (hostData != null && hostData.Length > 0)
            foreach (var data in hostData)
            {
                Network.Connect(data);
            }
        else
        {
            MasterServer.ipAddress = "127.0.0.1";
            var useNat = !Network.HavePublicAddress();

            Network.InitializeServer(1, 25001, useNat);
            MasterServer.RegisterHost(GameName, "Galaga", "Multiplayer Game");
            RefreshHost();

        }
    }

    void OnServerInitialized()
    {
        Debug.Log("Server Initialized");


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


    void RefreshHost()
    {
        Debug.Log("Refresh Host");
        MasterServer.RequestHostList(GameName);
        _refreshing = true;

    }

    void OnGUI()
    {
        //var startServer = GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Start Server");
        //var refreshHost = GUI.Button(new Rect(btnX, btnY * 3.2f, btnW, btnH), "Refresh Host");

        //if (startServer)
        //{
        //    Debug.Log("Server started");
        //    try
        //    {
        //        MasterServer.RegisterHost(GameName, "Galaga", "Multiplayer Game");
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.Log(e);
        //    }
        //}


    }

    // Use this for initialization
    void Start()
    {
        btnX = Screen.width * 0.1f;
        btnY = Screen.width * 0.1f;
        btnW = Screen.width * 0.1f;
        btnH = Screen.width * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_refreshing || MasterServer.PollHostList().Length <= 0) return;

        _refreshing = false;
        hostData = MasterServer.PollHostList();
        Debug.Log(hostData.Length);
    }
}
