using Mirror;
using System;
using UnityEngine;

public class NetworkedRunnerMovement : NetworkBehaviour
{
    public Camera Camera { get; private set; }
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
    public float JumpIntensity { get; private set; } = 100.0f;

    [SerializeField]
    private CharacterFloorTrigger m_floorTrigger;

    private Vector2 CurrentRelativeVelocity { get; set; }
    public Vector2 CurrentDirectionalInputs { get; private set; }

    [SerializeField]
    private GameObject m_character;
    [SerializeField]
    private Animator m_animator;
    private bool m_isjump = false;

    private void Start()
    {
        if (isLocalPlayer)
        {
            Camera.gameObject.SetActive(true);
        }
        Camera = Camera.main;
    }

    void Update()
    {
        VerifiIfCanJump();
        if (!isLocalPlayer)
        {
            return;
        } 
    }

    private void FixedUpdate()
    {
        if (m_floorTrigger.IsOnFloor == true)
        {
            SetDirectionalInputs();
            Set2dRelativeVelocity();
            UpdateMovementsToAnimator();
            OnFixedUpdateTest(); // TODO changer le nom / ordre
        }
    }

    private void Set2dRelativeVelocity()
    {
        Vector3 relativeVelocity = RB.transform.InverseTransformDirection(RB.velocity);

        CurrentRelativeVelocity = new Vector2(relativeVelocity.x, relativeVelocity.z);
    }

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
    }

    private void UpdateMovementsToAnimator()
    {
        // Set the animator values
        m_animator.SetFloat("MoveX", CurrentDirectionalInputs.x);
        m_animator.SetFloat("MoveY", CurrentDirectionalInputs.y);
    }

    private void VerifiIfCanJump()
    {
        //Debug.Log("enter jump not jumping");
        if (m_floorTrigger.IsOnFloor == true)
        {
            m_isjump = false;
            //Debug.Log("On floor");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("enter jump press space");
                RB.AddForce(Vector3.up * JumpIntensity, ForceMode.Acceleration);
                m_animator.SetTrigger("Jump");
                m_isjump = true;
            }
        }
    }
    public void OnFixedUpdateTest()
    {
        FixedUpdateRotateWithCamera();

        if (CurrentDirectionalInputs == Vector2.zero)
        {
            FixedUpdateQuickDeceleration();
            return;
        }

        ApplyMovementsOnFloorFU(CurrentDirectionalInputs);
    }

    private void ApplyMovementsOnFloorFU(Vector2 inputVector2)
    {
        var vectorOnFloor = Vector3.ProjectOnPlane(Camera.transform.forward * inputVector2.y, Vector3.up);
        vectorOnFloor += Vector3.ProjectOnPlane(Camera.transform.right * inputVector2.x, Vector3.up);
        vectorOnFloor.Normalize();

        RB.AddForce(vectorOnFloor * AccelerationValue, ForceMode.Acceleration);

        var currentMaxSpeed = GetCurrentMaxSpeed();

        if (RB.velocity.magnitude > currentMaxSpeed)
        {
            RB.velocity = RB.velocity.normalized;
            RB.velocity *= currentMaxSpeed;
        }
    }

    private void FixedUpdateQuickDeceleration()
    {
        var oppositeDirectionForceToApply = -RB.velocity * DecelerationValue * Time.fixedDeltaTime;
        RB.AddForce(oppositeDirectionForceToApply, ForceMode.Acceleration);
    }

    private void FixedUpdateRotateWithCamera()
    {
        var forwardCamOnFloor = Vector3.ProjectOnPlane(Camera.transform.forward, Vector3.up);
        RB.transform.LookAt(forwardCamOnFloor + RB.transform.position);
    }
}
