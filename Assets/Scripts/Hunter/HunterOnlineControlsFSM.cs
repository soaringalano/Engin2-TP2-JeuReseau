using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class HunterOnlineControlsFSM : AbstractNetworkStateMachine<HunterState>
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
    private Vector3 m_previousMousePosition;
    private Vector3 m_currentRotation = Vector3.zero;

    [Header("Moving Settings")]
    private Vector3 m_currentDirectionalInput = Vector3.zero;


    private bool m_isInitialized = false;


    protected override void Start()
    {
        base.Start();
    }

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<HunterState>();
        m_possibleStates.Add(new HunterFreeState());
        m_possibleStates.Add(new PowerUpState());
        m_possibleStates.Add(new PlateformRotationState());
    }

    public void FixedRefreshDirectionalInput()
    {
        Vector3 direction = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            direction += Camera.transform.TransformDirection(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Camera.transform.TransformDirection(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Camera.transform.TransformDirection(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Camera.transform.TransformDirection(0, 0, -1);
        }

        if (direction.magnitude > 0)
        {
            m_currentDirectionalInput = direction;
        }
        else
        {
            m_currentDirectionalInput = Vector3.zero;
        }
    }

    public Vector3 GetCurrentDirectionalInput()
    {
        return m_currentDirectionalInput;
    }

    public void FixedMoveByDirectionalInput()
    {
        EnableMouseTracking();
        SetStopLookAt(false);

        if (RB.velocity.magnitude <= m_LookAtCurrentMaxVelocity)
            RB.AddTorque(GetShiftPressedSpeed() * GetCameraDistanceSpeed() * Time.fixedDeltaTime * m_currentDirectionalInput, ForceMode.Force);
    }

    public void SetCurrentRotation(float currentRotationX, float currentRotationZ)
    {
        m_currentRotation.x = currentRotationX;
        m_currentRotation.z = currentRotationZ;
    }

    public Vector3 GetCurrentRotation()
    {
        return m_currentRotation;
    }

    public void EnterRotation()
    {
        DisableMouseTracking();
        m_previousMousePosition = Input.mousePosition;
    }

    public void ExitRotation()
    {
        EnableMouseTracking();
    }

    public void Initialize()
    {
        if (!isLocalPlayer)
        {
            return;
        }

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

        m_isInitialized = true;
    }

    protected override void Update()
    {
        if (!m_isInitialized)
        {
            return;
        }

        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SetStopLookAt(true);
        }
        base.Update();
    }

    public void SetStopLookAt(bool stopLookAt)
    {
        m_isLookAtSetToStop = stopLookAt;
    }

    public void SetLastMousePosition(Vector3 mousePosition)
    {
        m_previousMousePosition = Input.mousePosition;
    }

    protected override void FixedUpdate()
    {
        if (!m_isInitialized)
        {
            return;
        }

        if (!isLocalPlayer)
        {
            return;
        }
        FixedRefreshDirectionalInput();
        base.FixedUpdate();
    }

    private void LateUpdate()
    {
        if (!m_isInitialized)
        {
            return;
        }

        if (!isLocalPlayer)
        {
            return;
        }

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

    public void LateUpdateStopLookAtRBVelocity()
    {
        if (m_isLookAtSetToStop)
            RB.velocity = Vector3.Lerp(RB.velocity, Vector3.zero, m_lookAtDecelerationRate);
    }

    public void FixedRotatePlatform()
    {

        Vector3 mouseDelta = Input.mousePosition - m_previousMousePosition;

        float angleZ = mouseDelta.x * m_rotationSpeed;
        float angleX = mouseDelta.y * m_rotationSpeed;

        m_currentRotation.x += angleX;
        m_currentRotation.z += angleZ;


        m_currentRotation.x = Mathf.Clamp(m_currentRotation.x, -m_maxRotationAngle, m_maxRotationAngle);
        m_currentRotation.z = Mathf.Clamp(m_currentRotation.z, -m_maxRotationAngle, m_maxRotationAngle);

        // Calculate delta rotation to apply
        float deltaRotationX = m_currentRotation.x - m_terrainPlane.transform.rotation.eulerAngles.x;
        float deltaRotationZ = m_currentRotation.z - m_terrainPlane.transform.rotation.eulerAngles.z;

        // Apply rotation
        m_terrainPlane.transform.Rotate(Vector3.right, deltaRotationX, Space.World);
        m_terrainPlane.transform.Rotate(Vector3.forward, deltaRotationZ, Space.World);
    }

    public void DisableMouseTracking()
    {
        if (m_cinemachinePOV == null) Debug.LogError("CinemachinePOV not set correctly!");

        Cursor.visible = true;
        m_cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = 0;
        m_cinemachinePOV.m_VerticalAxis.m_MaxSpeed = 0;
    }

    public void EnableMouseTracking()
    {
        if (m_cinemachinePOV == null) Debug.LogError("CinemachinePOV not set correctly!");

        Cursor.visible = false;
        m_cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = m_cinemachinePOVMaxSpeedHorizontal;
        m_cinemachinePOV.m_VerticalAxis.m_MaxSpeed = m_cinemachinePOVMaxSpeedVertical;
    }

    private float GetShiftPressedSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_LookAtCurrentMaxVelocity = m_lookAtMaxVelocity;
            return m_lookAtMaxTorque;
        }

        m_LookAtCurrentMaxVelocity = m_lookAtMinVelocity;
        return m_lookAtMinTorque;
    }

    public float GetCameraDistanceSpeed()
    {
        return m_framingTransposer.m_CameraDistance * m_camDistLookAtSpeedMultiplier;
    }

}
