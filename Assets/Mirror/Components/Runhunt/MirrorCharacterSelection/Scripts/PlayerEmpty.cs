using UnityEngine;
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
        
        public EPlayerSelectedTeam m_playerSelectedTeam = EPlayerSelectedTeam.Count;

        private void Start()
        {
            //int playerIndex = GetComponentInParent<NetworkRoomPlayer>().index;
            //Debug.Log("Player Index: " + playerIndex);

            if (m_playerSelectedTeam == EPlayerSelectedTeam.Runners)
            {
                Debug.LogError("Player is a Runner");
                if (Runner == null) Debug.LogError("Runner is null");
                GameObject playerCharacter = Instantiate(Runner);
                if (playerCharacter == null) Debug.LogError("PlayerCharacter is null");
                //NetworkServer.Spawn(playerCharacter);
            }
            else if (m_playerSelectedTeam == EPlayerSelectedTeam.Hunters)
            {
                Debug.LogError("Player is a Hunter");
                if(Hunter == null) Debug.LogError("Hunter is null");
                GameObject playerCharacter = Instantiate(Hunter);
                if (playerCharacter == null) Debug.LogError("PlayerCharacter is null");
                //NetworkServer.Spawn(playerCharacter);
            }
            else
            {
                Debug.LogError("Player has no team selected");
            }
        }
    }
}