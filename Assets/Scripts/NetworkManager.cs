using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{

    float btnX, btnY, btnW, btnH;
    private string GameName = "Galaga_Networking";


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



    void OnGUI()
    {
        var startServer = GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Start Server");
        var refreshHost = GUI.Button(new Rect(btnX, btnY * 3.2f, btnW, btnH), "Refresh Host");

        if (startServer)
        {
            Debug.Log("Server started");
            Network.InitializeServer(2, 25001, !Network.HavePublicAddress());
            MasterServer.RegisterHost(GameName, "Galaga", "Multiplayer Game");
        }

        if (refreshHost)
        {
            Debug.Log("Refresh Host");
        }
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
