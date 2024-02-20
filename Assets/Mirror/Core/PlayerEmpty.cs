using UnityEngine;

using static Mirror.RoomManager;

namespace Mirror
{
    public class PlayerEmpty : NetworkBehaviour
    {
        public GameObject RoomPlayer { get; set;}
        [field : SerializeField] private GameObject Runner { get; set; }
        [field : SerializeField] private GameObject Hunter { get; set; }

        [SerializeField] public int m_playerSelectedTeam = 2;

        private static int m_emptyPlayerIdIterator = 0;
        [SerializeField] public int m_emptyPlayerId = 0;

        private bool m_isInstantiated = false;
        private bool m_isRoomPlayerNull = false;

        private void Start()
        {
            m_emptyPlayerIdIterator++;
            m_emptyPlayerId = m_emptyPlayerIdIterator;
            TryInstanciatePlayerCharacter();
        }

        public void TryInstanciatePlayerCharacter()
        {
            if (RoomPlayer == null)
            {
                Debug.LogWarning("RoomPlayer is null");
                m_isRoomPlayerNull = true;

                RoomPlayer = RoomManager.GetSelfLobbyUI(connectionToClient);

                if (RoomPlayer == null)
                {
                    Debug.LogError("RoomPlayer is null");
                    return;
                }
                //return;
            }

            Debug.LogWarning("PlayerEmpty Start() m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);

            Debug.Log("PlayerEmpty Start() RoomManager.m_currentPlayerSelectedTeam: " + RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);

            Debug.Log("PlayerEmpty Start() m_currentPlayerId: " + RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId);
            Debug.Log("PlayerEmpty Start() s_playerTwoId: " + RoomManager.s_playerTwoId);
            Debug.Log("PlayerEmpty Start() (RoomManager.s_playerTwoId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId): " + (RoomManager.s_playerTwoId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId));

            if (RoomManager.s_playerOneId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId)
            {
                InstanciateCharacter(RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);
            }
            else if (RoomManager.s_playerTwoId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId)
            {
                // Find the othe PlayerEmpty in scene:
                PlayerEmpty[] playerEmpties = FindObjectsOfType<PlayerEmpty>();
                foreach (PlayerEmpty playerEmpty in playerEmpties)
                {
                    if (playerEmpty.m_emptyPlayerId == 2)
                    {
                        playerEmpty.InstanciateCharacter(RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);
                    }
                }

                //Debug.Log("PlayerEmpty Start() RoomManager.m_playerTwoSelectedTeam: " + RoomManager.m_playerTwoSelectedTeam);
                //InstanciateCharacter(RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);
            }
            else if (RoomManager.s_playerThreeId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId)
            {
                InstanciateCharacter(RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);
            }
            else if (RoomManager.s_playerFourId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId)
            {
                InstanciateCharacter(RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);
            }
        }

        private void InstanciateCharacter(EPlayerSelectedTeam playerSelectedTeam)
        {
            Debug.Log("(playerOneSelectedTeam == RoomManager.EPlayerSelectedTeam.Runners)" + (playerSelectedTeam == RoomManager.EPlayerSelectedTeam.Runners));
            if (playerSelectedTeam == RoomManager.EPlayerSelectedTeam.Hunters)
            {
                Debug.Log("Instanciate Hunter");
                GameObject playerCharacter = Instantiate(Hunter);
                NetworkServer.Spawn(playerCharacter);
                playerCharacter.GetComponent<HunterGameObjectSpawner>().Initialize();
                RoomPlayer.GetComponent<LobbyUI>().gameObject.SetActive(false);

            }
            else if (playerSelectedTeam == RoomManager.EPlayerSelectedTeam.Runners)
            {
                Debug.Log("Instanciate Runner");
                if (Runner == null) Debug.LogError("Runner is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
                GameObject playerCharacter = Instantiate(Runner);
                if (playerCharacter == null) Debug.LogError("PlayerCharacter is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
                if (connectionToClient == null)
                {
                    Debug.LogError("playerCharacter does not have a NetworkIdentity component.");
                    return;
                }

                NetworkIdentity networkIdentity = playerCharacter?.GetComponent<NetworkIdentity>();
                if (networkIdentity == null)
                {
                    Debug.LogError("playerCharacter does not have a NetworkIdentity component or playerCharacter is null.");
                    return;
                }

                NetworkServer.Spawn(playerCharacter, gameObject);
                RoomPlayer.GetComponent<LobbyUI>().gameObject.SetActive(false);
            }
        }

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

        private void Update()
        {
            if (m_isRoomPlayerNull)
            {

            }

            if (!m_isInstantiated) return;
        }


    }
}