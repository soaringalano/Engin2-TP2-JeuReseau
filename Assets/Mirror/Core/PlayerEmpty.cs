using UnityEngine;
using static Mirror.LobbyUI;
using static Mirror.NetworkRoomPlayer;
using static Mirror.RoomManager;

namespace Mirror
{
    public class PlayerEmpty : NetworkBehaviour
    {
        public GameObject RoomPlayer { get; set;}
        [field : SerializeField] private GameObject Runner { get; set; }
        [field : SerializeField] private GameObject Hunter { get; set; }
        //private SceneReferencer sceneReferencer;

        //        public override void OnStartAuthority()
        //        {
        //            // enable UI located in the scene, after empty player spawns in.
        //#if UNITY_2021_3_OR_NEWER
        //            //sceneReferencer = GameObject.FindAnyObjectByType<SceneReferencer>();
        //#else
        //            // Deprecated in Unity 2023.1
        //            //sceneReferencer = GameObject.FindObjectOfType<SceneReferencer>();
        //#endif
        //            //sceneReferencer.GetComponent<Canvas>().enabled = true;
        //        }

        [SerializeField] public int m_playerSelectedTeam = 2;

        private static int m_emptyPlayerIdIterator = 0;
        [SerializeField] public int m_emptyPlayerId = 0;

        private bool m_isInstantiated = false;
        private bool m_isRoomPlayerNull = false;

        private void Start()
        {
            m_emptyPlayerIdIterator++;
            m_emptyPlayerId = m_emptyPlayerIdIterator;
            if (!isLocalPlayer) return;
            Debug.LogWarning("PlayerEmpty Start() m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);

            //m_playerSelectedTeam = RoomManager.singleton.
            //int playerIndex = RoomManager.singleton.currentPlayerIndex;
            
            Debug.Log("PlayerEmpty Start() m_playerOneConnection: " + RoomManager.s_playerOneId);
            Debug.Log("PlayerEmpty Start() m_playerTwoConnection: " + RoomManager.s_playerTwoId);
            Debug.Log("PlayerEmpty Start() m_playerThreeConnection: " + RoomManager.s_playerThreeId);
            Debug.Log("PlayerEmpty Start() m_playerFourConnection: " + RoomManager.s_playerFourId);

            if (RoomPlayer == null)
            {
                Debug.LogWarning("RoomPlayer is null");
                m_isRoomPlayerNull = true;
                return;
            }

            if (RoomManager.s_playerOneId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId)
            {
                InstanciateCharacter(RoomManager.m_playerOneSelectedTeam);
            }
            else if (RoomManager.s_playerTwoId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId)
            {
                InstanciateCharacter(RoomManager.m_playerTwoSelectedTeam);
            }
            else if (RoomManager.s_playerThreeId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId)
            {
                InstanciateCharacter(RoomManager.m_playerThreeSelectedTeam);
            }
            else if (RoomManager.s_playerFourId == RoomPlayer.GetComponent<LobbyUI>().m_currentPlayerId)
            {
                InstanciateCharacter(RoomManager.m_playerFourSelectedTeam);
            }
        }

        private void InstanciateCharacter(EPlayerSelectedTeam playerOneSelectedTeam)
        {
            if (playerOneSelectedTeam == RoomManager.EPlayerSelectedTeam.Hunters)
            {
                Debug.Log("Instanciate Hunter");
                GameObject playerCharacter = Instantiate(Hunter);
                NetworkServer.Spawn(playerCharacter);
                playerCharacter.GetComponent<HunterGameObjectSpawner>().Initialize();
            }
            else if (playerOneSelectedTeam == RoomManager.EPlayerSelectedTeam.Runners)
            {
                Debug.Log("Instanciate Runner");
                GameObject playerCharacter = Instantiate(Runner);
                NetworkServer.Spawn(playerCharacter);
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