// Source : https://mirror-networking.gitbook.io/docs/manual/guides/gameobjects/custom-character-spawning

using UnityEngine;
using static Mirror.NetworkManager;

namespace Mirror
{
    public class RunhuntPlayer : NetworkBehaviour
    {
        [SyncVar]
        public string m_playerName;

        [SyncVar]
        public string m_role;

        private void Start()
        {
            Debug.Log("Enters RunhuntPlayer Start()");
            if (!isLocalPlayer) return;
            Debug.Log("RunhuntPlayer Start() isLocalPlayer");
            transform.GetChild(0).gameObject.SetActive(true);
            singleton.RunHuntPlayer = this;
        }

        public void SetRole(Role newRole)
        {
            Debug.Log("Role set.");
            m_role = GetRoleToString(newRole);
        }
  
        public void GetSpawnablePrefab()
        {
            CreateRunhuntCharacterMessage characterMessage = new CreateRunhuntCharacterMessage
            {
                role = GetStringToRole(),
                name = m_playerName,
            };

            NetworkClient.Send(characterMessage);

            //if (m_role == "Count")
            //{
            //    Debug.LogError("Wrong type of player role!");
            //    return;
            //}

            //Debug.Log("Get role prefab.");

            //foreach (GameObject _gameObject in singleton.spawnPrefabs)
            //{
            //    if (_gameObject == null) return;

            //    if (_gameObject.GetComponent<Runner>() != null)
            //    {
            //        Debug.Log("Runner prefab found.");
            //        InstanciateCharacter(_gameObject);
            //    }
            //    else if (_gameObject.GetComponent<Hunter>() != null)
            //    {
            //        Debug.Log("Hunter prefab found.");
            //        InstanciateCharacter(_gameObject);
            //    }
            //    else
            //    {
            //        continue;
            //    }
            //}
        }

        // Source : https://mirror-networking.gitbook.io/docs/manual/guides/gameobjects/custom-character-spawning
        // Source : https://mirror-networking.gitbook.io/docs/manual/guides/authority
        //[Command]
        private void InstanciateCharacter(GameObject _gameObject)
        {
            Debug.Log("Instanciate player role spawnable prefab.");
            GameObject playerGO = Instantiate(_gameObject, transform);
            Debug.Log("Player role spawnable prefab instanciated.");
            //NetworkIdentity networkIdentity = GetComponent<NetworkIdentity>();
            //networkIdentity.AssignClientAuthority(connectionToClient);
            NetworkServer.Spawn(playerGO, connectionToClient);
            //NetworkServer.AddPlayerForConnection(connectionToClient, playerGO);
        }

        private Role GetStringToRole()
        {
            switch (m_role)
            {
                case "Runner":
                    return Role.Count;
                case "Hunter":
                    return Role.Count;
                case "Count":
                    return Role.Count;
            }
            return Role.Count;
        }

        private string GetRoleToString(Role newRole)
        {
            switch (newRole)
            {
                case Role.Runner:
                    return "Runner";
                case Role.Hunter:
                    return "Hunter";
                case Role.Count:
                    return "Count";

            }

            return "Count";
        }
    }
}