
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
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

        //private int m_playerOneSelectedUIIndex = 1;
        //private int m_playerTwoSelectedUIIndex = 1;
        //private int m_playerThreeSelectedUIIndex = 1;
        //private int m_playerFourSelectedUIIndex = 1;

        [SerializeField]
        [SyncVar(hook = nameof(CmdChangePlayerOneUIIndex))] private int m_playerOneSelectedUIIndex = 1;
        [SerializeField]
        [SyncVar(hook = nameof(CmdChangePlayerTwoUIIndex))] private int m_playerTwoSelectedUIIndex = 1;
        [SerializeField]
        [SyncVar(hook = nameof(CmdChangePlayerThreeUIIndex))] private int m_playerThreeSelectedUIIndex = 1;
        [SerializeField]
        [SyncVar(hook = nameof(CmdChangePlayerFourUIIndex))] private int m_playerFourSelectedUIIndex = 1;

        private bool m_playerArrivedInLobby = false;

        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
            // log player index and child index
            //Debug.Log("LobbyPlayer OnStartClient player index: " + index + " child index: " + transform.GetSiblingIndex());
            //base.OnStartClient();
            //InitializeSynVars();

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
            Debug.Log($"IndexChanged {newIndex}");
            m_playerArrivedInLobby = true;
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            //Debug.Log($"ReadyStateChanged {newReadyState}");
        }

        public override void Start()
        {
            // log player index and child index
            Debug.Log("LobbyPlayer Start player index: " + index + " child index: " + transform.GetSiblingIndex());
            base.Start();

            if (!isLocalPlayer) return;
            Image paneBackGround = GetComponentInChildren<Image>();
            if (paneBackGround.name != "PanelBackground") Debug.LogError("PaneBackground not found in LobbyPlayer, name is: " + paneBackGround.name);
            else Debug.Log("PaneBackground found in LobbyPlayer, name is: " + paneBackGround.name);
            // set alpha to 1
            paneBackGround.color = new Color(paneBackGround.color.r, paneBackGround.color.g, paneBackGround.color.b, 1);

            //UIPlayerOneUnselected = GameObject.Find("Player1");
            //UIPlayerTwoUnselected = GameObject.Find("Player2");
            //UIPlayerThreeUnselected = GameObject.Find("Player3");
            //UIPlayerFourUnselected = GameObject.Find("Player4");
            //UIPlayerOneHunter = GameObject.Find("HunterP1");
            //UIPlayerTwoHunter = GameObject.Find("HunterP2");
            //UIPlayerThreeHunter = GameObject.Find("HunterP3");
            //UIPlayerFourHunter = GameObject.Find("HunterP4");
            //UIPlayerOneRunner = GameObject.Find("RunnerP1");
            //UIPlayerTwoRunner = GameObject.Find("RunnerP2");
            //UIPlayerThreeRunner = GameObject.Find("RunnerP3");
            //UIPlayerFourRunner = GameObject.Find("RunnerP4");
        }


        private void Update()
        {
            if (m_playerArrivedInLobby == false && isClientOnly) return;
            // log player index and child index
            //Debug.Log("LobbyPlayer Update player index: " + index + " child index: " + transform.GetSiblingIndex());
            // Get player's current index
            if (isLocalPlayer)
            {

                Debug.Log("LobbyPlayer Update player index: " + index + " child index: " + (transform.GetSiblingIndex()-1));
                switch (index)
                {
                    case 0:
                        //Debug.Log("Player 1");
                        PlayerControlUpdate(m_playerOneSelectedUIIndex);
                        UpdateActivePlayerUI(m_playerOneSelectedUIIndex);
                        break;
                    case 1:
                        //Debug.Log("Player 2");
                        PlayerControlUpdate(m_playerTwoSelectedUIIndex);
                        UpdateActivePlayerUI(m_playerTwoSelectedUIIndex);
                        break;
                    case 2:
                        //Debug.Log("Player 3");
                        PlayerControlUpdate(m_playerThreeSelectedUIIndex);
                        UpdateActivePlayerUI(m_playerThreeSelectedUIIndex);
                        break;
                    case 3:
                        //Debug.Log("Player 4");
                        PlayerControlUpdate(m_playerFourSelectedUIIndex);
                        UpdateActivePlayerUI(m_playerFourSelectedUIIndex);
                        break;
                }
            } 
            else if (!isLocalPlayer)
            {
                //Debug.Log("Index: " + index);
                switch (index)
                {
                    case 0:
                        //Debug.Log("Player 1");
                        UpdateActivePlayerUI(m_playerOneSelectedUIIndex);
                        break;
                    case 1:
                        //Debug.Log("Player 2");
                        UpdateActivePlayerUI(m_playerTwoSelectedUIIndex);
                        break;
                    case 2:
                        //Debug.Log("Player 3");
                        UpdateActivePlayerUI(m_playerThreeSelectedUIIndex);
                        break;
                    case 3:
                        //Debug.Log("Player 4");
                        UpdateActivePlayerUI(m_playerFourSelectedUIIndex);
                        break;
                }
            }
        }

        private void PlayerControlUpdate(int playerSelectionUIIndex)
        {
            //int playerSelectionUIIndex;
            //GetCurrentPlayerIndex(out playerSelectionUIIndex);
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if ((playerSelectionUIIndex - 1) < 0) return;
                int newUiIndex = playerSelectionUIIndex - 1;
                ChangePlayerUIIndex(playerSelectionUIIndex, newUiIndex);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if ((playerSelectionUIIndex + 1) > 2) return;
                int newUiIndex = playerSelectionUIIndex + 1;
                ChangePlayerUIIndex(playerSelectionUIIndex, newUiIndex);
            }
        }

        private void UpdateActivePlayerUI(int playerSelectionUIIndex)
        {
            //int playerSelectionUIIndex;
            //GetCurrentPlayerIndex(out playerSelectionUIIndex);
            GameObject uIPlayerHunter, uIPlayerUnselected, uIPlayerRunner;
            GetCurrentPlayerGO(out uIPlayerHunter, out uIPlayerUnselected, out uIPlayerRunner);
            //Debug.Log(" ");
            //Debug.Log("m_playerSelectedUIIndex == 0: " + (playerSelectionUIIndex == 0));
            //Debug.Log("uIPlayerHunter.GetComponent<Image>().color.a == 0: " + (uIPlayerHunter.GetComponent<Image>().color.a == 0));
            //if (uIPlayerHunter.GetComponent<Image>().color.a != 0) Debug.Log("uIPlayerHunter.GetComponent<Image>().gameObject.name: " + uIPlayerHunter.GetComponent<Image>().gameObject.name);
            //Debug.Log(" ");

            if (playerSelectionUIIndex == 0 && uIPlayerHunter.GetComponent<Image>().color.a == 0)
            {
                //UIPlayerOneHunter.SetActive(true);
                //UIPlayerOneUnselected.SetActive(false);
                //UIPlayerOneRunner.SetActive(false);
                Image images = uIPlayerHunter.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 1);

                images = uIPlayerUnselected.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 0);
                TextMeshProUGUI textMeshPro = uIPlayerUnselected.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0);

                images = uIPlayerRunner.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 0);
                //Debug.Log("Player Hunter selected by player " + index);
            }
            else if (playerSelectionUIIndex == 1 && uIPlayerUnselected.GetComponent<Image>().color.a == 0)
            {
                //UIPlayerOneHunter.SetActive(false);
                //UIPlayerOneUnselected.SetActive(true);
                //UIPlayerOneRunner.SetActive(false);
                Image images = uIPlayerHunter.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 0);

                images = uIPlayerUnselected.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 1);
                TextMeshProUGUI textMeshPro = uIPlayerUnselected.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 1);

                images = uIPlayerRunner.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 0);

                Debug.Log("Player Unselected: " + uIPlayerUnselected.name + " selected by player " + index);
            }
            else if (playerSelectionUIIndex == 2 && uIPlayerRunner.GetComponent<Image>().color.a == 0)
            {
                //UIPlayerOneHunter.SetActive(false);
                //UIPlayerOneUnselected.SetActive(false);
                //UIPlayerOneRunner.SetActive(true);
                Image images = uIPlayerHunter.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 0);

                images = uIPlayerUnselected.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 0);
                TextMeshProUGUI textMeshPro = uIPlayerUnselected.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0);

                images = uIPlayerRunner.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 1);

                //Debug.Log("Player Runner selected by player " + index);
            }
        }

        private void GetCurrentPlayerGO(out GameObject uIPlayerHunter, out GameObject uIPlayerUnselected, out GameObject uIPlayerRunner)
        {
            uIPlayerHunter = null;
            uIPlayerUnselected = null;
            uIPlayerRunner = null;
            //Debug.Log("index: " + index);

            if (index == 0)
            {
                uIPlayerHunter = UIPlayerOneHunter;
                uIPlayerUnselected = UIPlayerOneUnselected;
                uIPlayerRunner = UIPlayerOneRunner;
            }
            else if (index == 1)
            {
                uIPlayerHunter = UIPlayerTwoHunter;
                uIPlayerUnselected = UIPlayerTwoUnselected;
                uIPlayerRunner = UIPlayerTwoRunner;
            }
            else if (index == 2)
            {
                uIPlayerHunter = UIPlayerThreeHunter;
                uIPlayerUnselected = UIPlayerThreeUnselected;
                uIPlayerRunner = UIPlayerThreeRunner;
            }
            else if (index == 3)
            {
                uIPlayerHunter = UIPlayerFourHunter;
                uIPlayerUnselected = UIPlayerFourUnselected;
                uIPlayerRunner = UIPlayerFourRunner;
            }
        }

        //private void GetCurrentPlayerIndex(out int playerSelectionUIIndex)
        //{
        //    playerSelectionUIIndex = 99;
        //    //Debug.Log("index: " + index);

        //    if (index == 0)
        //    {
        //        playerSelectionUIIndex = m_playerOneSelectedUIIndex;
        //    }
        //    else if (index == 1)
        //    {
        //        playerSelectionUIIndex = m_playerTwoSelectedUIIndex;
        //    }
        //    else if (index == 2)
        //    {
        //        playerSelectionUIIndex = m_playerThreeSelectedUIIndex;
        //    }
        //    else if (index == 3)
        //    {
        //        playerSelectionUIIndex = m_playerFourSelectedUIIndex;
        //    }
        //}

        [Command(requiresAuthority = false)]
        private void ChangePlayerUIIndex(int oldIndex, int newIndex)
        {
            if (index == 0)
            {
                CmdChangePlayerOneUIIndex(oldIndex, newIndex);
            }
            else if (index == 1)
            {
                CmdChangePlayerTwoUIIndex(oldIndex, newIndex);
            }
            else if (index == 2)
            {
                CmdChangePlayerThreeUIIndex(oldIndex, newIndex);
            }
            else if (index == 3)
            {
                CmdChangePlayerFourUIIndex(oldIndex, newIndex);
            }
        }

        [Command(requiresAuthority = false)]
        public void CmdChangePlayerOneUIIndex(int oldIndex, int newIndex)
        {
            m_playerOneSelectedUIIndex = newIndex;
        }

        [Command(requiresAuthority = false)]
        public void CmdChangePlayerTwoUIIndex(int oldIndex, int newIndex)
        {
            m_playerTwoSelectedUIIndex = newIndex;
        }

        [Command(requiresAuthority = false)]
        public void CmdChangePlayerThreeUIIndex(int oldIndex, int newIndex)
        {
            m_playerThreeSelectedUIIndex = newIndex;
        }

        [Command(requiresAuthority = false)]
        public void CmdChangePlayerFourUIIndex(int oldIndex, int newIndex)
        {
            m_playerFourSelectedUIIndex = newIndex;
        }

        public override void OnGUI()
        {
            base.OnGUI();
        }
    }
}