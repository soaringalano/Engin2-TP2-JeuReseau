using Mirror;
using UnityEngine;

public class InstantiateCamera : NetworkBehaviour
{
    [field : SerializeField]
    public Camera m_testCam { get; private set; }
    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Camera m_cam = gameObject.AddComponent<Camera>();
        var m_cameraPos = transform.Find("CameraPosition");
        m_testCam = Instantiate(m_cam, m_cameraPos.transform.position, m_cameraPos.transform.rotation);
    }
}
