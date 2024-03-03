using Mirror;
using UnityEngine;
using static Mirror.RoomManager;
using static PlasticGui.PlasticTableColumn;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

namespace Mirror
{
    public class PlayerEmpty : NetworkBehaviour
    {
        [SyncVar] public EPlayerSelectedTeam m_currentPlayerSelectedTeam = EPlayerSelectedTeam.Count;
        [field: SerializeField] private GameObject Runner { get; set; }
        [field: SerializeField] private GameObject Hunter { get; set; }

        [SerializeField] public int m_playerSelectedTeam = 2;

        private static int m_emptyPlayerIdIterator = 0;
        [SerializeField] public int m_emptyPlayerId = 0;

        private bool m_isInitialized = false;
        private bool m_instanciated = false;
        private bool m_isRoomPlayerNull = false;

        private void Start()
        {
            m_emptyPlayerIdIterator++;
            m_emptyPlayerId = m_emptyPlayerIdIterator;
        }

        public void Initialize()
        {
            Debug.LogError("PlayerEmpty Initialize() m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            m_isInitialized = true;
        }

        private void InstanciateCharacter(EPlayerSelectedTeam playerSelectedTeam)
        {

            if (isClient)
            {
                // If running on the client, send a command request to the server
                CmdRequestSpawnObject(playerSelectedTeam);
            }
            else
            {
                // If running on the server, spawn the object directly
                SpawnMyObject(playerSelectedTeam);
            }
            //SpawnMyObject(playerSelectedTeam);
            m_instanciated = true;
        }

        private void Update()
        {
            //if (!NetworkServer.active) return;
            if (!isLocalPlayer) return;

            if (m_instanciated) return;
            Debug.Log("PlayerEmpty Update() m_playerSelectedTeam: " + m_playerSelectedTeam + " m_currentPlayerSelectedTeam: " + m_currentPlayerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);

            if (m_currentPlayerSelectedTeam == EPlayerSelectedTeam.Count) Debug.LogError("PlayerEmpty Update() m_currentPlayerSelectedTeam: " + m_currentPlayerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            InstanciateCharacter(m_currentPlayerSelectedTeam);
            m_instanciated = true;
        }


        [Command]
        private void CmdRequestSpawnObject(EPlayerSelectedTeam playerSelectedTeam)
        {
            // Call the server-side method to spawn the object
            SpawnMyObject(playerSelectedTeam);
        }

        public void SpawnMyObject(EPlayerSelectedTeam selectedTeam)
        {
            Debug.Log("SpawnMyObject() selectedTeam: " + selectedTeam);
            GameObject prefab = null;
            if (selectedTeam == EPlayerSelectedTeam.Hunters)
            {
                prefab = Hunter;
            }
            else if (selectedTeam == EPlayerSelectedTeam.Runners)
            {
                prefab = Runner;
            }

            // Instantiate your object here
            Debug.Log("SpawnMyObject() Instantiate prefab: " + prefab);
            GameObject myObject = Instantiate(prefab);

            // Spawn the object with client authority
            Debug.Log("SpawnMyObject() NetworkServer.Spawn(myObject, connectionToClient)");
            NetworkServer.Spawn(myObject, connectionToClient);
        }
    }
}



//[Command]
//public void CmdSpawnMyObject(EPlayerSelectedTeam selectedTeam)
//{
//    Debug.Log("CmdSpawnMyObject() selectedTeam: " + selectedTeam);
//    GameObject prefab = null;
//    if (selectedTeam == EPlayerSelectedTeam.Hunters)
//    {
//        prefab = Hunter;
//    }
//    else if (selectedTeam == EPlayerSelectedTeam.Runners)
//    {
//        prefab = Runner;
//    }

//    // Instantiate your object here
//    Debug.Log("CmdSpawnMyObject() Instantiate prefab: " + prefab);
//    GameObject myObject = Instantiate(prefab);

//    // Spawn the object with client authority
//    Debug.Log("CmdSpawnMyObject() NetworkServer.Spawn(myObject, connectionToClient)");
//    NetworkServer.Spawn(myObject, connectionToClient);
//}

//public void Initialize()
//{
//    Debug.Log("PlayerEmpty Initialize() m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_lobbyUIID);
//    //m_isInstantiated = true;
//    //int playerIndex = GetComponentInParent<NetworkRoomPlayer>().index;
//    //Debug.Log("Player Index: " + playerIndex);
//    //if (!isLocalPlayer) return;
//    //Debug.Log("PlayerEmpty Update() m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_lobbyUIID);
//    //if (m_playerSelectedTeam == 1)
//    //{
//    //    Debug.LogError("Player is a Runner m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_lobbyUIID);
//    //    if (Runner == null) Debug.LogError("Runner is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_lobbyUIID);
//    //    GameObject playerCharacter = Instantiate(Runner);
//    //    if (playerCharacter == null) Debug.LogError("PlayerCharacter is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_lobbyUIID);
//    //    NetworkServer.Spawn(playerCharacter);
//    //}
//    //else if (m_playerSelectedTeam == 0)
//    //{
//    //    Debug.LogError("Player is a Hunter m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_lobbyUIID);
//    //    if (Hunter == null) Debug.LogError("Hunter is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_lobbyUIID);
//    //    GameObject playerCharacter = Instantiate(Hunter);
//    //    if (playerCharacter == null) Debug.LogError("PlayerCharacter is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_lobbyUIID);
//    //    NetworkServer.Spawn(playerCharacter);
//    //}
//    //else
//    //{
//    //    Debug.LogError("Player has no team selected m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_lobbyUIID);
//    //}
//}


//if (playerSelectedTeam == RoomManager.EPlayerSelectedTeam.Hunters)
//{
//    Debug.Log("Instanciate Hunter");
//    //GameObject playerCharacter = Instantiate(Hunter);
//    RoomManager.singleton.CmdSpawnMyObject(connectionToClient, Hunter);
//    //NetworkServer.Spawn(playerCharacter);
//    //playerCharacter.GetComponent<HunterGameObjectSpawner>().Initialize();
//    //RoomPlayer.GetComponent<LobbyUI>().gameObject.SetActive(false);
//    m_instanciated = true;

//}
//else if (playerSelectedTeam == RoomManager.EPlayerSelectedTeam.Runners)
//{
//    Debug.Log("Instanciate Runner");
//    if (Runner == null) Debug.LogError("Runner is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
//    //GameObject playerCharacter = Instantiate(Runner);
//    //if (playerCharacter == null) Debug.LogError("PlayerCharacter is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
//    //if (connectionToClient == null)
//    //{
//    //    Debug.LogError("playerCharacter does not have a NetworkIdentity component.");
//    //    return;
//    //}

//    //NetworkIdentity networkIdentity = playerCharacter?.GetComponent<NetworkIdentity>();
//    //if (networkIdentity == null)
//    //{
//    //    Debug.LogError("playerCharacter does not have a NetworkIdentity component or playerCharacter is null.");
//    //    return;
//    //}

//    //NetworkServer.Spawn(playerCharacter, gameObject);
//    RoomManager.singleton.CmdSpawnMyObject(connectionToClient, Runner);
//    //NetworkServer.Spawn(playerCharacter, gameObject);
//    //RoomPlayer.GetComponent<LobbyUI>().gameObject.SetActive(false);
//    m_instanciated = true;
//}



//public void TryToInstanciate()
//{
//    //Debug.Log("PlayerEmpty Initialize() m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
//    //if (RoomPlayer == null)
//    //{
//    //    Debug.LogWarning("RoomPlayer is null");
//    //    m_isRoomPlayerNull = true;

//    //    RoomPlayer = RoomManager.GetSelfLobbyUI(connectionToClient);

//    //    if (RoomPlayer == null)
//    //    {
//    //        Debug.LogError("RoomPlayer is null");
//    //        return;
//    //    }
//    //    //return;
//    //}

//    Debug.Log("PlayerEmpty Start() m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);

//    //Debug.Log("PlayerEmpty Start() RoomManager.m_currentPlayerSelectedTeam: " + RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);

//    //Debug.Log("PlayerEmpty Start() m_currentPlayerId: " + RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId);
//    Debug.Log("PlayerEmpty Start() s_playerTwoId: " + RoomManager.s_playerTwoId);
//    Debug.Log("PlayerEmpty Start() (RoomManager.s_playerTwoId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId): " + (RoomManager.s_playerTwoId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId));

//    if (RoomManager.s_playerOneId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId)
//    {
//        InstanciateCharacter(RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);
//    }
//    else if (RoomManager.s_playerTwoId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId)
//    {
//        // Find the othe PlayerEmpty in scene:
//        PlayerEmpty[] playerEmpties = FindObjectsOfType<PlayerEmpty>();
//        foreach (PlayerEmpty playerEmpty in playerEmpties)
//        {
//            if (playerEmpty.m_emptyPlayerId == 2)
//            {
//                playerEmpty.InstanciateCharacter(RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);
//            }
//        }

//        //Debug.Log("PlayerEmpty Start() RoomManager.m_playerTwoSelectedTeam: " + RoomManager.m_playerTwoSelectedTeam);
//        //InstanciateCharacter(RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);
//    }
//    else if (RoomManager.s_playerThreeId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId)
//    {
//        InstanciateCharacter(RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);
//    }
//    else if (RoomManager.s_playerFourId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId)
//    {
//        InstanciateCharacter(RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);
//    }
//}