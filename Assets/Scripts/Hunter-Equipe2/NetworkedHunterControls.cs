using Cinemachine;
using Mirror;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkedHunterControls : NetworkBehaviour
{

    public Camera Camera { get; set; }
    public CinemachineVirtualCamera VirtualCamera { get; set; }
    [field:SerializeField]
    private Camera OfflineCamera { get; set; }
    [field: SerializeField]
    private CinemachineVirtualCamera OfflineVirtualCamera { get; set; }
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

    [SerializeField]
    private float m_speed = 100.0f;

    [SerializeField]
    private float m_torque = 50.0f;

    private Rigidbody RB { get; set; }
    private Transform m_hunterTransform;
    private CinemachinePOV m_cinemachinePOV;
    private float m_cinemachinePOVMaxSpeedHorizontal;
    private float m_cinemachinePOVMaxSpeedVertical;


    // Start is called before the first frame update
    void Awake()
    {
        //if (!isLocalPlayer)
        //{
        //    return;
        //}

        if (m_objectToLookAt is null)
        {
            List<GameObject> gos = GameObjectHelper.s_instance.GetGameObjectsByLayerIdAndObjectName(7, "Plane");
            if (gos != null)
            {
                m_objectToLookAt = gos[0].transform;
                return;
            }
            gos = GameObjectHelper.s_instance.GetGameObjectsByLayerIdAndObjectName(7, "Terrain");
            if (gos != null)
            {
                m_objectToLookAt = gos[0].transform;
            }
        }
    }

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
        //FixedUpdateRotate();
        //FixedUpdateCameraLerp();
        //OnFixedUpdateTest();
    }

    /**
     * called in FixedApplyMovements
     */
    public float GetCurrentMaxSpeed()
    {

        if (Mathf.Approximately(CurrentDirectionalInputs.magnitude, 0))
        {
            return MaxForwardVelocity;
        }

        var normalizedInputs = CurrentDirectionalInputs.normalized;

        var currentMaxVelocity = Mathf.Pow(normalizedInputs.x, 2) * MaxSidewaysVelocity;

        if (normalizedInputs.y > 0)
        {
            currentMaxVelocity += Mathf.Pow(normalizedInputs.y, 2) * MaxForwardVelocity;
        }
        else
        {
            currentMaxVelocity += Mathf.Pow(normalizedInputs.y, 2) * MaxBackwardVelocity;
        }

        return currentMaxVelocity;
    }

    /**
     * called in fixed update method
     */
    public void SetDirectionalInputs()
    {
        //CurrentDirectionalInputs = Vector2.zero;

        //if (Input.GetKey(KeyCode.W))
        //{
        //    CurrentDirectionalInputs += Vector2.up;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    CurrentDirectionalInputs += Vector2.down;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    CurrentDirectionalInputs += Vector2.left;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    CurrentDirectionalInputs += Vector2.right;
        //}

        //Debug.Log("Directional input:" + CurrentDirectionalInputs);

        // Source : Maxime Flageole and Alexandre Pipon
        //m_lerpElapsedTime += Time.fixedDeltaTime;

        Vector3 direction = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            //VirtualCamera.gameObject.SetActive(true);
            EnableMouseTracking();
            direction += Camera.transform.TransformDirection(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //VirtualCamera.gameObject.SetActive(true);
            EnableMouseTracking();
            direction += Camera.transform.TransformDirection(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //VirtualCamera.gameObject.SetActive(true);
            EnableMouseTracking();
            direction += Camera.transform.TransformDirection(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //VirtualCamera.gameObject.SetActive(true);
            EnableMouseTracking();
            direction += Camera.transform.TransformDirection(0, 0, -1);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            EnableMouseTracking();
        }
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            //VirtualCamera.gameObject.SetActive(false);
            
            DisableMouseTracking();
            direction = Vector3.zero;
            RB.velocity = Vector3.zero;
            RB.angularVelocity = Vector3.zero;
        }

        if (direction.magnitude <= 0)
        {
            return;
        }

        RB.AddTorque(GetIsShiftPressed() * m_speed * m_torque * Time.fixedDeltaTime * direction, ForceMode.Force);
    }

    /**
     * called in fixed update method
     */
    public void OnFixedUpdateTest()
    {
        if (CurrentDirectionalInputs == Vector2.zero)
        {
            FixedUpdateQuickDeceleration();
            return;
        }

        FixedApplyMovements(CurrentDirectionalInputs);
    }

    /**
     * called in OnFixedUpdateTest
     */
    private void FixedApplyMovements(Vector2 inputVector2)
    {
        var vectorOnFloor = (Camera.transform.up * inputVector2.y + Camera.transform.right * inputVector2.x);
        vectorOnFloor.Normalize();

        RB.AddForce(vectorOnFloor * AccelerationValue, ForceMode.Acceleration);
    }

    /**
     * called in OnFixedUpdateTest
     */
    private void FixedUpdateQuickDeceleration()
    {
        var oppositeDirectionForceToApply = -RB.velocity * DecelerationValue * Time.fixedDeltaTime;
        RB.AddForce(oppositeDirectionForceToApply, ForceMode.Acceleration);
    }

    /**
     * called in fixed update method
     */
    private void FixedUpdateRotate()
    {
        float currentAngleX = -1 * Input.GetAxis("Mouse X") * m_rotationSpeed;
        m_hunterTransform.RotateAround(transform.position, m_objectToLookAt.up, currentAngleX);
    }

    /**
     * called in fixed update method
     */
    private void FixedUpdateCameraLerp()
    {
        m_desiredDistance -= Input.mouseScrollDelta.y;
        m_desiredDistance = Mathf.Clamp(m_desiredDistance, m_zoomClampValues.x, m_zoomClampValues.y);
        var desiredPosition = m_objectToLookAt.transform.position - (transform.forward * m_desiredDistance);
        m_hunterTransform.position = Vector3.Lerp(transform.position, desiredPosition, m_lerpSpeed);
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
            return 10000f;
        }
        return 1f;
    }
}