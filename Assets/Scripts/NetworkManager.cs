using System;
using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{

    float btnX, btnY, btnW, btnH;
    string GameName = "Galaga_Networking";
    private bool _refreshing;


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

    void RefreshHost()
    {
        Debug.Log("Refresh Host");
        MasterServer.RequestHostList(GameName);
        _refreshing = true;
        Debug.Log(MasterServer.PollHostList().Length);

    }

    void OnGUI()
    {
        var startServer = GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Start Server");
        var refreshHost = GUI.Button(new Rect(btnX, btnY * 3.2f, btnW, btnH), "Refresh Host");

        if (startServer)
        {
            Debug.Log("Server started");
            var useNat = !Network.HavePublicAddress();
            try
            {
                Network.InitializeServer(1, 25001, useNat);
                MasterServer.RegisterHost(GameName, "Galaga", "Multiplayer Game");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        if (refreshHost) 
            RefreshHost();

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

    }
}
