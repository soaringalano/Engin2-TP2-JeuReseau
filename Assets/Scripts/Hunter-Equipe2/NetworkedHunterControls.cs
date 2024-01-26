using Cinemachine;
using Mirror;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkedHunterControls : NetworkBehaviour
{

    public Camera Camera { get; set; }
    public CinemachineVirtualCamera VirtualCamera { get; set; }

    /*#### Do not use in code, it replaces the active Camera variables in Start() ###*/ 
    [field:SerializeField]
    private Camera OfflineCamera { get; set; }
    [field: SerializeField]
    private CinemachineVirtualCamera OfflineVirtualCamera { get; set; }
    /* ############################################################################### */

    [field:SerializeField]
    public Transform m_objectToLookAt { get; set; }

    [field:SerializeField]
    private float m_rotationSpeed { get; set; }

    [field: SerializeField]
    private float m_desiredDistance { get; set; } = 10.0f;

    [SerializeField]
    private float m_lerpSpeed = 0.05f;

    [field: SerializeField]
    public float AccelerationValue { get; private set; }
    [field: SerializeField]
    public float DecelerationValue { get; private set; } = 0.3f;
    [field: SerializeField]
    public float MaxForwardVelocity { get; private set; }
    [field: SerializeField]
    public float MaxSidewaysVelocity { get; private set; }
    [field: SerializeField]
    public float MaxBackwardVelocity { get; private set; }

    [field: SerializeField]
    public Vector2 CurrentDirectionalInputs { get; private set; }

    [SerializeField]
    private Vector2 m_zoomClampValues = new Vector2(2.0f, 15.0f);

    [Header("LookAt controls Settings")]
    [SerializeField]
    private float m_lookAtMinMovingSpeed = 20.0f;
    [SerializeField]
    private float m_lookAtMaxMovingSpeed = 100.0f;
    private bool m_isLookAtSetToStop = false;
    public float m_lookAtDecelerationRate = 0.2f;

    private Rigidbody RB { get; set; }
    private Transform m_hunterTransform;
    private CinemachinePOV m_cinemachinePOV;
    private CinemachineFramingTransposer m_framingTransposer;
    private float m_cinemachinePOVMaxSpeedHorizontal;
    private float m_cinemachinePOVMaxSpeedVertical;

    [Header("Scrolling Settings")]
    private float m_scrollSpeed = 200.0f;
    private float m_minCamDist = 15.0f;
    private float m_maxCamDist = 60.0f;
    private float m_minCamFOV = 1.0f;
    private float m_maxCamFOV = 90.0f;
    private float m_scrollSmoothDampTime = 0.01f;
    private float m_FOVmoothDampTime = 0.4f;

    private void Start()
    {
        //if (!isLocalPlayer)
        //{
        //    return;
        //}

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


    void FixedUpdate()
    {
        //if (!isLocalPlayer)
        //{
        //    return;
        //}

        SetDirectionalInputs();
    }

    private void LateUpdate()
    {
        //if (!isLocalPlayer)
        //{
        //    return;
        //}

        UpdateCameraScroll();
        UpdateFOV();
        StopLookAtRBVelocity();
    }

    private void UpdateFOV()
    {
        float distancePercent = m_framingTransposer.m_CameraDistance / m_maxCamDist;

        float newFOV = Mathf.Lerp(m_maxCamFOV, m_minCamFOV, distancePercent * m_FOVmoothDampTime);
        VirtualCamera.m_Lens.FieldOfView = newFOV;
    }

    private void UpdateCameraScroll()
    {
        float scrollDelta = Input.mouseScrollDelta.y;

        if (Mathf.Approximately(scrollDelta, 0f))
        {
            return;
        }

        float lerpedScrolDist = Mathf.Lerp(m_framingTransposer.m_CameraDistance, m_framingTransposer.m_CameraDistance - (scrollDelta * m_scrollSpeed), m_scrollSmoothDampTime);
        m_framingTransposer.m_CameraDistance = Mathf.Clamp(lerpedScrolDist, m_minCamDist, m_maxCamDist);
    }

    public void SetDirectionalInputs()
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
        if (Input.GetKeyUp(KeyCode.LeftShift) || (Input.GetKeyUp(KeyCode.LeftShift) && GetIsAnyDirectionPressed()))
        {
            m_isLookAtSetToStop = true;
        }
        else if (GetIsNoDirectionPressed() && Input.GetKey(KeyCode.Space))
        {
            DisableMouseTracking();
            m_isLookAtSetToStop = true;
        }
        else if (GetIsNoDirectionPressed() && !Input.GetKey(KeyCode.Space))
        {
            EnableMouseTracking();
            direction = Vector3.zero;
            m_isLookAtSetToStop = true;
        }

        if (direction.magnitude <= 0)
        {
            return;
        }

        RB.AddTorque(GetIsShiftPressed() * m_lookAtMinMovingSpeed * Time.fixedDeltaTime * direction, ForceMode.Force);
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

    private float GetIsShiftPressed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return m_lookAtMaxMovingSpeed;
        }

        return m_lookAtMinMovingSpeed;
    }

    private bool GetIsNoDirectionPressed()
    {
        return (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D));
    }

    private bool GetIsAnyDirectionPressed()
    {
        return (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));
    }

    private void StopLookAtRBVelocity()
    {
        if (m_isLookAtSetToStop == false) return;

        RB.velocity = Vector3.Lerp(RB.velocity, Vector3.zero, m_lookAtDecelerationRate);
    }
}