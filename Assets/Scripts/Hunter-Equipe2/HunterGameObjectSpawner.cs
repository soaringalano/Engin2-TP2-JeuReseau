using Cinemachine;
using UnityEngine;

public class HunterGameObjectSpawner : GameObjectSpawner
{
    [field: SerializeField]
    private GameObject HunterCameraAssetsPrefab { get; set; }

    private NetworkedHunterControls m_networkedHunterMovement;
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
        SetCameraInNetworkedPlayerControls();
        SetTheCameraFollow();
        SetTheCameraLookAt();
    }

    protected override void GetPlayerGameObject()
    {
        m_hunterTransform = GetComponentInChildren<Rigidbody>().transform;
        if (m_hunterTransform == null)
        {
            Debug.LogError("Hunter GameObject Not found!");
            return;
        }
    }

    protected override void InstanciateAssets()
    {
        Debug.Log("Instanciate Hunter Assets.");
        m_hunterCamAssetsGameObject = Instantiate(HunterCameraAssetsPrefab, m_hunterTransform);
    }

    protected override void GetNetworkedPlayerControls()
    {
        Debug.Log("Get NetworkedHunterControls.");
        m_networkedHunterMovement = GetComponent<NetworkedHunterControls>();
        if (m_networkedHunterMovement == null)
        {
            Debug.LogError("NetworkedRunnerMovement Not found!");
        }
    }

    protected override void SetCameraInNetworkedPlayerControls()
    {
        Debug.Log("Set Camera in NetworkedPlayerControls.");
        m_networkedHunterMovement.Camera = Camera.main;
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
        Transform lookAt = m_hunterTransform.GetChild(0);
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