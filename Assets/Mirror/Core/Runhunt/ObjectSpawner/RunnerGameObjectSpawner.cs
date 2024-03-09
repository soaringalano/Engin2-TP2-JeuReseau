using Cinemachine;
using UnityEngine;
using Mirror;

namespace Mirror
{
    public class RunnerGameObjectSpawner : GameObjectSpawner
    {
        [field: SerializeField]
        private GameObject RunnerCameraAssetsPrefab { get; set; }

        [field: SerializeField] public bool IsInitialable { get; set; } = false;

        private RunnerFSM m_networkedRunnerMovement;
        private GameObject m_runnerGameObject;
        private GameObject m_runnerCamAssetsGameObject;
        private CinemachineVirtualCamera m_virtualCamera;

        private void Start()
        {
            //Debug.Log("HunterGameObjectSpawner Start() called!");
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
        }

        protected override void InstanciateAssets()
        {
            m_runnerCamAssetsGameObject = Instantiate(RunnerCameraAssetsPrefab, transform);
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
            m_networkedRunnerMovement = GetComponent<RunnerFSM>();
            if (m_networkedRunnerMovement == null)
            {
                Debug.LogError("NetworkedRunnerMovement Not found!");
                return;
            }
        }

        protected override void SetCameraInNetworkedPlayerControls()
        {
            m_networkedRunnerMovement.Camera = Camera.main;
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
    }
}