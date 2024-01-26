using Cinemachine;
using UnityEngine;

public class HunterGameObjectSpawner : GameObjectSpawner
{
    [field: SerializeField]
    private GameObject HunterCameraAssetsPrefab { get; set; }

    private NetworkedHunterControls m_networkedHunterMovement;
    private Transform m_hunterTransform;


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
        Instantiate(HunterCameraAssetsPrefab, m_hunterTransform);
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
}