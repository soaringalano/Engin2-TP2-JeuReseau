using Cinemachine;
using UnityEngine;
using Mirror;

public class RunnerGameObjectSpawner : NetworkBehaviour
{
    [field: SerializeField]
    private GameObject RunnerCameraAssetsPrefab { get; set; }
    [field: SerializeField]
    //private GameObject RunnerPrefab { get; set; }
    NetworkedRunnerMovement m_networkedRunnerMovement;
    private GameObject m_runnerGameObject;
    private GameObject m_runnerCamAssetsGameObject;
    private CinemachineVirtualCamera m_virtualCamera;

    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        InstanciateCamAssets();
        GetRunnerGameObject();
        GetNetworkedRunnerMovement();
        SetCameraInNetworkedRunnerMovement();
        SetTheCameraFollow();
        SetTheCameraLookAt();
    }
    private void InstanciateCamAssets()
    {
        //m_runnerGameObject = Instantiate(RunnerPrefab, transform);

        //if (!isLocalPlayer)
        //{
        //    return;
        //}

        m_runnerCamAssetsGameObject = Instantiate(RunnerCameraAssetsPrefab, transform);
    }

    private void GetRunnerGameObject()
    {
        m_runnerGameObject = transform.GetComponentInChildren<Rigidbody>().gameObject;
        if (m_runnerGameObject == null)
        {
            Debug.LogError("Runner GameObject Not found!");
            return;
        }
    }

    private void GetNetworkedRunnerMovement()
    {
        m_networkedRunnerMovement = GetComponent<NetworkedRunnerMovement>();
        if (m_networkedRunnerMovement == null)
        {
            Debug.LogError("NetworkedRunnerMovement Not found!");
            return;
        }
    }

    private void SetCameraInNetworkedRunnerMovement()
    {
        //Camera cam = m_runnerCamAssetsGameObject.GetComponentInChildren<Camera>();
        //if (cam == null)
        //{
        //    Debug.LogError("Camera Not found!");
        //    return;
        //}

        //m_networkedRunnerMovement.Camera = cam;
        m_networkedRunnerMovement.Camera = Camera.main;
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
        Transform runnerTransform = m_runnerGameObject.transform;
        m_virtualCamera.m_Follow = runnerTransform;
    }

    private void SetTheCameraLookAt()
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
