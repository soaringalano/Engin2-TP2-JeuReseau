using Cinemachine;
using UnityEngine;
using Mirror;

namespace Mirror
{
    public class RunnerGameObjectSpawner : GameObjectSpawner
    {
        [field: SerializeField]
        private GameObject RunnerCameraAssetsPrefab { get; set; }
        [field: SerializeField] private GameObject RunnerUIPrefab { get; set; }
        [field: SerializeField] public bool IsInitialable { get; set; } = false;
        [field: SerializeField] private GameObject EventSystemPrefab { get; set; }

        private RunnerFSM m_runnerFSM = null;
        private GameObject m_runnerGameObject = null;
        private GameObject m_runnerCamAssetsGameObject = null;
        private GameObject m_runnerUIPrefab = null;
        private CinemachineVirtualCamera m_virtualCamera = null;

        private void Start()
        {
            //Debug.Log("RunnerGameObjectSpawner Start() called!");
        }

        private void Update()
        {
            //Debug.Log("RunnerGameObjectSpawner Update() connectionToClient: " + connectionToClient);
            //if (!isLocalPlayer) return;

            if (!GetComponent<NetworkIdentity>().isOwned) return;

            if (!IsInitialable) return;

            Initialize();
            IsInitialable = false;
        }

        public void Initialize()
        {
            if (this.netIdentity == null)
            {
                Debug.LogError("NetworkIdentity is null in HunterGameObjectSpawner.");
                return;
            }

            Debug.Log("RunnerGameObjectSpawner Initialize() called!");

            //if (!isLocalPlayer)
            //{
            //    Debug.LogWarning("Initialize called but is not local player.");
            //    //return;
            //}

            Debug.Log("RunnerGameObjectSpawner Start() called!");

            InstanciateAssets();
            GetPlayerGameObject();
            GetNetworkedPlayerControls();
            SetCameraInNetworkedPlayerControls();
            SetTheCameraFollow();
            SetTheCameraLookAt();
            InitializeSpawnedAssets();
        }

        protected override void InstanciateAssets()
        {
            Debug.Log("Instacieat Runner Camera.");
            m_runnerCamAssetsGameObject = Instantiate(RunnerCameraAssetsPrefab, transform);

            Debug.Log("Instanciate Runner UI.");
            m_runnerUIPrefab = Instantiate(RunnerUIPrefab, transform);
            if (m_runnerUIPrefab == null) Debug.LogError("Runner UI Prefab Not found!");
            else Debug.Log("Runner UI Prefab found!");


            Debug.Log("Instanciate EventSystem.");
            Instantiate(EventSystemPrefab, transform);
        }

        protected override void GetPlayerGameObject()
        {
            m_runnerGameObject = transform.GetComponentInChildren<Rigidbody>().gameObject;
            if (m_runnerGameObject == null)
            {
                Debug.LogError("Runner GameObject Not found!");
                return;
            }
        }

        protected override void GetNetworkedPlayerControls()
        {
            m_runnerFSM = GetComponent<RunnerFSM>();
            if (m_runnerFSM == null)
            {
                Debug.LogError("NetworkedRunnerMovement Not found!");
                return;
            }

            m_runnerFSM.RunnerUI = m_runnerUIPrefab;
            if (m_runnerFSM.RunnerUI == null) Debug.LogError("Runner UI Not found!");
            else Debug.Log("Runner UI found in RunnerFSM!");
        }

        protected override void SetCameraInNetworkedPlayerControls()
        {
            m_runnerFSM.Camera = Camera.main;
        }

        protected override void SetTheCameraFollow()
        {
            CinemachineVirtualCamera virtualCam = m_runnerCamAssetsGameObject.GetComponentInChildren<CinemachineVirtualCamera>();
            if (virtualCam == null)
            {
                Debug.LogError("CinemachineVirtualCamera Not found!");
                return;
            }

            m_virtualCamera = virtualCam;
            Transform runnerTransform = m_runnerGameObject.transform;
            m_virtualCamera.m_Follow = runnerTransform;
        }

        protected override void SetTheCameraLookAt()
        {
            Transform lookAt = m_runnerGameObject.transform.GetChild(0);
            if (lookAt == null)
            {
                Debug.LogError("LookAt Not found!");
                return;
            }

            if (lookAt.name != "LookAt")
            {
                Debug.LogError("Make sure that the GameObject LookAt is the first child of in this prefab hierarchy!");
            }

            m_virtualCamera.m_LookAt = lookAt;
        }

        protected override void InitializeSpawnedAssets()
        {
            Debug.Log("RunnerGameObjectSpawner InitializeSpawnedAssets() called!");
            m_runnerFSM.Initialize();
        }
    }
}