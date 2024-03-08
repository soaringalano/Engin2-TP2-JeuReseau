using Mirror;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

namespace Mirror
{
    [AddComponentMenu("")]
    public class RoomManager : NetworkRoomManager
    {
        //[Header("Spawner1 Setup")]
        //[Tooltip("Reward Prefab for the Spawner1")]
        //public GameObject rewardPrefab;

        public enum EPlayerSelectedTeam
        {
            Hunters,
            Runners,
            Count
        }

        public static EPlayerSelectedTeam m_playerOneSelectedTeam = EPlayerSelectedTeam.Count;
        public static EPlayerSelectedTeam m_playerTwoSelectedTeam = EPlayerSelectedTeam.Count;
        public static EPlayerSelectedTeam m_playerThreeSelectedTeam = EPlayerSelectedTeam.Count;
        public static EPlayerSelectedTeam m_playerFourSelectedTeam = EPlayerSelectedTeam.Count;

        public static NetworkConnectionToClient m_playerOneConnection;
        public static NetworkConnectionToClient m_playerTwoConnection;
        public static NetworkConnectionToClient m_playerThreeConnection;
        public static NetworkConnectionToClient m_playerFourConnection;

        public static int s_idIterator = 0;

        public static int s_playerOneId = 0;
        public static int s_playerTwoId = 0;
        public static int s_playerThreeId = 0;
        public static int s_playerFourId = 0;


        public static new RoomManager singleton { get; private set; }

        /// <summary>
        /// Runs on both Server and Client
        /// Networking is NOT initialized when this fires
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            singleton = this;
        }

        public override void Update()
        {
            base.Update();

            if (GetComponentInChildren<Image>().color.a == 1 && showStartButton == false)
            {
                ToggleStartButton(false);
            }
            else if (GetComponentInChildren<Image>().color.a == 0 && showStartButton == true)
            {
                ToggleStartButton(true);
            }
        }

        //public static void AssignRoomToPlayerEmpty(NetworkConnectionToClient conn, GameObject roomPlayer)
        //{
        //    Debug.LogWarning("AssignRoomToPlayerEmpty() identity: " + conn + " roomPlayer: " + roomPlayer.name);
        //    if (conn.identity.gameObject.GetComponent<PlayerEmpty>() == null) return;
        //    conn.identity.gameObject.GetComponent<PlayerEmpty>().RoomPlayer = roomPlayer.GetComponent<LobbyUI>().gameObject;
        //}

        //public static void InitializePlayerEmpty(NetworkConnectionToClient conn)
        //{
        //    if (conn.identity.gameObject.GetComponent<PlayerEmpty>() == null) return;
        //    conn.identity.gameObject.GetComponent<PlayerEmpty>().Initialize();
        //}

        public static void AssignTeamToPlayerEmpty(NetworkConnectionToClient conn, GameObject roomPlayer)
        {
            Debug.Log("AssignTeamToPlayerEmpty() identity: " + conn + " roomPlayer: " + roomPlayer.name + " roomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam: " + roomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam);
            //if (conn.identity.gameObject.GetComponent<PlayerEmpty>() == null) return;
            conn.identity.gameObject.GetComponent<PlayerEmpty>().m_currentPlayerSelectedTeam = roomPlayer.GetComponent<LobbyUI>().m_currentPlayerSelectedTeam;
        }

        public GameObject GetRoomPlayer(NetworkConnectionToClient conn)
        {
            foreach (NetworkRoomPlayer roomPlayer in roomSlots)
            {
                if (roomPlayer == null) continue;
                if (roomPlayer.connectionToClient == conn)
                {
                    return roomPlayer.gameObject;
                }
            }

            return null;
        }


        //public static GameObject GetSelfLobbyUI(NetworkConnectionToClient networkIdentity)
        //{
        //    Debug.Log("GetSelfLobbyUI() NetworkClient.connection.identity.gameObject: " + NetworkClient.connection.identity.gameObject.name);

        //    Debug.Log("singleton.pendingPlayers.Count: " + singleton.pendingPlayers.Count);

        //    foreach (PendingPlayer pendingPlayer in singleton.pendingPlayers)
        //    {
        //        if (pendingPlayer.conn == networkIdentity)
        //        {
        //            return pendingPlayer.roomPlayer.gameObject;
        //        }
        //    }

