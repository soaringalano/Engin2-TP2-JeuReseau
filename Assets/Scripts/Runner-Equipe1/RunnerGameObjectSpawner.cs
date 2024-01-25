using Cinemachine;
using UnityEngine;
using Mirror;

public class RunnerGameObjectSpawner : NetworkBehaviour
{
    [field: SerializeField]
    private GameObject RunnerCameraAssetsPrefab { get; set; }
    [field: SerializeField]
    private GameObject RunnerPrefab { get; set; }

    private GameObject m_runnerGameObject;
    private GameObject m_runnerCamAssetsGameObject;
    private CinemachineVirtualCamera m_virtualCamera;

    void Start()
    {
        InstanciateGameObjects();
        SetCameraInNetworkedRunnerMovement();
        SetTheCameraFollow();
        SetTheCameraLookAt();
    }

    private void SetCameraInNetworkedRunnerMovement()
    {
        Camera cam = m_runnerCamAssetsGameObject.GetComponentInChildren<Camera>();
        if (cam == null)
        {
            Debug.LogError("Camera Not found!");
            return;
        }

        NetworkedRunnerMovement NwRunnerMov = m_runnerGameObject.GetComponent<NetworkedRunnerMovement>();
        if (NwRunnerMov == null)
        {
            Debug.LogError("NetworkedRunnerMovement Not found!");
            return;
        }

        NwRunnerMov.Camera = cam;
    }

    private void SetTheCameraLookAt()
    {
        Transform lookAt = m_runnerCamAssetsGameObject.transform.GetChild(0);
        if (lookAt == null)
        {
            Debug.LogError("LookAt Not found!");
            return;
        }

        m_virtualCamera.m_LookAt = lookAt;
    }

    private void SetTheCameraFollow()
    {
        CinemachineVirtualCamera virtualCam = m_runnerCamAssetsGameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        if (virtualCam == null)
        {
            Debug.LogError("CinemachineVirtualCamera Not found!");
            return;
        }

        m_virtualCamera = virtualCam;
        m_virtualCamera.m_Follow = m_runnerGameObject.transform;
    }

    private void InstanciateGameObjects()
    {
        m_runnerCamAssetsGameObject = Instantiate(RunnerCameraAssetsPrefab, transform);
        m_runnerGameObject = Instantiate(RunnerPrefab, transform);
    }
}
