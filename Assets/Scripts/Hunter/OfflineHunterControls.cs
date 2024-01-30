using Cinemachine;
using UnityEngine;

public class OfflineHunterControls : MonoBehaviour
{

    public Camera Camera { get; set; }
    public CinemachineVirtualCamera VirtualCamera { get; set; }

    /* ################# Do not use in code, it replaces the active Camera variables in Start() ############### */
    [field: SerializeField] private Camera OfflineCamera { get; set; }
    [field: SerializeField] private CinemachineVirtualCamera OfflineVirtualCamera { get; set; }
    /* ######################################################################################################## */

    private Rigidbody RB { get; set; }
    private Transform m_hunterTransform;
    private CinemachinePOV m_cinemachinePOV;
    private CinemachineFramingTransposer m_framingTransposer;
    private float m_cinemachinePOVMaxSpeedHorizontal;
    private float m_cinemachinePOVMaxSpeedVertical;

    [field: SerializeField] public Transform m_objectToLookAt { get; set; }

    [Header("LookAt controls Settings")]
    [SerializeField] private float m_lookAtMinTorque = 100.0f;
    [SerializeField] private float m_lookAtMaxTorque = 500.0f;
    [SerializeField] private float m_lookAtMinVelocity = 10.0f;
    [SerializeField] private float m_lookAtMaxVelocity = 20.0f;
    private float m_LookAtCurrentMaxVelocity = 0f;
    private bool m_isLookAtSetToStop = false;
    private float m_lookAtDecelerationRate = 0.2f;
    private float m_camDistLookAtSpeedMultiplier = 0.1f;


    [Header("Scrolling Settings")]
    private float m_scrollSpeed = 200.0f;
    private float m_minCamDist = 15.0f;
    private float m_maxCamDist = 60.0f;
    private float m_minCamFOV = 1.0f;
    private float m_maxCamFOV = 90.0f;
    private float m_scrollSmoothDampTime = 0.01f;
    private float m_FOVmoothDampTime = 0.4f;

    [Header("Rotating PLatform Settings")]
    [SerializeField] public GameObject m_terrainPlane;
    [SerializeField] private float m_rotationSpeed = 0f;
    [SerializeField] private float m_maxRotationAngle = 15f;
    private bool m_isRotating = false;
    private Vector3 m_previousMousePosition;
    private float m_currentRotationX = 0f;
    private float m_currentRotationZ = 0f;


