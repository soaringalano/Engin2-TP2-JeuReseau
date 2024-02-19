using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mirror
{
    [AddComponentMenu("")]
    public class LobbyUI : NetworkRoomPlayer
    {
        [field: SerializeField] private GameObject EventSysPrefab { get; set; }
        //[field: SerializeField] private GameObject UIReadyBoxPrefab { get; set; }
        [field: SerializeField] private GameObject UIReadyBoxGO { get; set; }

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

        [field: SerializeField] private GameObject P1Ready { get; set; }
        [field: SerializeField] private GameObject P2Ready { get; set; }
        [field: SerializeField] private GameObject P3Ready { get; set; }
        [field: SerializeField] private GameObject P4Ready { get; set; }



        [SerializeField]
        [SyncVar(hook = nameof(CmdChangePlayerOneUIIndex))] public int m_playerOneSelectedUIIndex = 1;
        [SerializeField]
        [SyncVar(hook = nameof(CmdChangePlayerTwoUIIndex))] public int m_playerTwoSelectedUIIndex = 1;
        [SerializeField]
        [SyncVar(hook = nameof(CmdChangePlayerThreeUIIndex))] public int m_playerThreeSelectedUIIndex = 1;
        [SerializeField]
        [SyncVar(hook = nameof(CmdChangePlayerFourUIIndex))] public int m_playerFourSelectedUIIndex = 1;

        [SerializeField]
        [SyncVar(hook = nameof(CmdSetUnselectedImageAlpha))] private float m_unselectedImageAlpha = 0f;
        [SerializeField]
        [SyncVar(hook = nameof(CmdSetUnselectedTextAlpha))] private float m_unselectedTextAlpha = 0f;
        [SerializeField]
        [SyncVar(hook = nameof(CmdSetHunterImageAlpha))] private float m_hunterImageAlpha = 0f;
        [SerializeField]
        [SyncVar(hook = nameof(CmdSetRunnerImageAlpha))] private float m_runnerImageAlpha = 0f;

        public enum EPlayerSelectedTeam
        {
            Hunters,
            Runners,
            Count
        }
        
        [SerializeField] public static int m_playerOneSelectedTeam = 2;
        [SerializeField][SyncVar(hook = nameof(CmdSetPlayerTwoSelectedTeam))] public int m_playerTwoSelectedTeam = 2;
        [SerializeField][SyncVar(hook = nameof(CmdSetPlayerThreeSelectedTeam))] public int m_playerThreeSelectedTeam = 2;
        [SerializeField][SyncVar(hook = nameof(CmdSetPlayerFourSelectedTeam))] public int m_playerFourSelectedTeam = 2;

        private int m_previousSelection = 1;
        private bool m_playerArrivedInLobby = false;
        private bool m_previousReadyToBegin = false;
        private bool m_isEventSystemInstaciated = false;

        public override void OnStartClient()
        {
            Debug.Log($"OnStartClient {gameObject}");
        }

        public override void OnClientEnterRoom()
        {
            Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");

            if (!isLocalPlayer) return;
            if (m_isEventSystemInstaciated) return;

            m_isEventSystemInstaciated = true;
            Instantiate(EventSysPrefab, transform);
            if (transform.GetChild(0).name != "Canvas") Debug.LogError("Canvas not found");
            //UIReadyBoxGO = Instantiate(UIReadyBoxPrefab, transform.GetChild(0));
            if (UIReadyBoxGO == null) Debug.LogError("UIReadyBoxGO is null");
            UIReadyBoxGO.SetActive(true);
        }

        public override void OnClientExitRoom()
        {
            Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            Debug.Log($"IndexChanged {newIndex}");

            if (!isLocalPlayer) return;
            m_playerArrivedInLobby = true;
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            Debug.Log($"ReadyStateChanged {newReadyState}");

            if (!isLocalPlayer) return;

            if (newReadyState)
            {
                switch (index)
                {
                    case 0:
                        Debug.Log("ReadyStateChanged() Player 1 picked a team: " + m_playerOneSelectedUIIndex);
                        if (m_playerOneSelectedUIIndex == 0) m_playerOneSelectedTeam = 0;
                        else if (m_playerOneSelectedUIIndex == 2) m_playerOneSelectedTeam = 1;
                        break;
                    case 1:
                        Debug.Log("ReadyStateChanged() Player 2 picked a team: " + m_playerTwoSelectedUIIndex);
                        if (m_playerTwoSelectedUIIndex == 0) CmdSetPlayerTwoSelectedTeam(m_playerTwoSelectedTeam, 0);
                        else if (m_playerTwoSelectedUIIndex == 2) CmdSetPlayerTwoSelectedTeam(m_playerTwoSelectedTeam, 1);
                        break;
                    case 2:
                        Debug.Log("ReadyStateChanged() Player 3 picked a team: " + m_playerThreeSelectedUIIndex);
                        if (m_playerThreeSelectedUIIndex == 0) CmdSetPlayerThreeSelectedTeam(m_playerThreeSelectedTeam, 0);
                        else if (m_playerThreeSelectedUIIndex == 2) CmdSetPlayerThreeSelectedTeam(m_playerThreeSelectedTeam, 1);
                        break;
                    case 3:
                        Debug.Log("ReadyStateChanged() Player 4 picked a team: " + m_playerFourSelectedUIIndex);
                        if (m_playerFourSelectedUIIndex == 0) CmdSetPlayerFourSelectedTeam(m_playerFourSelectedTeam, 0);
                        else if (m_playerFourSelectedUIIndex == 2) CmdSetPlayerFourSelectedTeam(m_playerFourSelectedTeam, 1);
                        break;
                }

            }
        }


        [Command(requiresAuthority = false)]
        private void CmdSetPlayerTwoSelectedTeam(int oldValuem, int newValue)
        {
            m_playerTwoSelectedTeam = newValue;
        }

        [Command(requiresAuthority = false)]
        private void CmdSetPlayerThreeSelectedTeam(int oldValuem, int newValue)
        {
            m_playerThreeSelectedTeam = newValue;
        }

        [Command(requiresAuthority = false)]
        private void CmdSetPlayerFourSelectedTeam(int oldValuem, int newValue)
        {
            m_playerFourSelectedTeam = newValue;
        }

        public override void Start()
        {
            // log player index and child index
            Debug.Log("LobbyPlayer Start player index: " + index + " child index: " + (transform.GetSiblingIndex() - 1));
            base.Start();

            if (!isOwned)
            {
                ToggleReadyBoxVisibility(false);
                ToggleReadyBoxInteractibility(false);
            }
            else if (isOwned)
            {
                ToggleReadyBoxVisibility(true);
                ToggleReadyBoxInteractibility(false);
            }
        }

        private void ToggleReadyBoxVisibility(bool isVisible)
        {
            Debug.Log("OnClientEnterRoom - isLocalPlayer: " + isLocalPlayer + " index: " + index);
            UIReadyBoxGO.GetComponentInChildren<Image>().color = new Color(UIReadyBoxGO.GetComponentInChildren<Image>().color.r, UIReadyBoxGO.GetComponentInChildren<Image>().color.g, UIReadyBoxGO.GetComponentInChildren<Image>().color.b, isVisible ? 1 : 0);
            UIReadyBoxGO.GetComponentInChildren<Text>().color = new Color(UIReadyBoxGO.GetComponentInChildren<Text>().color.r, UIReadyBoxGO.GetComponentInChildren<Text>().color.g, UIReadyBoxGO.GetComponentInChildren<Text>().color.b, isVisible ? 1 : 0);
        }

        private void ToggleReadyBoxInteractibility(bool isVisible)
        {
            UIReadyBoxGO.GetComponent<Toggle>().interactable = isVisible;
            UIReadyBoxGO.GetComponent<Toggle>().isOn = isVisible;
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                switch (index)
                {
                    case 0:
                        //Debug.Log("isLocalPlayer index: " + index + " child index: " + (transform.GetSiblingIndex() - 1));
                        //Debug.Log("Player 1");
                        UpdateActivePlayerUILocal(m_playerOneSelectedUIIndex);
                        TogglePlayerIsReadyUI(P1Ready);
                        break;
                    case 1:
                        //Debug.Log("Player 2");
                        UpdateActivePlayerUILocal(m_playerTwoSelectedUIIndex);
                        TogglePlayerIsReadyUI(P2Ready);
                        break;
                    case 2:
                        //Debug.Log("Player 3");
                        UpdateActivePlayerUILocal(m_playerThreeSelectedUIIndex);
                        TogglePlayerIsReadyUI(P3Ready);
                        break;
                    case 3:
                        //Debug.Log("Player 4");
                        UpdateActivePlayerUILocal(m_playerFourSelectedUIIndex);
                        TogglePlayerIsReadyUI(P4Ready);
                        break;
                }
            }

            //Debug.Log("isLocalPlayer index: " + index + " child index: " + (transform.GetSiblingIndex() - 1));
            if (m_playerArrivedInLobby == false && isClientOnly) return;
            // log player index and child index
            //Debug.Log("LobbyPlayer Update player index: " + index + " child index: " + transform.GetSiblingIndex());
            // Get player's current index
            if (isLocalPlayer)
            {

                //Debug.Log("LobbyPlayer Update player index: " + index + " child index: " + (transform.GetSiblingIndex()-1));
                switch (index)
                {
                    case 0:
                        //Debug.Log("Player 1");
                        PlayerControlUpdateServer(m_playerOneSelectedUIIndex);
                        UpdateActivePlayerUILocal(m_playerOneSelectedUIIndex);
                        UpdatePlayerIsReady(m_playerOneSelectedUIIndex);
                        TogglePlayerIsReadyUI(P1Ready);
                        break;
                    case 1:
                        //Debug.Log("Player 2");
                        PlayerControlUpdateServer(m_playerTwoSelectedUIIndex);
                        UpdateActivePlayerUILocal(m_playerTwoSelectedUIIndex);
                        UpdatePlayerIsReady(m_playerTwoSelectedUIIndex);
                        TogglePlayerIsReadyUI(P2Ready);
                        break;
                    case 2:
                        //Debug.Log("Player 3");
                        PlayerControlUpdateServer(m_playerThreeSelectedUIIndex);
                        UpdateActivePlayerUILocal(m_playerThreeSelectedUIIndex);
                        UpdatePlayerIsReady(m_playerThreeSelectedUIIndex);
                        TogglePlayerIsReadyUI(P3Ready);
                        break;
                    case 3:
                        //Debug.Log("Player 4");
                        PlayerControlUpdateServer(m_playerFourSelectedUIIndex);
                        UpdateActivePlayerUILocal(m_playerFourSelectedUIIndex);
                        UpdatePlayerIsReady(m_playerFourSelectedUIIndex);
                        TogglePlayerIsReadyUI(P4Ready);
                        break;
                }
            }
        }

        private void PlayerControlUpdateServer(int playerSelectionUIIndex)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if ((playerSelectionUIIndex - 1) < 0) return;
                int newUiIndex = playerSelectionUIIndex - 1;
                ChangePlayerUIIndexServer(playerSelectionUIIndex, newUiIndex);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if ((playerSelectionUIIndex + 1) > 2) return;
                int newUiIndex = playerSelectionUIIndex + 1;
                ChangePlayerUIIndexServer(playerSelectionUIIndex, newUiIndex);
            }
        }

        private void UpdateActivePlayerUILocal(int playerSelectionUIIndex)
        {
            GameObject uIPlayerHunter, uIPlayerUnselected, uIPlayerRunner;
            GetCurrentPlayerGO(out uIPlayerHunter, out uIPlayerUnselected, out uIPlayerRunner);
            //Debug.Log("isLocalPlayer index: " + index + " child index: " + (transform.GetSiblingIndex() - 1) + " playerSelectionUIIndex == 1: " + (playerSelectionUIIndex == 1) + " IPlayerUnselected.GetComponent<Image>().color.a == 0: " + (uIPlayerUnselected.GetComponent<Image>().color.a == 0));

            if (playerSelectionUIIndex == 0 && uIPlayerHunter.GetComponent<Image>().color.a == 0)
            {
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
                CmdSetShowStartButton(RoomManager.singleton.showStartButton ,false);
                Image images = uIPlayerHunter.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 0);

                images = uIPlayerUnselected.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 1);
                TextMeshProUGUI textMeshPro = uIPlayerUnselected.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 1);

                images = uIPlayerRunner.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 0);

                //Debug.Log("Player Unselected: " + uIPlayerUnselected.name + " selected by player " + index);
            }
            else if (playerSelectionUIIndex == 2 && uIPlayerRunner.GetComponent<Image>().color.a == 0)
            {
                Image images = uIPlayerHunter.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 0);

                images = uIPlayerUnselected.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 0);
                TextMeshProUGUI textMeshPro = uIPlayerUnselected.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0);

                images = uIPlayerRunner.GetComponent<Image>();
                images.color = new Color(images.color.r, images.color.g, images.color.b, 1);
            }
        }

        private void UpdatePlayerIsReady(int playerSelectionUIIndex)
        {
            GameObject uIPlayerHunter, uIPlayerUnselected, uIPlayerRunner;
            GetCurrentPlayerGO(out uIPlayerHunter, out uIPlayerUnselected, out uIPlayerRunner);
            //Debug.Log("isLocalPlayer index: " + index + " child index: " + (transform.GetSiblingIndex() - 1) + " playerSelectionUIIndex == 1: " + (playerSelectionUIIndex == 1) + " IPlayerUnselected.GetComponent<Image>().color.a == 0: " + (uIPlayerUnselected.GetComponent<Image>().color.a == 0));
            if (m_previousSelection == playerSelectionUIIndex) return;
            m_previousSelection = playerSelectionUIIndex;

            if (playerSelectionUIIndex == 0 && UIReadyBoxGO.GetComponent<Toggle>().interactable == false)
            {
                UIReadyBoxGO.GetComponent<Toggle>().isOn = false;
                UIReadyBoxGO.GetComponent<Toggle>().interactable = true;
                Debug.Log("UpdatePlayerIsReady index: " + index + " child index: " + (transform.GetSiblingIndex() - 1));
            }
            else if (playerSelectionUIIndex == 1 && UIReadyBoxGO.GetComponent<Toggle>().interactable)
            {
                CmdSetShowStartButton(RoomManager.singleton.showStartButton, false);
                UIReadyBoxGO.GetComponent<Toggle>().isOn = false;
                UIReadyBoxGO.GetComponent<Toggle>().interactable = false;
                Debug.Log("UpdatePlayerIsReady index: " + index + " child index: " + (transform.GetSiblingIndex() - 1));
            }
            else if (playerSelectionUIIndex == 2 && UIReadyBoxGO.GetComponent<Toggle>().interactable == false)
            {
                UIReadyBoxGO.GetComponent<Toggle>().isOn = false;
                UIReadyBoxGO.GetComponent<Toggle>().interactable = true;
                Debug.Log("UpdatePlayerIsReady index: " + index + " child index: " + (transform.GetSiblingIndex() - 1));
            }
        }

        private void TogglePlayerIsReadyUI(GameObject pIsReadyUi)
        {
            if (readyToBegin == m_previousReadyToBegin) return;
            m_previousReadyToBegin = readyToBegin;

            Debug.Log("TogglePlayerIsReadyUI - isLocalPlayer: " + isLocalPlayer + " index: " + index);
            if (pIsReadyUi == null) Debug.LogError("pIsReadyUi is null");

            Image image = pIsReadyUi.GetComponent<Image>();
            if (image == null) Debug.LogError("Image is null" + pIsReadyUi.name);
            pIsReadyUi.GetComponent<Image>().color = new Color(image.color.r, image.color.g, image.color.b, readyToBegin ? 1 : 0);

            TextMeshProUGUI textMeshPro = pIsReadyUi.GetComponentInChildren<TextMeshProUGUI>();
            if (textMeshPro == null) Debug.LogError("TextMeshPro is null" + pIsReadyUi.name);
            pIsReadyUi.GetComponentInChildren<TextMeshProUGUI>().color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, readyToBegin ? 1 : 0);
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

        private void ChangePlayerUIIndexServer(int oldIndex, int newIndex)
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
        public void CmdSetUnselectedImageAlpha(float oldIndex, float newIndex)
        {
            m_unselectedImageAlpha = newIndex;
        }

        [Command(requiresAuthority = false)]
        public void CmdSetUnselectedTextAlpha(float oldIndex, float newIndex)
        {
            m_unselectedTextAlpha = newIndex;
        }

        [Command(requiresAuthority = false)]
        public void CmdSetHunterImageAlpha(float oldIndex, float newIndex)
        {
            m_hunterImageAlpha = newIndex;
        }

        [Command(requiresAuthority = false)]
        public void CmdSetRunnerImageAlpha(float oldIndex, float newIndex)
        {
            m_runnerImageAlpha = newIndex;
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

        public void OnReadyTicked()
        {
            if (!isLocalPlayer) return;

            switch (index)
            {
                case 0:
                    Debug.Log("OnReadyTicked() P1");
                    TogglePlayerReadyState();
                    break;
                case 1:
                    Debug.Log("OnReadyTicked() P2");
                    TogglePlayerReadyState();
                    break;
                case 2:
                    Debug.Log("OnReadyTicked() P3");
                    TogglePlayerReadyState();
                    break;
                case 3:
                    Debug.Log("OnReadyTicked() P4");
                    TogglePlayerReadyState();
                    break;
            }
        }

        private void TogglePlayerReadyState()
        {
            if (UIReadyBoxGO.GetComponent<Toggle>().isOn)
            {
                Debug.Log("Player " + index + " is ready");
                CmdChangeReadyState(true);
            }
            else
            {
                Debug.Log("Player " + index + " is not ready");
                CmdChangeReadyState(false);
            }
        }

        public override void OnGUI()
        {
            base.OnGUI();
        }

        [Command(requiresAuthority = false)]
        public void CmdSetShowStartButton(bool oldValue, bool newValue)
        {
            RoomManager.singleton.showStartButton = newValue;
        }


        //[SyncVar(hook = nameof(CmdSetShowStartButton))]
        //public bool showStartButton;

        //[Command(requiresAuthority = false)]
        //public void CmdSetShowStartButton(bool oldValue, bool newValue)
        //{
        //    RoomManager.singleton.showStartButton = newValue;
        //}

        //public override void OnRoomServerPlayersReady()
        //{
        //    // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
        //    if (Utils.IsHeadless())
        //    {
        //        base.OnRoomServerPlayersReady();
        //    }
        //    else
        //    {
        //        CmdSetShowStartButton(showStartButton, true);
        //        ToggleStartButton(true);
        //    }
        //}

        //private void ToggleStartButton(bool isVsible)
        //{
        //    Button startButton = GetComponentInChildren<Button>();
        //    GetComponentInChildren<Button>().interactable = isVsible;

        //    Image image = startButton.GetComponent<Image>();
        //    image.color = new Color(image.color.r, image.color.g, image.color.b, isVsible ? 1 : 0);

        //    TextMeshProUGUI textMeshProUGUI = startButton.GetComponentInChildren<TextMeshProUGUI>();
        //    textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, isVsible ? 1 : 0);
        //}

        //public override void OnGUI()
        //{
        //    base.OnGUI();

        //    if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
        //    {
        //        // set to false to hide it in the game scene
        //        showStartButton = false;

        //        ServerChangeScene(GameplayScene);
        //    }
        //}

        //public void OnStartButtonPressed()
        //{
        //    if (!RoomManager.singleton.showStartButton) return;
        //    if (!RoomManager.singleton.allPlayersReady) return;

        //    if (!IsBothTeamPopulated())
        //    {
        //        Debug.Log("Both teams must be populated");
        //        return;
        //    }
        //    Debug.Log("Both teams are populated");
        //    Debug.Log("Start Game button pressed");
        //    // set to false to hide it in the game scene
        //    RoomManager.singleton.CmdSetShowStartButton(RoomManager.singleton.showStartButton, false);
        //    RoomManager.singleton.ToggleStartButton(false);

        //    //SetTeamOnNetwork();

        //    RoomManager.singleton.ServerChangeScene(RoomManager.singleton.GameplayScene);
        //}

        //private bool IsBothTeamPopulated()
        //{
        //    int nbHunters = 0;
        //    int nbRunners = 0;
        //    int playerIndex = 0;

        //    foreach (NetworkRoomPlayer player in RoomManager.singleton.roomSlots)
        //    {
        //        if (player == null) continue;
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
        //    }

        //    Debug.Log("nbHunters: " + nbHunters + " nbRunners: " + nbRunners + " nbPlayers: " + playerIndex);
        //    return nbHunters > 0 && nbRunners > 0;
        //}

    }
}