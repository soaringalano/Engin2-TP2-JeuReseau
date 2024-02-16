using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class RunhuntLoginUIHUD : MonoBehaviour
{
    [field: SerializeField] private NetworkManager Manager { get; set; }

    //public int offsetX;
    //public int offsetY;
    [field: SerializeField] private InputField NetworkAddress { get; set; }
    [field: SerializeField] private InputField NetworkPort { get; set; }

    //void Awake()
    //{
    //    manager = GetComponent<NetworkManager>();
    //}

    public void OnStartHost()
    {
        if (!NetworkClient.active)
        {
            Debug.Log("Starting Host");
            SendIpAndPort();
            Manager.StartHost();
        }
    }

    public void OnStartClient()
    {
        if (!NetworkClient.active)
        {
            Debug.Log("Starting Client");
            SendIpAndPort();
            Manager.StartClient();
        }
    }

    private void SendIpAndPort()
    {
        if (NetworkAddress.text == "localhost" || string.IsNullOrWhiteSpace(NetworkAddress.text) || string.IsNullOrEmpty(NetworkAddress.text))
        {
            Debug.LogWarning("NetworkAddress is null, assign loacalhost");
            Manager.networkAddress = "localhost";
        }
        else
        {
            Debug.Log("NetworkAddress is not null: " + NetworkAddress.text);
            Manager.networkAddress = NetworkAddress.text;
        }
        
        // only show a port field if we have a port transport
        // we can't have "IP:PORT" in the address field since this only
        // works for IPV4:PORT.
        // for IPV6:PORT it would be misleading since IPV6 contains ":":
        // 2001:0db8:0000:0000:0000:ff00:0042:8329
        if (Transport.active is PortTransport portTransport)
        {
            // use TryParse in case someone tries to enter non-numeric characters
            if (NetworkPort == null) 
            {
                Debug.LogWarning("NetworkPort is null, assign 7777");
                portTransport.Port = 7777;
            }
            else
            {
                Debug.Log("NetworkPort is not null: " + NetworkPort.text);
                if (ushort.TryParse(NetworkPort.text, out ushort port))
                    portTransport.Port = port;
            }
        }
    }

    public void SetNetworkAddress(InputField address)
    {
        NetworkAddress = address;
    }

    public void SetNetworkPort(InputField port)
    {
        NetworkPort = port;
    }


    //        void OnGUI()
    //        {
    //            // If this width is changed, also change offsetX in GUIConsole::OnGUI
    //            int width = 300;

    //            GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, width, 9999));

    //            if (!NetworkClient.isConnected && !NetworkServer.active)
    //                StartButtons();
    //            else
    //                StatusLabels();

    //            if (NetworkClient.isConnected && !NetworkClient.ready)
    //            {
    //                if (GUILayout.Button("Client Ready"))
    //                {
    //                    // client ready
    //                    Debug.Log("NetworkClient.Ready()");
    //                    NetworkClient.Ready();
    //                    if (NetworkClient.localPlayer == null)
    //                        NetworkClient.AddPlayer();
    //                }
    //            }

    //            StopButtons();

    //            GUILayout.EndArea();
    //        }

    //        void StartButtons()
    //        {
    //            if (!NetworkClient.active)
    //            {
    //#if UNITY_WEBGL
    //                // cant be a server in webgl build
    //                if (GUILayout.Button("Single Player"))
    //                {
    //                    NetworkServer.dontListen = true;
    //                    manager.StartHost();
    //                }
    //#else
    //                // Server + Client
    //                if (GUILayout.Button("Host (Server + Client)"))
    //                    manager.StartHost();
    //#endif

    //                // Client + IP (+ PORT)
    //                GUILayout.BeginHorizontal();

    //                if (GUILayout.Button("Client"))
    //                    manager.StartClient();

    //                manager.networkAddress = GUILayout.TextField(manager.networkAddress);
    //                // only show a port field if we have a port transport
    //                // we can't have "IP:PORT" in the address field since this only
    //                // works for IPV4:PORT.
    //                // for IPV6:PORT it would be misleading since IPV6 contains ":":
    //                // 2001:0db8:0000:0000:0000:ff00:0042:8329
    //                if (Transport.active is PortTransport portTransport)
    //                {
    //                    // use TryParse in case someone tries to enter non-numeric characters
    //                    if (ushort.TryParse(GUILayout.TextField(portTransport.Port.ToString()), out ushort port))
    //                        portTransport.Port = port;
    //                }

    //                GUILayout.EndHorizontal();

    //                // Server Only
    //#if UNITY_WEBGL
    //                // cant be a server in webgl build
    //                GUILayout.Box("( WebGL cannot be server )");
    //#else
    //                if (GUILayout.Button("Server Only"))
    //                    manager.StartServer();
    //#endif
    //            }
    //            else
    //            {
    //                // Connecting
    //                GUILayout.Label($"Connecting to {manager.networkAddress}..");
    //                if (GUILayout.Button("Cancel Connection Attempt"))
    //                    manager.StopClient();
    //            }
    //        }

    //        void StatusLabels()
    //        {
    //            // host mode
    //            // display separately because this always confused people:
    //            //   Server: ...
    //            //   Client: ...
    //            if (NetworkServer.active && NetworkClient.active)
    //            {
    //                // host mode
    //                GUILayout.Label($"<b>Host</b>: running via {Transport.active}");
    //            }
    //            else if (NetworkServer.active)
    //            {
    //                // server only
    //                GUILayout.Label($"<b>Server</b>: running via {Transport.active}");
    //            }
    //            else if (NetworkClient.isConnected)
    //            {
    //                // client only
    //                GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.active}");
    //            }
    //        }

    //        void StopButtons()
    //        {
    //            if (NetworkServer.active && NetworkClient.isConnected)
    //            {
    //                GUILayout.BeginHorizontal();
    //#if UNITY_WEBGL
    //                if (GUILayout.Button("Stop Single Player"))
    //                    manager.StopHost();
    //#else
    //                // stop host if host mode
    //                if (GUILayout.Button("Stop Host"))
    //                    manager.StopHost();

    //                // stop client if host mode, leaving server up
    //                if (GUILayout.Button("Stop Client"))
    //                    manager.StopClient();
    //#endif
    //                GUILayout.EndHorizontal();
    //            }
    //            else if (NetworkClient.isConnected)
    //            {
    //                // stop client if client-only
    //                if (GUILayout.Button("Stop Client"))
    //                    manager.StopClient();
    //            }
    //            else if (NetworkServer.active)
    //            {
    //                // stop server if server-only
    //                if (GUILayout.Button("Stop Server"))
    //                    manager.StopServer();
    //            }
    //        }
}