    public void Start()
    {
        m_LookAtCurrentMaxVelocity = m_lookAtMinTorque;
        RB = GetComponentInChildren<Rigidbody>();
        if (RB != null) Debug.Log("Hunter RigidBody found!");
        else Debug.LogError("Hunter RigidBody not found!");

        m_hunterTransform = RB.transform;
        if (m_hunterTransform != null) Debug.Log("Hunter Transform found!");
        else Debug.LogError("Hunter Transform not found!");

        // Offline mode has not GameObjectSpawner to fill in the Camera
        if (Camera == null)
        {
            Debug.LogWarning("No networked camera found! Using offline camera.");
            Camera = OfflineCamera;
        }

        // Offline mode has not GameObjectSpawner to fill in the VirtualCamera
        if (VirtualCamera == null)
        {
            Debug.LogWarning("No networked VirtualCamera found! Using offline VirtualCamera.");
            VirtualCamera = OfflineVirtualCamera;
        }

        m_cinemachinePOV = VirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        if (m_cinemachinePOV != null)
        {
            Debug.Log("CinemachinePOV found!");
            m_cinemachinePOVMaxSpeedHorizontal = m_cinemachinePOV.m_HorizontalAxis.m_MaxSpeed;
            m_cinemachinePOVMaxSpeedVertical = m_cinemachinePOV.m_VerticalAxis.m_MaxSpeed;
            m_framingTransposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        else Debug.LogError("CinemachinePOV not found!");
    }

    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.LeftShift) || (Input.GetKeyUp(KeyCode.LeftShift) && GetIsAnyDirectionPressed()))
        {
            m_isLookAtSetToStop = true;
        }
        else if (GetIsNoDirectionPressed() && Input.GetKey(KeyCode.Space))
        {
            DisableMouseTracking();
            m_isLookAtSetToStop = true;

            if (Input.GetMouseButtonDown(1))
            {
                m_isRotating = true;
                m_previousMousePosition = Input.mousePosition;
            }
        }
        else if (GetIsNoDirectionPressed() && !Input.GetKey(KeyCode.Space))
        {
            EnableMouseTracking();
            m_isLookAtSetToStop = true;
        }
    }

    void FixedUpdate()
    {
        if (m_isRotating)
        {
            FixedUpdateRotatingPlatform();
            return;
        }

        FixedUpdateDirectionalInputs();
    }

    private void LateUpdate()
    {
        LateUpdateCameraScroll();
        LateUpdateFOV();
        LateUpdateStopLookAtRBVelocity();
    }

    private void LateUpdateFOV()
    {
        float distancePercent = m_framingTransposer.m_CameraDistance / m_maxCamDist;

        float newFOV = Mathf.Lerp(m_maxCamFOV, m_minCamFOV, distancePercent * m_FOVmoothDampTime);
        VirtualCamera.m_Lens.FieldOfView = newFOV;
    }

    private void LateUpdateCameraScroll()
    {
        float scrollDelta = Input.mouseScrollDelta.y;

        if (Mathf.Approximately(scrollDelta, 0f)) return;

        float lerpedScrolDist = Mathf.Lerp(m_framingTransposer.m_CameraDistance, m_framingTransposer.m_CameraDistance - (scrollDelta * m_scrollSpeed), m_scrollSmoothDampTime);
        m_framingTransposer.m_CameraDistance = Mathf.Clamp(lerpedScrolDist, m_minCamDist, m_maxCamDist);
    }

    private void FixedUpdateRotatingPlatform()
    {
        if (Input.GetMouseButtonUp(1))
        {
            EnableMouseTracking();
            m_isRotating = false;
            return;
        }

        Vector3 mouseDelta = Input.mousePosition - m_previousMousePosition;

        float angleZ = mouseDelta.x * m_rotationSpeed;
        float angleX = mouseDelta.y * m_rotationSpeed;

        m_currentRotationX += angleX;
        m_currentRotationZ += angleZ;


        m_currentRotationX = Mathf.Clamp(m_currentRotationX, -m_maxRotationAngle, m_maxRotationAngle);
        m_currentRotationZ = Mathf.Clamp(m_currentRotationZ, -m_maxRotationAngle, m_maxRotationAngle);

        // Calculate delta rotation to apply
        float deltaRotationX = m_currentRotationX - m_terrainPlane.transform.rotation.eulerAngles.x;
        float deltaRotationZ = m_currentRotationZ - m_terrainPlane.transform.rotation.eulerAngles.z;

        // Apply rotation
        m_terrainPlane.transform.Rotate(Vector3.right, deltaRotationX, Space.World);
        m_terrainPlane.transform.Rotate(Vector3.forward, deltaRotationZ, Space.World);
    }

    public void FixedUpdateDirectionalInputs()
    {

        Vector3 direction = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            EnableMouseTracking();
            direction += Camera.transform.TransformDirection(1, 0, 0);
            m_isLookAtSetToStop = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            EnableMouseTracking();
            direction += Camera.transform.TransformDirection(0, 0, 1);
            m_isLookAtSetToStop = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            EnableMouseTracking();
            direction += Camera.transform.TransformDirection(-1, 0, 0);
            m_isLookAtSetToStop = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            EnableMouseTracking();
            direction += Camera.transform.TransformDirection(0, 0, -1);
            m_isLookAtSetToStop = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            DisableMouseTracking();
            m_isRotating = true;
            m_previousMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            EnableMouseTracking();
            m_isRotating = false;
        }


        if (direction.magnitude <= 0)
        {
            return;
        }

        if (RB.velocity.magnitude > m_LookAtCurrentMaxVelocity) return;

        RB.AddTorque(GetIsShiftPressedSpeed() * GetCameraDistanceSpeed() * Time.fixedDeltaTime * direction, ForceMode.Force);
    }

    private void DisableMouseTracking()
    {
        if (m_cinemachinePOV == null) Debug.LogError("CinemachinePOV not set correctly!");

        Cursor.visible = true;
        m_cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = 0;
        m_cinemachinePOV.m_VerticalAxis.m_MaxSpeed = 0;
    }

    private void EnableMouseTracking()
    {
        if (m_cinemachinePOV == null) Debug.LogError("CinemachinePOV not set correctly!");

        Cursor.visible = false;
        m_cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = m_cinemachinePOVMaxSpeedHorizontal;
        m_cinemachinePOV.m_VerticalAxis.m_MaxSpeed = m_cinemachinePOVMaxSpeedVertical;
    }

    private float GetIsShiftPressedSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_LookAtCurrentMaxVelocity = m_lookAtMaxVelocity;
            return m_lookAtMaxTorque;
        }

        m_LookAtCurrentMaxVelocity = m_lookAtMinVelocity;
        return m_lookAtMinTorque;
    }

    private float GetCameraDistanceSpeed()
    {
        return m_framingTransposer.m_CameraDistance * m_camDistLookAtSpeedMultiplier;
    }

    private bool GetIsNoDirectionPressed()
    {
        return (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D));
    }

    private bool GetIsAnyDirectionPressed()
    {
        return (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));
    }

    private void LateUpdateStopLookAtRBVelocity()
    {
        if (m_isLookAtSetToStop == false) return;

        RB.velocity = Vector3.Lerp(RB.velocity, Vector3.zero, m_lookAtDecelerationRate);
    }
}