using UnityEngine;
using static Mirror.LobbyUI;
using static Mirror.NetworkRoomPlayer;

namespace Mirror
{
    public class PlayerEmpty : NetworkBehaviour
    {
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
        
        public int m_playerSelectedTeam = 2;

        private static int m_emptyPlayerId = 0;

        private bool m_isInstantiated = false;

        private void Start()
        {
            m_emptyPlayerId++;
            ////int playerIndex = GetComponentInParent<NetworkRoomPlayer>().index;
            ////Debug.Log("Player Index: " + playerIndex);
            //if (!isLocalPlayer) return;
            //Debug.Log("PlayerEmpty Start() m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: "+ m_emptyPlayerId);
            //if (m_playerSelectedTeam == 1)
            //{
            //    Debug.LogError("Player is a Runner m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            //    if (Runner == null) Debug.LogError("Runner is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            //    GameObject playerCharacter = Instantiate(Runner);
            //    if (playerCharacter == null) Debug.LogError("PlayerCharacter is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            //    //NetworkServer.Spawn(playerCharacter);
            //}
            //else if (m_playerSelectedTeam == 0)
            //{
            //    Debug.LogError("Player is a Hunter m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            //    if (Hunter == null) Debug.LogError("Hunter is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            //    GameObject playerCharacter = Instantiate(Hunter);
            //    if (playerCharacter == null) Debug.LogError("PlayerCharacter is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            //    //NetworkServer.Spawn(playerCharacter);
            //}
            //else
            //{
            //    Debug.LogError("Player has no team selected m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            //}
        }

        private void Update()
        {
            if (m_isInstantiated) return;
            if (m_playerSelectedTeam == 2)
            {
                Debug.LogWarning("Player has no team selected m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
                return;
            }
            
            m_isInstantiated = true;
            //int playerIndex = GetComponentInParent<NetworkRoomPlayer>().index;
            //Debug.Log("Player Index: " + playerIndex);
            if (!isLocalPlayer) return;
            Debug.Log("PlayerEmpty Update() m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            if (m_playerSelectedTeam == 1)
            {
                Debug.LogError("Player is a Runner m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
                if (Runner == null) Debug.LogError("Runner is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
                GameObject playerCharacter = Instantiate(Runner);
                if (playerCharacter == null) Debug.LogError("PlayerCharacter is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
                //NetworkServer.Spawn(playerCharacter);
            }
            else if (m_playerSelectedTeam == 0)
            {
                Debug.LogError("Player is a Hunter m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
                if (Hunter == null) Debug.LogError("Hunter is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
                GameObject playerCharacter = Instantiate(Hunter);
                if (playerCharacter == null) Debug.LogError("PlayerCharacter is null m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
                //NetworkServer.Spawn(playerCharacter);
            }
            else
            {
                Debug.LogError("Player has no team selected m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            }

        }
    }
}