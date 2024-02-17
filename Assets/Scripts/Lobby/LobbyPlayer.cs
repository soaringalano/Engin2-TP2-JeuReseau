//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//namespace Mirror
//{

//    [AddComponentMenu("")]
//    public class LobbyPlayer : NetworkRoomPlayer
//    {
//        private NetworkRoomManager m_room;

//        [field: SerializeField] private GameObject UIPlayerOneUnselected { get; set; }
//        [field: SerializeField] private GameObject UIPlayerTwoUnselected { get; set; }
//        [field: SerializeField] private GameObject UIPlayerThreeUnselected { get; set; }
//        [field: SerializeField] private GameObject UIPlayerFourUnselected { get; set; }

//        [field: SerializeField] private GameObject UIPlayerOneHunter { get; set; }
//        [field: SerializeField] private GameObject UIPlayerOneRunner { get; set; }

//        [field: SerializeField] private GameObject UIPlayerTwoHunter { get; set; }
//        [field: SerializeField] private GameObject UIPlayerTwoRunner { get; set; }

//        [field: SerializeField] private GameObject UIPlayerThreeHunter { get; set; }
//        [field: SerializeField] private GameObject UIPlayerThreeRunner { get; set; }

//        [field: SerializeField] private GameObject UIPlayerFourHunter { get; set; }
//        [field: SerializeField] private GameObject UIPlayerFourRunner { get; set; }
//        private GameObject UIPlayerHunter { get; set; }
//        private GameObject UIPlayerRunner { get; set; }
//        private GameObject UIPlayerUnselected { get; set; }

//        //[SyncVar(hook = nameof(OnSelectedUIIndexChanged))]
//        private int m_playerSelectedUIIndex = 1;

//        private int m_playerOneSelectedUIIndex = 1;
//        private int m_playerTwoSelectedUIIndex = 1;
//        private int m_playerThreeSelectedUIIndex = 1;
//        private int m_playerFourSelectedUIIndex = 1;

//        public override void Start()
//        {
//            base.Start();
//            m_room = NetworkManager.singleton as NetworkRoomManager;
//            //Debug.Log("Client Index: " + clientIndex);
//        }

//        private void Update()
//        {
//            if (!isLocalPlayer) return;
//            AssignVariables();
//            //Debug.Log("m_room.clientIndex: " + m_room.clientIndex);
//            //Debug.Log("Index: " + index);
//            switch (index)
//            {
//                case 0:
//                    PlayerControlUpdate();
//                    UpdateActiveUI();
//                    break;
//                case 1:
//                    PlayerControlUpdate();
//                    UpdateActiveUI();
//                    break;
//                case 2:
//                    PlayerControlUpdate();
//                    UpdateActiveUI();
//                    break;
//                case 3:
//                    PlayerControlUpdate();
//                    UpdateActiveUI();
//                    break;
//            }
//        }

//        private void AssignVariables()
//        {
//            if (UIPlayerHunter != null) return;

//            switch (index)
//            {
//                case 0:
//                    Debug.Log("Player 1 index: " + index);
//                    Debug.Log("m_room.clientIndex: " + m_room.clientIndex);
//                    UIPlayerHunter = UIPlayerOneHunter;
//                    UIPlayerRunner = UIPlayerOneRunner;
//                    UIPlayerUnselected = UIPlayerOneUnselected;
//                    m_playerSelectedUIIndex = m_playerOneSelectedUIIndex;
//                    break;
//                case 1:
//                    Debug.Log("Player 2 index: " + index);
//                    Debug.Log("m_room.clientIndex: " + m_room.clientIndex);
//                    UIPlayerHunter = UIPlayerTwoHunter;
//                    UIPlayerRunner = UIPlayerTwoRunner;
//                    UIPlayerUnselected = UIPlayerTwoUnselected;
//                    m_playerSelectedUIIndex = m_playerTwoSelectedUIIndex;
//                    break;
//                case 2:
//                    Debug.Log("Player 3 index: " + index);
//                    Debug.Log("m_room.clientIndex: " + m_room.clientIndex);
//                    UIPlayerHunter = UIPlayerThreeHunter;
//                    UIPlayerRunner = UIPlayerThreeRunner;
//                    UIPlayerUnselected = UIPlayerThreeUnselected;
//                    m_playerSelectedUIIndex = m_playerThreeSelectedUIIndex;
//                    break;
//                case 3:
//                    Debug.Log("Player 4 index: " + index);
//                    Debug.Log("m_room.clientIndex: " + m_room.clientIndex);
//                    UIPlayerHunter = UIPlayerFourHunter;
//                    UIPlayerRunner = UIPlayerFourRunner;
//                    UIPlayerUnselected = UIPlayerFourUnselected;
//                    m_playerSelectedUIIndex = m_playerFourSelectedUIIndex;
//                    break;
//            }
//        }

