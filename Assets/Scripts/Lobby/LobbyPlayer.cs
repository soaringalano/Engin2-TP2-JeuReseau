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

using UnityEngine;

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

        [SyncVar] private GameObject m_uIPlayerHunter = null;
        [SyncVar] private GameObject m_uIPlayerRunner = null;
        [SyncVar] private GameObject m_uIPlayerUnselected = null;
        [SerializeField] [SyncVar] private int m_playerSelectedUIIndex = 1;

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

            Debug.Log("Start() player index: " + index);
            if (index == 0)
            {
                m_uIPlayerHunter = UIPlayerOneHunter;
                m_uIPlayerRunner = UIPlayerOneRunner;
                m_uIPlayerUnselected = UIPlayerOneUnselected;
                m_playerSelectedUIIndex = m_playerOneSelectedUIIndex;
            }
            else if (index == 1)
            {
                m_uIPlayerHunter = UIPlayerTwoHunter;
                m_uIPlayerRunner = UIPlayerTwoRunner;
                m_uIPlayerUnselected = UIPlayerTwoUnselected;
                m_playerSelectedUIIndex = m_playerTwoSelectedUIIndex;
            }
            else if (index == 2)
            {
                m_uIPlayerHunter = UIPlayerThreeHunter;
                m_uIPlayerRunner = UIPlayerThreeRunner;
                m_uIPlayerUnselected = UIPlayerThreeUnselected;
                m_playerSelectedUIIndex = m_playerThreeSelectedUIIndex;
            }
            else if (index == 3)
            {
                m_uIPlayerHunter = UIPlayerFourHunter;
                m_uIPlayerRunner = UIPlayerFourRunner;
                m_uIPlayerUnselected = UIPlayerFourUnselected;
                m_playerSelectedUIIndex = m_playerFourSelectedUIIndex;
            }
        }

        private void Update()
        {
            // Get player's current index
            if (!isLocalPlayer) return;
            //Debug.Log("Index: " + index);

            PlayerControlUpdate();
            //UpdateActiveUI();
            Debug.Log("Current Index: " + m_playerSelectedUIIndex + " Player Id: " + index);

        }

        private void PlayerControlUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if ((m_playerSelectedUIIndex - 1) < 0) return;
                //int oldIndex = m_playerSelectedUIIndex;
                int newUiIndex = m_playerSelectedUIIndex - 1;
                //m_playerSelectedUIIndex--;
                CmdChangePlayerUIIndex(newUiIndex);
                //Debug.Log("Current Index: " + (m_playerSelectedUIIndex) + " Player Id: "+ index);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if ((m_playerSelectedUIIndex + 1) > 2) return;
                //int oldIndex = m_playerSelectedUIIndex;
                int newUiIndex = m_playerSelectedUIIndex + 1;
                //m_playerSelectedUIIndex++;
                CmdChangePlayerUIIndex(newUiIndex);
                //Debug.Log("Current Index: " + (m_playerSelectedUIIndex) + " Player Id: " + index);
                //OnSelectedUIIndexChanged(oldIndex, m_playerSelectedUIIndex);
            }
            //else
            //{
            //    OnSelectedUIIndexChanged(m_playerSelectedUIIndex, m_playerSelectedUIIndex);
            //}
        }

        [Command]
        public void CmdChangePlayerUIIndex(int newIndex)
        {
            m_playerSelectedUIIndex = newIndex;
        }

        public override void OnGUI()
        {
            base.OnGUI();
        }
    }
}