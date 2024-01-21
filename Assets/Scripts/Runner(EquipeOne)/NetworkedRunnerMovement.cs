using Mirror;
using UnityEngine;

public class NetworkedRunnerMovement : NetworkBehaviour
{
    private Camera Camera { get; set; }
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

    [SerializeField]
    private CharacterFloorTrigger m_floorTrigger;

    private Vector2 CurrentDirectionalInputs { get; set; }

    [SerializeField]
    private GameObject m_character;
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private bool m_isJumping = false;

    private void Awake()
    {
        if (isLocalPlayer)
        {
            Camera.gameObject.SetActive(true);
        }
        Camera = Camera.main;
    }

    void Update()
    {
        VerifiIfCanJump(); // a changer de place surment
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
            UpdateMovementsToAnimator();
            ApplyMovementsOnFloorFU(CurrentDirectionalInputs);
        }
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

    private void UpdateMovementsToAnimator()
    {
        // Set the animator values
        m_animator.SetFloat("MoveX", CurrentDirectionalInputs.x);
        m_animator.SetFloat("MoveY", CurrentDirectionalInputs.y);
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

    private void ApplyMovementsOnFloorFU(Vector2 inputVector2)
    {
        var vectorOnFloor = Vector3.ProjectOnPlane(Camera.transform.forward * inputVector2.y, Vector3.up);
        vectorOnFloor += Vector3.ProjectOnPlane(Camera.transform.right * inputVector2.x, Vector3.up);
        vectorOnFloor.Normalize();
        var currentMaxSpeed = GetCurrentMaxSpeed();

        RB.AddForce(vectorOnFloor * AccelerationValue, ForceMode.Acceleration);
        if (RB.velocity.magnitude > currentMaxSpeed)
        {
            RB.velocity = RB.velocity.normalized;
            RB.velocity *= currentMaxSpeed;
        }
    }
}