//        private void PlayerControlUpdate()
//        {
//            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
//            {
//                if ((m_playerSelectedUIIndex - 1) < 0) return;
//                int oldIndex = m_playerSelectedUIIndex;
//                m_playerSelectedUIIndex--;
//                //Debug.Log("Current Index: " + (m_playerSelectedUIIndex));
//                //OnSelectedUIIndexChanged(oldIndex, m_playerSelectedUIIndex - 1);
//            }
//            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
//            {
//                if ((m_playerSelectedUIIndex + 1) > 2) return;
//                int oldIndex = m_playerSelectedUIIndex;
//                m_playerSelectedUIIndex++;
//                //Debug.Log("Current Index: " + (m_playerSelectedUIIndex));
//                //OnSelectedUIIndexChanged(oldIndex, m_playerSelectedUIIndex);
//            }
//            //else
//            //{
//            //    OnSelectedUIIndexChanged(m_playerSelectedUIIndex, m_playerSelectedUIIndex);
//            //}
//        }

//        //private void OnSelectedUIIndexChanged(int oldIndex, int newIndex)
//        //{
//        //    UpdateActiveUI(newIndex);
//        //}

//        private void UpdateActiveUI()
//        {
//            if (m_playerSelectedUIIndex == 0 && UIPlayerHunter.activeSelf == false)
//            {
//                UIPlayerHunter.SetActive(true);
//                UIPlayerRunner.SetActive(false);
//                UIPlayerUnselected.SetActive(false);
//                //Debug.Log("Player Hunter selected by player " + index);
//            }
//            else if (m_playerSelectedUIIndex == 1 && UIPlayerUnselected.activeSelf == false)
//            {
//                UIPlayerHunter.SetActive(false);
//                UIPlayerUnselected.SetActive(true);
//                UIPlayerRunner.SetActive(false);
//                //Debug.Log("Player Unselected selected by player " + index);
//            }
//            else if (m_playerSelectedUIIndex == 2 && UIPlayerRunner.activeSelf == false)
//            {
//                UIPlayerHunter.SetActive(false);
//                UIPlayerUnselected.SetActive(false);
//                UIPlayerRunner.SetActive(true);
//                //Debug.Log("Player Runner selected by player " + index);
//            }
//        }

//        public void OnReadyChecked()
//        {
//            CmdChangeReadyState(true);
//        }

//        public void OnReadyUnchecked()
//        {
//            CmdChangeReadyState(false);
//        }

//        //public override void OnClientEnterRoom() 
//        //{ 
//        //    base.OnClientEnterRoom();
//        //    //NetworkRoomManager room = NetworkManager.singleton as NetworkRoomManager;
//        //    //room.RecalculateRoomPlayerIndices();
//        //}

//        //public override void IndexChanged(int oldIndex, int newIndex)
//        //{
//        //    base.IndexChanged(oldIndex, newIndex);
//        //}


//        //public override void OnStartLocalPlayer()
//        //{
//        //    base.OnStartLocalPlayer();