        //    return null;
        //    //Debug.Log("pendingPlayer.roomPlayer.name" + pendingPlayer.roomPlayer.name);
        //    //return NetworkClient.connection.identity.gameObject.GetComponent<LobbyUI>().gameObject;
        //}

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
        }

        /// <summary>
        /// This is called on the server when a networked scene finishes loading.
        /// </summary>
        /// <param name="sceneName">Name of the new scene.</param>
        public override void OnRoomServerSceneChanged(string sceneName)
        {
            // spawn the initial batch of Rewards
            // if (sceneName == GameplayScene)
            //     Spawner1.InitialSpawn();
        }

        /// <summary>
        /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
        /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
        /// into the GamePlayer object as it is about to enter the Online scene.
        /// </summary>
        /// <param name="roomPlayer"></param>
        /// <param name="gamePlayer"></param>
        /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            return true;
        }

        public override void OnRoomStopClient()
        {
            base.OnRoomStopClient();
        }

        public override void OnRoomStopServer()
        {
            base.OnRoomStopServer();
        }

        /*
            This code below is to demonstrate how to do a Start button that only appears for the Host player
            showStartButton is a local bool that's needed because OnRoomServerPlayersReady is only fired when
            all players are ready, but if a player cancels their ready state there's no callback to set it back to false
            Therefore, allPlayersReady is used in combination with showStartButton to show/hide the Start button correctly.
            Setting showStartButton false when the button is pressed hides it in the game scene since NetworkRoomManager
            is set as DontDestroyOnLoad = true.
        */

        public bool showStartButton;

        public override void OnRoomServerPlayersReady()
        {
            // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
            if (Utils.IsHeadless())
            {
                base.OnRoomServerPlayersReady();
            }
            else
            {
                showStartButton = true;
                ToggleStartButton(true);
            }
        }

        private void ToggleStartButton(bool isVsible)
        {
            Button startButton = GetComponentInChildren<Button>();
            GetComponentInChildren<Button>().interactable = isVsible;

            Image image = startButton.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, isVsible ? 1 : 0);

            TextMeshProUGUI textMeshProUGUI = startButton.GetComponentInChildren<TextMeshProUGUI>();
            textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, isVsible ? 1 : 0);
        }

        public void OnStartButtonPressed()
        {
            if (!showStartButton) return;
            if (!allPlayersReady)
            {
                Debug.Log("All players are not ready");
                showStartButton = false;
                return;
            }

            if (!IsBothTeamPopulated())
            {
                Debug.Log("Both teams must be populated");
                return;
            }

            //Debug.Log("Both teams are populated");
            //Debug.Log("Start Game button pressed");
            // set to false to hide it in the game scene
            showStartButton = false;
            ToggleStartButton(false);

            //GetComponent<UIManager>().DisableLobbyUI();

            ServerChangeScene(GameplayScene);
        }


        private bool IsBothTeamPopulated()
        {
            int nbHunters = 0;
            int nbRunners = 0;
            int playerIndex = 0;

            foreach (NetworkRoomPlayer player in roomSlots)
            {
                if (player == null) continue;
                playerIndex++;
                //Debug.Log("Player " + playerIndex);
                if (player.GetComponent<LobbyUI>() == null) Debug.Log("LobbyPlayer is null");
                //if (player.GetComponent<LobbyUI>().m_playerSelectedTeam == LobbyUI.EPlayerSelectedTeam.Hunters) nbHunters++;
                //else if (player.GetComponent<LobbyUI>().m_playerSelectedTeam == LobbyUI.EPlayerSelectedTeam.Runners) nbRunners++;
                if (player.GetComponent<LobbyUI>().m_playerOneSelectedUIIndex == 0) nbHunters++;
                else if (player.GetComponent<LobbyUI>().m_playerOneSelectedUIIndex == 2) nbRunners++;

                if (player.GetComponent<LobbyUI>().m_playerTwoSelectedUIIndex == 0) nbHunters++;
                else if (player.GetComponent<LobbyUI>().m_playerTwoSelectedUIIndex == 2) nbRunners++;

                if (player.GetComponent<LobbyUI>().m_playerThreeSelectedUIIndex == 0) nbHunters++;
                else if (player.GetComponent<LobbyUI>().m_playerThreeSelectedUIIndex == 2) nbRunners++;

                if (player.GetComponent<LobbyUI>().m_playerFourSelectedUIIndex == 0) nbHunters++;
                else if (player.GetComponent<LobbyUI>().m_playerFourSelectedUIIndex == 2) nbRunners++;
            }

            //Debug.Log("nbHunters: " + nbHunters + " nbRunners: " + nbRunners + " nbPlayers: " + playerIndex);
            return nbHunters > 0 && nbRunners > 0;
        }

        //private void SetTeamOnNetwork()
        //{

        //    int playerIndex = 0;

        //    foreach (NetworkRoomPlayer player in roomSlots)
        //    {
        //        if (player == null) continue;
        //        int nbHunters = 0;
        //        int nbRunners = 0;
        //        playerIndex++;
        //        Debug.Log("Player " + playerIndex);
        //        if (player.GetComponent<LobbyUI>() == null) Debug.Log("LobbyPlayer is null");
        //        //if (player.GetComponent<LobbyUI>().m_playerSelectedTeam == LobbyUI.EPlayerSelectedTeam.Hunters) nbHunters++;
        //        //else if (player.GetComponent<LobbyUI>().m_playerSelectedTeam == LobbyUI.EPlayerSelectedTeam.Runners) nbRunners++;
        //        if (player.GetComponent<LobbyUI>().m_playerOneSelectedUIIndex == 0) nbHunters++;
        //        else if (player.GetComponent<LobbyUI>().m_playerOneSelectedUIIndex == 2) nbRunners++;

        //        if (player.GetComponent<LobbyUI>().m_playerTwoSelectedUIIndex == 0) nbHunters++;
        //        else if (player.GetComponent<LobbyUI>().m_playerTwoSelectedUIIndex == 2) nbRunners++;

        //        if (player.GetComponent<LobbyUI>().m_playerThreeSelectedUIIndex == 0) nbHunters++;
        //        else if (player.GetComponent<LobbyUI>().m_playerThreeSelectedUIIndex == 2) nbRunners++;

        //        if (player.GetComponent<LobbyUI>().m_playerFourSelectedUIIndex == 0) nbHunters++;
        //        else if (player.GetComponent<LobbyUI>().m_playerFourSelectedUIIndex == 2) nbRunners++;

        //        int playerSlot = 0;
        //        if(player.index == 0) playerSlot = 1;
        //        else if(player.index == 1) playerSlot = 10;
        //        else if(player.index == 2) playerSlot = 100;
        //        else if(player.index == 3) playerSlot = 1000;
        //        if (nbHunters > 0) playerSlot *= -1;
        //        else if(nbRunners > 0) playerSlot *= 1;
        //        m_playesTeamSelection += playerSlot;
        //    }

        //    Debug.Log("m_playesTeamSelection: " + m_playesTeamSelection);
        //}
    }
}
