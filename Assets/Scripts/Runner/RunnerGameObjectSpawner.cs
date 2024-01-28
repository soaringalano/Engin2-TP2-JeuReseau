using Cinemachine;
using UnityEngine;

public class RunnerGameObjectSpawner : GameObjectSpawner
{
    [field: SerializeField]
    private GameObject RunnerCameraAssetsPrefab { get; set; }

    private OfflineRunnerControls m_networkedRunnerMovement;
    private GameObject m_runnerGameObject;
    private GameObject m_runnerCamAssetsGameObject;
    private CinemachineVirtualCamera m_virtualCamera;

    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

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
        m_networkedRunnerMovement = GetComponent<OfflineRunnerControls>();
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