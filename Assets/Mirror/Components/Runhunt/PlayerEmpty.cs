
using UnityEngine;
using static Mirror.NetworkRoomPlayer;


namespace Mirror
{
    public class PlayerEmpty : NetworkBehaviour
    {
        [field: SerializeField] private GameObject Runner { get; set; }
        [field: SerializeField] private GameObject Hunter { get; set; }
        private GameObject PlayerGO { get; set; } = null;
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
                Debug.Log("Player is a Runner");
                PlayerGO = Instantiate(Runner);
            }
            else if (m_playerSelectedTeam == EPlayerSelectedTeam.Hunters)
            {
                Debug.Log("Player is a Hunter");
                PlayerGO = Instantiate(Hunter);
            }
            else
            {
                Debug.LogError("Player has no team selected");
            }
        }
    }
}