//        //    if (isLocalPlayer)
//        //    {
//        //        if (NetworkManager.singleton is NetworkRoomManager room)
//        //        {
//        //            Debug.Log("Local Player Index: " + index);
//        //            room.RecalculateRoomPlayerIndices();
//        //            Debug.Log("Local Player Index: " + index);
//        //        }
//        //    }
//        //}
//    }
//}

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class LobbyPlayer : NetworkRoomPlayer
    {
        [field: SerializeField] private GameObject UIPlayerOneUnselected { get; set; }
        [field: SerializeField] private GameObject UIPlayerTwoUnselected { get; set; }
        [field: SerializeField] private GameObject UIPlayerThreeUnselected { get; set; }
        [field: SerializeField] private GameObject UIPlayerFourUnselected { get; set; }

        [field: SerializeField] private GameObject UIPlayerOneHunter { get; set; }
        [field: SerializeField] private GameObject UIPlayerOneRunner { get; set; }

        [field: SerializeField] private GameObject UIPlayerTwoHunter { get; set; }
        [field: SerializeField] private GameObject UIPlayerTwoRunner { get; set; }

        [field: SerializeField] private GameObject UIPlayerThreeHunter { get; set; }
        [field: SerializeField] private GameObject UIPlayerThreeRunner { get; set; }

        [field: SerializeField] private GameObject UIPlayerFourHunter { get; set; }
        [field: SerializeField] private GameObject UIPlayerFourRunner { get; set; }

        //[SerializeField][SyncVar] private GameObject m_uIPlayerHunter = null;
        //[SerializeField][SyncVar] private GameObject m_uIPlayerRunner = null;
        //[SerializeField][SyncVar] private GameObject m_uIPlayerUnselected = null;
        //[SerializeField][SyncVar] private int m_playerSelectedUIIndex = 1;

        private int m_playerOneSelectedUIIndex = 1;
        private int m_playerTwoSelectedUIIndex = 1;
        private int m_playerThreeSelectedUIIndex = 1;
        private int m_playerFourSelectedUIIndex = 1;

        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
        }

        public override void OnClientEnterRoom()
        {
            //Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");
        }

        public override void OnClientExitRoom()
        {
            //Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            //Debug.Log($"IndexChanged {newIndex}");
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            //Debug.Log($"ReadyStateChanged {newReadyState}");
        }

        public override void Start()
        {
            base.Start();

            if (!isLocalPlayer) return;
            Image paneBackGround = GetComponentInChildren<Image>();
            if (paneBackGround.name != "PanelBackground") Debug.LogError("PaneBackground not found in LobbyPlayer, name is: " + paneBackGround.name);
            else Debug.Log("PaneBackground found in LobbyPlayer, name is: " + paneBackGround.name);
            // set alpha to 1
            paneBackGround.color = new Color(paneBackGround.color.r, paneBackGround.color.g, paneBackGround.color.b, 1);

            //if (!isLocalPlayer) return;
            //if (isLocalPlayer)
            //{
            //Initialize();
            //}
            //else
            //{
            //    InitializeOnServer();
            //}

        }

        //private void InitializeOnServer()
        //{
        //    Debug.Log("Start() player index: " + index);
        //    if (index == 0)
        //    {
        //        CmdSetUIPlayerHunter(UIPlayerOneHunter);
        //        CmdSetUIPlayerRunner(UIPlayerOneRunner);
        //        CmdSetUIPlayerUnselected(UIPlayerOneUnselected);
        //    }
        //    else if (index == 1)
        //    {
        //        CmdSetUIPlayerHunter(UIPlayerTwoHunter);
        //        CmdSetUIPlayerRunner(UIPlayerTwoRunner);
        //        CmdSetUIPlayerUnselected(UIPlayerTwoUnselected);
        //    }
        //    else if (index == 2)
        //    {
        //        CmdSetUIPlayerHunter(UIPlayerThreeHunter);
        //        CmdSetUIPlayerRunner(UIPlayerThreeRunner);
        //        CmdSetUIPlayerUnselected(UIPlayerThreeUnselected);
        //    }
        //    else if (index == 3)
        //    {
        //        CmdSetUIPlayerHunter(UIPlayerFourHunter);
        //        CmdSetUIPlayerRunner(UIPlayerFourRunner);
        //        CmdSetUIPlayerUnselected(UIPlayerFourUnselected);
        //    }
        //}

        //private void Initialize()
        //{
        //    base.Start();
        //    if (!isLocalPlayer) return;

        //    Debug.Log("Start() player index: " + index);
        //    if (index == 0)
        //    {
        //        m_uIPlayerHunter = UIPlayerOneHunter;
        //        m_uIPlayerRunner = UIPlayerOneRunner;
        //        m_uIPlayerUnselected = UIPlayerOneUnselected;
        //    }
        //    else if (index == 1)
        //    {
        //        m_uIPlayerHunter = UIPlayerTwoHunter;
        //        m_uIPlayerRunner = UIPlayerTwoRunner;
        //        m_uIPlayerUnselected = UIPlayerTwoUnselected;
        //    }
        //    else if (index == 2)
        //    {
        //        m_uIPlayerHunter = UIPlayerThreeHunter;
        //        m_uIPlayerRunner = UIPlayerThreeRunner;
        //        m_uIPlayerUnselected = UIPlayerThreeUnselected;
        //    }
        //    else if (index == 3)
        //    {
        //        m_uIPlayerHunter = UIPlayerFourHunter;
        //        m_uIPlayerRunner = UIPlayerFourRunner;
        //        m_uIPlayerUnselected = UIPlayerFourUnselected;
        //    }
        //}

        private void Update()
        {
            // Get player's current index
            if (!isLocalPlayer) return;
            //Debug.Log("Index: " + index);

            switch (index)
            {
                case 0:
                    PlayerOneControlUpdate();
                    UpdateActivePlayerOneUI();
                    break;
                case 1:
                    PlayerTwoControlUpdate();
                    UpdateActivePlayerTwoUI();
                    break;
                //case 2:
                //    PlayerThreeControlUpdate();
                //    UpdateActivePlayerThreeUI();
                //    break;
                //case 3:
                //    PlayerFourControlUpdate();
                //    UpdateActivePlayerFourUI();
                //    break;
            }

        }

        private void PlayerOneControlUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if ((m_playerOneSelectedUIIndex - 1) < 0) return;
                int newUiIndex = m_playerOneSelectedUIIndex - 1;
                CmdChangePlayerOneUIIndex(newUiIndex);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if ((m_playerOneSelectedUIIndex + 1) > 2) return;
                int newUiIndex = m_playerOneSelectedUIIndex + 1;
                CmdChangePlayerOneUIIndex(newUiIndex);
            }
        }

        private void PlayerTwoControlUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if ((m_playerTwoSelectedUIIndex - 1) < 0) return;
                int newUiIndex = m_playerTwoSelectedUIIndex - 1;
                CmdChangePlayerTwoUIIndex(newUiIndex);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if ((m_playerTwoSelectedUIIndex + 1) > 2) return;
                int newUiIndex = m_playerTwoSelectedUIIndex + 1;
                CmdChangePlayerTwoUIIndex(newUiIndex);
            }
        }

        //private void PlayerThreeControlUpdate()
        //{
        //    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        //    {
        //        if ((m_playerThreeSelectedUIIndex - 1) < 0) return;
        //        int newUiIndex = m_playerThreeSelectedUIIndex - 1;
        //        CmdChangePlayerThreeUIIndex(newUiIndex);
        //    }
        //    else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        //    {
        //        if ((m_playerThreeSelectedUIIndex + 1) > 2) return;
        //        int newUiIndex = m_playerThreeSelectedUIIndex + 1;
        //        CmdChangePlayerThreeUIIndex(newUiIndex);
        //    }
        //}

        //private void PlayerFourControlUpdate()
        //{
        //    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        //    {
        //        if ((m_playerFourSelectedUIIndex - 1) < 0) return;
        //        int newUiIndex = m_playerFourSelectedUIIndex - 1;
        //        CmdChangePlayerFourUIIndex(newUiIndex);
        //    }
        //    else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        //    {
        //        if ((m_playerFourSelectedUIIndex + 1) > 2) return;
        //        int newUiIndex = m_playerFourSelectedUIIndex + 1;
        //        CmdChangePlayerFourUIIndex(newUiIndex);
        //    }
        //}

        private void UpdateActivePlayerOneUI()
        {
            if (m_playerOneSelectedUIIndex == 0 && UIPlayerOneHunter.activeSelf == false)
            {
                CmdSetUIPlayerOneHunterActive(true);
                CmdSetUIPlayerOneUnselectedActive(false);
                CmdSetUIPlayerOneRunnerActive(false);
                //Debug.Log("Player Hunter selected by player " + index);
            }
            else if (m_playerOneSelectedUIIndex == 1 && UIPlayerOneUnselected.activeSelf == false)
            {
                CmdSetUIPlayerOneHunterActive(false);
                CmdSetUIPlayerOneUnselectedActive(true);
                CmdSetUIPlayerOneRunnerActive(false);
                //Debug.Log("Player Unselected selected by player " + index);
            }
            else if (m_playerOneSelectedUIIndex == 2 && UIPlayerOneRunner.activeSelf == false)
            {
                CmdSetUIPlayerOneHunterActive(false);
                CmdSetUIPlayerOneUnselectedActive(false);
                CmdSetUIPlayerOneRunnerActive(true);
                //Debug.Log("Player Runner selected by player " + index);
            }
        }

        private void UpdateActivePlayerTwoUI()
        {
            if (m_playerTwoSelectedUIIndex == 0 && UIPlayerTwoHunter.activeSelf == false)
            {
                CmdSetUIPlayerTwoHunterActive(true);
                CmdSetUIPlayerTwoUnselectedActive(false);
                CmdSetUIPlayerTwoRunnerActive(false);
                //Debug.Log("Player Hunter selected by player " + index);
            }
            else if (m_playerTwoSelectedUIIndex == 1 && UIPlayerTwoUnselected.activeSelf == false)
            {
                CmdSetUIPlayerTwoHunterActive(false);
                CmdSetUIPlayerTwoUnselectedActive(true);
                CmdSetUIPlayerTwoRunnerActive(false);
                //Debug.Log("Player Unselected selected by player " + index);
            }
            else if (m_playerTwoSelectedUIIndex == 2 && UIPlayerTwoRunner.activeSelf == false)
            {
                CmdSetUIPlayerTwoHunterActive(false);
                CmdSetUIPlayerTwoUnselectedActive(false);
                CmdSetUIPlayerTwoRunnerActive(true);
                //Debug.Log("Player Runner selected by player " + index);
            }
        }

        //private void UpdateActiveUIToServer()
        //{
        //    //if (m_uIPlayerHunter == null) Debug.LogError("m_uIPlayerHunter is null");
        //    //if (m_uIPlayerUnselected == null) Debug.LogError("m_uIPlayerUnselected is null");
        //    //if (m_uIPlayerRunner == null) Debug.LogError("m_uIPlayerRunner is null");

        //    if (m_playerSelectedUIIndex == 0 && m_uIPlayerHunter.activeSelf == false)
        //    {
        //        CmdSetUIPlayerHunterActive(true);
        //        CmdSetUIPlayerUnselectedActive(false);
        //        CmdSetUIPlayerRunnerActive(false);
        //        //Debug.Log("Player Hunter selected by player " + index);
        //    }
        //    else if (m_playerSelectedUIIndex == 1 && m_uIPlayerUnselected.activeSelf == false)
        //    {
        //        CmdSetUIPlayerHunterActive(false);
        //        CmdSetUIPlayerUnselectedActive(true);
        //        CmdSetUIPlayerRunnerActive(false);
        //        //Debug.Log("Player Unselected selected by player " + index);
        //    }
        //    else if (m_playerSelectedUIIndex == 2 && m_uIPlayerRunner.activeSelf == false)
        //    {
        //        CmdSetUIPlayerHunterActive(false);
        //        CmdSetUIPlayerUnselectedActive(false);
        //        CmdSetUIPlayerRunnerActive(true);
        //        //Debug.Log("Player Runner selected by player " + index);
        //    }
        //}

        [Command]
        public void CmdChangePlayerOneUIIndex(int newIndex)
        {
            m_playerOneSelectedUIIndex = newIndex;
        }

        public void ChangePlayerOneUIIndex(int newIndex)
        {
            m_playerOneSelectedUIIndex = newIndex;
        }

        [Command]
        private void CmdSetUIPlayerOneHunterActive(bool isActive)
        {
            UIPlayerOneHunter.SetActive(isActive);
        }

        [Command]
        private void CmdSetUIPlayerOneRunnerActive(bool isActive)
        {
            UIPlayerOneRunner.SetActive(isActive);
        }

        [Command]
        private void CmdSetUIPlayerOneUnselectedActive(bool isActive)
        {
            UIPlayerOneUnselected.SetActive(isActive);
        }

        public void CmdChangePlayerTwoUIIndex(int newIndex)
        {
            m_playerTwoSelectedUIIndex = newIndex;
        }

        [Command]
        private void CmdSetUIPlayerTwoHunterActive(bool isActive)
        {
            UIPlayerTwoHunter.SetActive(isActive);
        }

        [Command]
        private void CmdSetUIPlayerTwoRunnerActive(bool isActive)
        {
            UIPlayerTwoRunner.SetActive(isActive);
        }

        [Command]
        private void CmdSetUIPlayerTwoUnselectedActive(bool isActive)
        {
            UIPlayerTwoUnselected.SetActive(isActive);
        }

        //[Command]
        //private void CmdSetUIPlayerHunter(GameObject _gameObject)
        //{
        //    m_uIPlayerHunter = _gameObject;
        //}

        //[Command]
        //private void CmdSetUIPlayerRunner(GameObject _gameObject)
        //{
        //    m_uIPlayerRunner = _gameObject;
        //}

        //[Command]
        //private void CmdSetUIPlayerUnselected(GameObject _gameObject)
        //{
        //    m_uIPlayerUnselected = _gameObject;
        //}


        public override void OnGUI()
        {
            base.OnGUI();
        }
    }
}