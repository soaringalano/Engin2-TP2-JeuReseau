
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class LobbyPlayer : NetworkRoomPlayer
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
        [SyncVar(hook = nameof(CmdChangePlayerOneUIIndex))] private int m_playerOneSelectedUIIndex = 1;
        [SerializeField]
        [SyncVar(hook = nameof(CmdChangePlayerTwoUIIndex))] private int m_playerTwoSelectedUIIndex = 1;
        [SerializeField]
        [SyncVar(hook = nameof(CmdChangePlayerThreeUIIndex))] private int m_playerThreeSelectedUIIndex = 1;
        [SerializeField]
        [SyncVar(hook = nameof(CmdChangePlayerFourUIIndex))] private int m_playerFourSelectedUIIndex = 1;

        [SerializeField]
        [SyncVar(hook = nameof(CmdSetUnselectedImageAlpha))] private float m_unselectedImageAlpha = 0f;
        [SerializeField]
        [SyncVar(hook = nameof(CmdSetUnselectedTextAlpha))] private float m_unselectedTextAlpha = 0f;
        [SerializeField]
        [SyncVar(hook = nameof(CmdSetHunterImageAlpha))] private float m_hunterImageAlpha = 0f;
        [SerializeField]
        [SyncVar(hook = nameof(CmdSetRunnerImageAlpha))] private float m_runnerImageAlpha = 0f;

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
                    Debug.LogError("OnReadyTicked() P2");
                    TogglePlayerReadyState();
                    break;
                case 2:
                    Debug.LogError("OnReadyTicked() P3");
                    TogglePlayerReadyState();
                    break;
                case 3:
                    Debug.LogError("OnReadyTicked() P4");
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
    }
}