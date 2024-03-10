using UnityEngine;
using UnityEngine.EventSystems;
using static Mirror.RoomManager;

namespace Mirror
{
    public class PlayerEmpty : NetworkBehaviour
    {
        [SyncVar] public EPlayerSelectedTeam m_currentPlayerSelectedTeam = EPlayerSelectedTeam.Count;
        [field: SerializeField] private GameObject Runner { get; set; }
        [field: SerializeField] private GameObject Hunter { get; set; }

        [SerializeField] public int m_playerSelectedTeam = 2;

        private static int m_emptyPlayerIdIterator = 0;
        [SerializeField] public int m_emptyPlayerId = 0;

        //private bool m_isInitialized = false;
        private bool m_isInstanciated = false;
        private bool m_isAutorithyGiven = false;

        //private bool m_isRoomPlayerNull = false;

        private void Start()
        {
            m_emptyPlayerIdIterator++;
            m_emptyPlayerId = m_emptyPlayerIdIterator;
        }

        public void Initialize()
        {
            Debug.LogError("PlayerEmpty Initialize() m_playerSelectedTeam: " + m_playerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            //m_isInitialized = true;
        }

        private void InstanciateCharacter()
        {
            if (isClient)
            {
                // If running on the client, send a command request to the server
                Debug.Log("PlayerEmpty Spawn player command - selectedTeam: " + m_currentPlayerSelectedTeam);
                CmdRequestSpawnObject();
            }
            else
            {
                // If running on the server, spawn the object directly
                Debug.LogError("PlayerEmpty Spawn player NOT cmd - selectedTeam: " + m_currentPlayerSelectedTeam);
                SpawnMyObject();
            }

            //SpawnMyObject(playerSelectedTeam);
            m_isInstanciated = true;
        }

        private void Update()
        {
            //if (isServer) Debug.Log("PlayerEmpty Update() isServer: " + isServer);
            //if (isClientOnly) Debug.Log("PlayerEmpty Update() isClientOnly: " + isClientOnly);

            if (m_isInstanciated) return;
            if (!isLocalPlayer) return;
 
            Debug.Log("PlayerEmpty Update() m_currentPlayerSelectedTeam: " + m_currentPlayerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);

            InstanciatePlayerSelectedRole();
            InitializeSelectedRolePrefab();
        }

        private void InstanciatePlayerSelectedRole()
        {
            Debug.Log("PlayerEmpty InstanciatePlayerSelectedRole() m_playerSelectedTeam: " + m_playerSelectedTeam + " m_currentPlayerSelectedTeam: " + m_currentPlayerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);

            if (m_currentPlayerSelectedTeam == EPlayerSelectedTeam.Count) Debug.LogError("PlayerEmpty Update() m_currentPlayerSelectedTeam: " + m_currentPlayerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);
            InstanciateCharacter();
            CmdDeactivateRoomPlayer();
        }

        private void InitializeSelectedRolePrefab()
        {
            if (!m_isAutorithyGiven) return;
            Debug.Log("PlayerEmpty InitializeSelectedRolePrefab() m_currentPlayerSelectedTeam: " + m_currentPlayerSelectedTeam + " m_emptyPlayerId: " + m_emptyPlayerId);

            if (m_currentPlayerSelectedTeam == EPlayerSelectedTeam.Hunters)
            {
                Debug.Log("PlayerEmpty OnStartAuthority() Set Hunter IsInitialable to true");
                Hunter.GetComponent<HunterGameObjectSpawner>().IsInitialable = true;
            }
            else if (m_currentPlayerSelectedTeam == EPlayerSelectedTeam.Runners)
            {
                Debug.Log("PlayerEmpty OnStartAuthority() Set Runner IsInitialable to true");
                Runner.GetComponent<RunnerGameObjectSpawner>().IsInitialable = true;
            }
            else if (m_currentPlayerSelectedTeam == EPlayerSelectedTeam.Count)
            {
                Debug.LogError("PlayerEmpty OnStartAuthority() selectedTeam is Count");
                // Log the names of each children of this game object:
                foreach (Transform child in transform)
                {
                    Debug.Log("PlayerEmpty OnStartAuthority() child.name: " + child.name);
                }
            }

            m_isInstanciated = true;
        }

        [Command]
        private void CmdDeactivateRoomPlayer()
        {
            RpcDeactivateRoomPlayers();
        }

        [ClientRpc]
        private void RpcDeactivateRoomPlayers()
        {
            //Debug.Log("Get RoomPlayer");
            GameObject roomPlayer = RoomManager.singleton.GetRoomPlayer(connectionToClient);
            //Debug.LogError("Deactivate RoomPlayer");
            roomPlayer.SetActive(false);

            //foreach (NetworkRoomPlayer roomPlayer in RoomManager.singleton.roomSlots)
            //{
            //    if (roomPlayer != null)
            //    {
            //        // Assuming each RoomPlayer has a child GameObject representing the UI
            //        roomPlayer.gameObject.SetActive(false);
            //    }
            //}
        }

        [Command]
        private void CmdRequestSpawnObject()
        {
            SpawnMyObject();
        }

        public void SpawnMyObject()
        {
            //Debug.Log("SpawnMyObject() selectedTeam: " + m_currentPlayerSelectedTeam);
            //GameObject prefab = null;
            if (m_currentPlayerSelectedTeam == EPlayerSelectedTeam.Hunters)
            {
                Hunter = SpawnRoleGO(Hunter);
                //Hunter.GetComponent<HunterGameObjectSpawner>().Initialize();
            }
            else if (m_currentPlayerSelectedTeam == EPlayerSelectedTeam.Runners)
            {
                Runner = SpawnRoleGO(Runner);
                //Runner.GetComponent<RunnerGameObjectSpawner>().Initialize();
            }
        }

        private GameObject SpawnRoleGO(GameObject rolePrefab)
        {
            //Debug.Log("SpawnRoleGO() Instantiate prefab: " + rolePrefab.name + " connectionToClient: " + connectionToClient);
            GameObject roleGO = Instantiate(rolePrefab, transform);
            SpawnWithAuthority(roleGO);
            //Debug.Log("PlayerEmpty SpawnRoleGO() roleGO: " + roleGO);
            //CmdSetParent(roleGO.GetComponent<NetworkIdentity>(), transform.GetComponent<NetworkIdentity>());

            return roleGO;
        }

        private void SpawnWithAuthority(GameObject roleGO)
        {
            if (isServer) // Ensure only the local player can request to spawn an object
            {
                Debug.Log("PlayerEmpty SpawnWithAuthority() connectionToClient: " + connectionToClient);
                NetworkServer.Spawn(roleGO, connectionToClient);
                AssignAutority(roleGO);
            }
            else if (isClientOnly)
            {
                Debug.Log("PlayerEmpty SpawnWithAuthority() connectionToClient: " + connectionToClient);
                RcpSpawnWithClientAuthority(roleGO);
                AssignAutority(roleGO);
            }
        }

        private void AssignAutority(GameObject roleGO)
        {
            CmdSetParent(roleGO.GetComponent<NetworkIdentity>(), transform.GetComponent<NetworkIdentity>());
            if (roleGO.GetComponent<NetworkIdentity>().connectionToClient == connectionToClient)
            {
                Debug.Log("Authority correctly assigned on server. connectionToClient: " + connectionToClient);
            }
            else
            {
                Debug.LogError("Authority assignment failed on server. connectionToClient: " + connectionToClient);
            }
        }

        [Command]
        private void CmdSpawnWithClientAuthority(GameObject roleGO)
        {
            NetworkServer.Spawn(roleGO, connectionToClient);
        }

        [ClientRpc]
        private void RcpSpawnWithClientAuthority(GameObject roleGO)
        {
            NetworkServer.Spawn(roleGO, connectionToClient);
        }

        [Command(requiresAuthority = false)]
        private void CmdSetParent(NetworkIdentity child, NetworkIdentity parent)
        {
            //Debug.Log("CmdSetParent() child: " + child + " parent: " + parent);
            child.transform.SetParent(parent.transform, false);
            RpcSetParent(child.netId, parent.netId);
        }

        [ClientRpc]
        private void RcpSetParent(NetworkIdentity child, NetworkIdentity parent)
        {
            child.transform.SetParent(parent.transform, false);
            RpcSetParent(child.netId, parent.netId);
        }

        [ClientRpc]
        private void RpcSetParent(uint childNetId, uint parentNetId)
        {
            if (NetworkServer.active) return;

            // Find objects on the client
            if (NetworkClient.spawned.TryGetValue(childNetId, out NetworkIdentity childIdentity) && NetworkClient.spawned.TryGetValue(parentNetId, out NetworkIdentity parentIdentity))
            {
                childIdentity.transform.SetParent(parentIdentity.transform, false);
            }
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            m_isAutorithyGiven = true;
        }
    }
}