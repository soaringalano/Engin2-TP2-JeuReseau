using Mirror;
using UnityEngine;

public class RunnerOnlineControls : NetworkBehaviour
{
    public Camera Camera { get; set; }
    [field: SerializeField]
    private Rigidbody RB { get; set; }
    [field: SerializeField]
    private float AccelerationValue { get; set; }
    [field: SerializeField]
    private float MaxForwardVelocity { get; set; }
    [field: SerializeField]
    private float MaxSidewaysVelocity { get; set; }
    [field: SerializeField]
    private float MaxBackwardVelocity { get; set; }
    [field: SerializeField]
    private float JumpIntensity { get; set; } = 100.0f;
    [field: SerializeField]
    private float MeshRotationLerpSpeed { get; set; } = 4.0f;

    [SerializeField]
    private RunnerFloorTrigger m_floorTrigger;

    private Vector2 CurrentDirectionalInputs { get; set; }
    private float AnimatorRunningValue { get; set; } = 0.5f; // Has to stay between 0.5 and 1
    private float AccelerationRunningValue { get; set; } = 10.0f;

    [SerializeField]
    private GameObject m_character;
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private bool m_isJumping = false;


    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        VerifiIfCanJump();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (m_floorTrigger.IsOnFloor == false)
        {
            return;
        }

        SetDirectionalInputs();
        SetRunningInput();
        UpdateMovementsToAnimator();
        RotatePlayerMesh();
        ApplyMovementsOnFloorFU();
    }

    private float GetCurrentMaxSpeed()
    {
        var normalizedInputs = CurrentDirectionalInputs.normalized;
        var currentMaxVelocity = Mathf.Pow(normalizedInputs.x, 2) * MaxSidewaysVelocity;

        if (Mathf.Approximately(CurrentDirectionalInputs.magnitude, 0))
        {
            return MaxForwardVelocity;
        }
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

    private void SetDirectionalInputs()
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

    private void SetRunningInput()
    {
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            AnimatorRunningValue = 1.0f;
            AccelerationRunningValue = 10.0f;
        }
        else
        {
            AnimatorRunningValue = 0.5f;
            AccelerationRunningValue = 1.0f;
        }
    }

    private void UpdateMovementsToAnimator()
    {
        // Set the animator values
        m_animator.SetFloat("MoveX", CurrentDirectionalInputs.x * AnimatorRunningValue);
        m_animator.SetFloat("MoveY", CurrentDirectionalInputs.y * AnimatorRunningValue);
    }

    private void VerifiIfCanJump()
    {
        if (m_floorTrigger.IsOnFloor == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RB.AddForce(Vector3.up * JumpIntensity, ForceMode.Acceleration);
                m_animator.SetTrigger("Jump");
                m_isJumping = true;
            }
        }

        if (m_floorTrigger.IsOnFloor == false && m_isJumping == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RB.AddForce(Vector3.up * JumpIntensity, ForceMode.Acceleration);
                m_animator.SetTrigger("DoubleJump");
                m_isJumping = false;
            }
        }
    }

    private void RotatePlayerMesh()
    {
        if (CurrentDirectionalInputs == Vector2.zero)
        {
            return;
        }

        var vectorOnFloor = Vector3.ProjectOnPlane(Camera.transform.forward * CurrentDirectionalInputs.y, Vector3.up);
        vectorOnFloor += Vector3.ProjectOnPlane(Camera.transform.right * CurrentDirectionalInputs.x, Vector3.up);
        vectorOnFloor.Normalize();
        Quaternion meshRotation = Quaternion.LookRotation(vectorOnFloor, Vector3.up);

        RB.rotation = Quaternion.Slerp(RB.rotation, meshRotation, MeshRotationLerpSpeed * Time.deltaTime);
    }

    private void ApplyMovementsOnFloorFU()
    {
        var vectorOnFloor = Vector3.ProjectOnPlane(Camera.transform.forward * CurrentDirectionalInputs.y, Vector3.up);

        vectorOnFloor += Vector3.ProjectOnPlane(Camera.transform.right * CurrentDirectionalInputs.x, Vector3.up);
        vectorOnFloor.Normalize();
        var currentMaxSpeed = GetCurrentMaxSpeed();
        
        RB.AddForce(vectorOnFloor * (AccelerationValue * AccelerationRunningValue), ForceMode.Acceleration);
        if (RB.velocity.magnitude > currentMaxSpeed)
        {
            RB.velocity = RB.velocity.normalized;
            RB.velocity *= currentMaxSpeed;
        }
    }
}