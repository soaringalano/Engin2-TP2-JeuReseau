using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        bool showStartButton;

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

        public void OnStartButtonPressed()
        {
            if (allPlayersReady)
            {
                // set to false to hide it in the game scene
                showStartButton = false;
                ToggleStartButton(false);

                ServerChangeScene(GameplayScene);
            }
        }
    }
}
