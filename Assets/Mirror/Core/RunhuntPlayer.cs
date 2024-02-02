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

        public void SetRole(Role newRole)
        {
            Debug.Log("Role set.");
            m_role = GetRoleToString(newRole);
        }

  
        public void GetSpawnablePrefab()
        {
            if (m_role == "Count")
            {
                Debug.LogError("Wrong type of player role!");
                return;
            }
            Debug.Log("Instanciate character.");

            foreach (GameObject _gameObject in singleton.spawnPrefabs) 
            {
                if (_gameObject == null) return;

                if (_gameObject.GetComponent<Runner>() != null)
                {
                    InstanciateCharacter(_gameObject);
                }
                else if (_gameObject.GetComponent<Hunter>() != null)
                {
                    InstanciateCharacter(_gameObject);
                }
                else
                {
                    continue;
                }
            }
        }

        // Source : https://mirror-networking.gitbook.io/docs/manual/guides/authority
        //[Command]
        private void InstanciateCharacter(GameObject _gameObject)
        {
            GameObject playerGO = Instantiate(_gameObject, transform);
            //NetworkIdentity networkIdentity = GetComponent<NetworkIdentity>();
            //networkIdentity.AssignClientAuthority(connectionToClient);
            NetworkServer.Spawn(playerGO, connectionToClient);
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