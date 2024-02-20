using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
    public class HunterGameObjectSpawner : GameObjectSpawner
    {
        [field: SerializeField] private GameObject HunterCameraAssetsPrefab { get; set; }
        [field: SerializeField] private GameObject HunterUIPrefab { get; set; }

        private HunterFSM m_hunterFSM;
        private HunterPowerUpButton m_hunterAbilities;
        private GameObject m_hunterCamAssetsGameObject;
        private Transform m_hunterTransform;
        private CinemachineVirtualCamera m_virtualCamera;

        public void Initialize()
        {
            //if (!isLocalPlayer)
            //{
            //    return;
            //}

            Debug.LogError("HunterGameObjectSpawner Start() called!");

            GetPlayerGameObject();
            InstanciateAssets();
            GetNetworkedPlayerControls();
            SetAssetGameObject();
            SetCameraInNetworkedPlayerControls();
            SetTheCameraFollow();
            SetTheCameraLookAt();
            InitializeSpawnedAssets();
        }

        protected override void GetPlayerGameObject()
        {
            m_hunterTransform = transform.GetComponentInChildren<Rigidbody>().transform;
            if (m_hunterTransform == null || m_hunterTransform.name != "HunterLookAtFloorBody")
            {
                Debug.LogError("Hunter GameObject Not found! Or is not named HunterLookAtFloorBody!: " + m_hunterTransform.name);
                return;
            }
        }

        protected override void InstanciateAssets()
        {

            //TODO: instanciate later at the hunter camera position
            Debug.Log("transform child count: " + transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                Debug.Log("HunterUIPrefab child name: " + child.name);
                if (child.name != "HunterSelectionPose") continue;

                //Transform hunterDroneControlsTransform = child.GetComponent<HunterDroneControls>().transform;
                child.transform.position = new Vector3(0, -500f, 0);
                Debug.Log("HunterDroneControls position set to: " + child.transform.position);
            }

            Debug.Log("Instanciate Hunter Camera Assets.");
            m_hunterCamAssetsGameObject = Instantiate(HunterCameraAssetsPrefab, transform);
            Instantiate(HunterUIPrefab, transform);
        }

        protected override void GetNetworkedPlayerControls()
        {
            Debug.Log("Get GetNetworkedPlayerControlsAndAbilities.");
            m_hunterFSM = GetComponent<HunterFSM>();
            if (m_hunterFSM == null)
            {
                Debug.LogError("HunterFSM Not found!");
            }
        }

        protected override void SetAssetGameObject()
        {
            // Source : https://discussions.unity.com/t/find-gameobjects-in-specific-scene-only/163901
            Scene scene = gameObject.scene;
            GameObject[] gameObjects = scene.GetRootGameObjects();
            Transform sceneTransform = null;

            foreach (GameObject _gameObject in gameObjects)
            {
                if (_gameObject.name != "Scene") continue;

                sceneTransform = _gameObject.transform;
                break;
            }

            if (sceneTransform == null)
            {
                Debug.LogError("First scene child GameObject not found!");
                return;
            }

            Transform environement = sceneTransform.GetChild(0);
            if (environement.name != "Environment")
            {
                Debug.LogError("Please place Environment GameObject as first child in the scene! First scene child GameObject name: " + sceneTransform.name);
                return;
            }

            Transform runnerPlatform = environement.GetChild(0);
            if (runnerPlatform.name != "RunnerPlatform")
            {
                Debug.LogError("Please place RunnerPlatform GameObject as first child in Environment! Cureent GO is: " + runnerPlatform.name);
                return;
            }

            Transform runnerFloorPlatform = runnerPlatform.GetChild(0);
            if (runnerFloorPlatform.name != "RunnerFloorPlatform")
            {
                Debug.LogError("Please place RunnerFloorPlatform GameObject as first child in RunnerPlatform! Cureent GO is: " + runnerFloorPlatform.name);
                return;
            }

            if (runnerFloorPlatform == null)
            {
                Debug.LogError("Setting the platform failed!");
                return;
            }

            if (m_hunterFSM == null)
            {
                Debug.LogError("NetworkedHunterMovement is not ready to be accesed!");
                return;
            }

            m_hunterFSM.TerrainPlane = runnerFloorPlatform.gameObject;
        }


        protected override void SetCameraInNetworkedPlayerControls()
        {
            Debug.Log("Set Camera in NetworkedPlayerControls.");
            m_hunterFSM.Camera = Camera.main;
            m_hunterFSM.VirtualCamera = m_hunterCamAssetsGameObject.GetComponentInChildren<CinemachineVirtualCamera>();

            if (m_hunterFSM.Camera == null) Debug.LogError("MainCamera Not found!");
            else Debug.Log("Hunter MainCamera found!");
        }

        protected override void SetTheCameraFollow()
        {
            CinemachineVirtualCamera virtualCam = m_hunterCamAssetsGameObject.GetComponentInChildren<CinemachineVirtualCamera>();
            if (virtualCam == null)
            {
                Debug.LogError("CinemachineVirtualCamera Not found!");
                return;
            }

            m_virtualCamera = virtualCam;
            m_virtualCamera.m_Follow = m_hunterFSM.HunterLookAtFloorBody;
        }

        protected override void SetTheCameraLookAt()
        {
            if (m_hunterTransform == null)
            {
                Debug.LogError("HunterTransform not properly set!");
                return;
            }

            Transform lookAt = m_hunterTransform;

            if (lookAt.name != "HunterLookAtFloorBody")
            {
                Debug.LogError("Make sure that the GameObject HunterLookAtFloorBody is the first child of in this prefab hierarchy! The current GameObject is: " + lookAt.name);
            }

            m_virtualCamera.m_LookAt = lookAt;
        }

        protected override void InitializeSpawnedAssets()
        {
            m_hunterFSM.Initialize();
        }
    }
}