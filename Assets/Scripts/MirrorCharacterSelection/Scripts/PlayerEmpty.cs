using UnityEngine;
using Mirror;

namespace Mirror
{
    public class PlayerEmpty : NetworkBehaviour
    {
        [field : SerializeField] private GameObject Runner { get; set; }
        [field : SerializeField] private GameObject Hunter { get; set; }
        //private SceneReferencer sceneReferencer;

        public override void OnStartAuthority()
        {
            // enable UI located in the scene, after empty player spawns in.
#if UNITY_2021_3_OR_NEWER
            //sceneReferencer = GameObject.FindAnyObjectByType<SceneReferencer>();
#else
            // Deprecated in Unity 2023.1
            //sceneReferencer = GameObject.FindObjectOfType<SceneReferencer>();
#endif
            //sceneReferencer.GetComponent<Canvas>().enabled = true;
        }
    }
}