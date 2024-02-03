// Source : https://mirror-networking.gitbook.io/docs/manual/guides/gameobjects/custom-character-spawning

using UnityEngine;

namespace Mirror
{
    internal class RoleSelection : NetworkBehaviour
    {
        public void OnSelectRunner()
        {
            Debug.Log("RoleSelection OnSelectRunner()");
            if (!isLocalPlayer)
            {
                return;
            }
            Debug.Log("RoleSelection OnSelectRunner() isLocalPlayer");

            Debug.Log("Runner slected!");

            if (NetworkManager.singleton.RunHuntPlayer == null) Debug.LogError("RunhuntPLayer not ready.");
            NetworkManager.singleton.RunHuntPlayer.SetRole(Role.Runner);
            NetworkManager.singleton.RunHuntPlayer.GetSpawnablePrefab();
            Debug.Log("Deactivate role selection menu!");
            transform.GetChild(0).gameObject.SetActive(false);
        }

        public void OnSelectHuntner()
        {
            Debug.Log("RoleSelection OnSelectHuntner()");
            if (!isLocalPlayer)
            {
                return;
            }
            Debug.Log("RoleSelection OnSelectHuntner() isLocalPlayer");

            Debug.Log("Hunter slected!");
            NetworkManager.singleton.RunHuntPlayer.SetRole(Role.Runner);
            NetworkManager.singleton.RunHuntPlayer.GetSpawnablePrefab();
            Debug.Log("Deactivate role selection menu!");
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}