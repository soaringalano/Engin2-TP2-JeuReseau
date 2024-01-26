using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class NetworkViewController : NetworkBehaviour
{

    //[field:SerializeField]
    //private Transform m_objectToLookAt { get; set; }

    [field:SerializeField]
    private float m_rotationSpeed { get; set; }

    [field: SerializeField]
    private float m_desiredDistance { get; set; } = 10.0f;

    [SerializeField]
    private float m_lerpSpeed = 0.05f;

    [field: SerializeField]
    public Rigidbody RB { get; private set; }
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

    // Start is called before the first frame update
    void Awake()
    {
        if(isLocalPlayer)
        {
            gameObject.SetActive(true);
        }
        
        //if(m_objectToLookAt == null)
        //{
        //    List<GameObject> gos = GameObjectHelper.GetInstance().GetGameObjectsByLayerIdAndObjectName(7, "Plane");
        //    if (gos != null)
        //    {
        //        m_objectToLookAt = gos[0].transform;
        //        return;
        //    }
        //    gos = GameObjectHelper.GetInstance().GetGameObjectsByLayerIdAndObjectName(7, "Terrain");
        //    if (gos != null)
        //    {
        //        m_objectToLookAt = gos[0].transform;
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        SetDirectionalInputs();
        FixedUpdateRotate();
        FixedUpdateCameraLerp();
        OnFixedUpdateTest();
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

        //Debug.Log("Current max speed :" + currentMaxVelocity);

        return currentMaxVelocity;
    }

    /**
     * called in fixed update method
     */
    public void SetDirectionalInputs()
    {
        CurrentDirectionalInputs = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            CurrentDirectionalInputs += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            CurrentDirectionalInputs += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            CurrentDirectionalInputs += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            CurrentDirectionalInputs += Vector2.right;
        }

        //Debug.Log("Directional input:" + CurrentDirectionalInputs);
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

        FixedApplyMovements(CurrentDirectionalInputs, gameObject.GetComponent<Camera>());
    }

    /**
     * called in OnFixedUpdateTest
     */
    private void FixedApplyMovements(Vector2 inputVector2, Camera camera)
    {
        /*var vectorOnFloor = Vector3.ProjectOnPlane(-camera.transform.up * inputVector2.y, Vector3.up);
        vectorOnFloor += Vector3.ProjectOnPlane(-camera.transform.right * inputVector2.x, Vector3.up);*/
        //Debug.Log("god view camera is moving");
        //Debug.Log("input vector is :" + inputVector2);
        var vectorOnFloor = (camera.transform.up * inputVector2.y + camera.transform.right * inputVector2.x);
        vectorOnFloor.Normalize();
        //Debug.Log("Normalized input is : " + vectorOnFloor);

        RB.AddForce(vectorOnFloor * AccelerationValue, ForceMode.Acceleration);

        //var currentMaxSpeed = GetCurrentMaxSpeed();
        // Debug.Log("current max speed is :" + currentMaxSpeed);

        //if (RB.velocity.magnitude > currentMaxSpeed)
        //{
            //RB.velocity = RB.velocity.normalized * currentMaxSpeed;
            //RB.velocity *= currentMaxSpeed;
            //Debug.Log("RB Velocity:" + RB.velocity);
        //}
    }

    /**
     * called in OnFixedUpdateTest
     */
    private void FixedUpdateQuickDeceleration()
    {
        //Debug.Log("god view camera is decelerating quickly");
        var oppositeDirectionForceToApply = -RB.velocity * DecelerationValue * Time.fixedDeltaTime;
        RB.AddForce(oppositeDirectionForceToApply, ForceMode.Acceleration);
    }

    /**
     * called in fixed update method
     */
    private void FixedUpdateRotate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            float currentAngleX = -1 * Input.GetAxis("Mouse X") * m_rotationSpeed;
           // transform.RotateAround(transform.position, m_objectToLookAt.up, currentAngleX);
        }
    }

    /**
     * called in fixed update method
     */
    private void FixedUpdateCameraLerp()
    {
        m_desiredDistance -= Input.mouseScrollDelta.y;
        m_desiredDistance = Mathf.Clamp(m_desiredDistance, m_zoomClampValues.x, m_zoomClampValues.y);
        //var desiredPosition = m_objectToLookAt.transform.position - (transform.forward * m_desiredDistance);
        //transform.position = Vector3.Lerp(transform.position, desiredPosition, m_lerpSpeed);
    }

}
