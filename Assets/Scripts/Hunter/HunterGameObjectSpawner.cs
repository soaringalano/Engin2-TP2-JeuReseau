using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HunterGameObjectSpawner : GameObjectSpawner
{
    [field: SerializeField] private GameObject HunterCameraAssetsPrefab { get; set; }
    [field: SerializeField] private GameObject HunterUIPrefab { get; set; }

    private HunterOnlineControls m_networkedHunterMovement;
    private GameObject m_hunterCamAssetsGameObject;
    private Transform m_hunterTransform;
    private CinemachineVirtualCamera m_virtualCamera;


    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

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
        if (m_hunterTransform == null || m_hunterTransform.name != "LookAt")
        {
            Debug.LogError("Hunter GameObject Not found! Or is not named LookAt!");
            return;
        }
    }

    protected override void InstanciateAssets()
    {
        Debug.Log("Instanciate Hunter Assets.");
        m_hunterCamAssetsGameObject = Instantiate(HunterCameraAssetsPrefab, transform);
        Instantiate(HunterUIPrefab, transform);
    }

    protected override void GetNetworkedPlayerControls()
    {
        Debug.Log("Get NetworkedHunterControls.");
        m_networkedHunterMovement = GetComponent<HunterOnlineControls>();
        if (m_networkedHunterMovement == null)
        {
            Debug.LogError("NetworkedRunnerMovement Not found!");
        }
    }

    protected override void SetAssetGameObject()
    {
        // Source : https://discussions.unity.com/t/find-gameobjects-in-specific-scene-only/163901
        Scene scene = gameObject.scene;
        GameObject[] gameObjects = scene.GetRootGameObjects();
        Transform environmentTransform = null;

        foreach (GameObject _gameObject in gameObjects)
        {
            if (_gameObject.name != "Environment") continue;

            environmentTransform = _gameObject.transform;
            break;
        }

        if (environmentTransform == null)
        {
            Debug.LogError("First scene child GameObject not found!");
            return;
        }

        if (environmentTransform.name != "Environment")
        {
            Debug.LogError("Please place Environment GameObject as first child in the scene! First scene child GameObject name: " + environmentTransform.name);
            return;
        }

        Transform runnerPlatform = environmentTransform.GetChild(0);
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

        if (m_networkedHunterMovement == null)
        {
            Debug.LogError("NetworkedHunterMovement is not ready to be accesed!");
            return;
        }

        m_networkedHunterMovement.m_terrainPlane = runnerFloorPlatform.gameObject;
    }


    protected override void SetCameraInNetworkedPlayerControls()
    {
        Debug.Log("Set Camera in NetworkedPlayerControls.");
        m_networkedHunterMovement.Camera = Camera.main;
        m_networkedHunterMovement.VirtualCamera = m_hunterCamAssetsGameObject.GetComponentInChildren<CinemachineVirtualCamera>();

        if (m_networkedHunterMovement.Camera == null)
        {
            Debug.LogError("MainCamera Not found!");
        }
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
        m_virtualCamera.m_Follow = m_networkedHunterMovement.m_objectToLookAt;
    }

    protected override void SetTheCameraLookAt()
    {
        if (m_hunterTransform == null)
        {
            Debug.LogError("HunterTransform not properly set!");
            return;
        }

        Transform lookAt = m_hunterTransform;

        if (lookAt.name != "LookAt")
        {
            Debug.LogError("Make sure that the GameObject LookAt is the first child of in this prefab hierarchy! The current GameObject is: " + lookAt.name);
        }

        m_virtualCamera.m_LookAt = lookAt;
    }

    protected override void InitializeSpawnedAssets()
    {
        m_networkedHunterMovement.Initialize();
    }
}