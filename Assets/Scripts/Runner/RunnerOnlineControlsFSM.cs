using Mirror;
using RPGCharacterAnims.Lookups;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditorInternal;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;
//using UnityEngine.Rendering.Universal.Internal;

public class RunnerControllerStateMachine : AbstractNetworkStateMachine<RunnerState>
{
    public static Action<HunterMineExplosion> OnExplosionEvent;

    public Camera Camera { get; set; }
    [field: SerializeField] public Rigidbody RB { get; private set; }
    [field: SerializeField] private float AccelerationValue { get; set; }
    [field: SerializeField] private float MaxForwardVelocity { get; set; }
    [field: SerializeField] private float MaxSidewaysVelocity { get; set; }
    [field: SerializeField] private float MaxBackwardVelocity { get; set; }
    [field: SerializeField] private float JumpIntensity { get; set; } = 100.0f;
    [field: SerializeField] private float MeshRotationLerpSpeed { get; set; } = 4.0f;
    [field: SerializeField] public RunnerFloorTrigger m_floorTrigger { get; private set; }
    [field: SerializeField] public HunterMineExplosion MineTrigger { get; private set; }
    [field: SerializeField] public NetworkAnimator m_networkAnimator { get; private set; }
    [field: SerializeField] public GameObject StaminaBarSliderPrefab { private get; set; }
    private Transform m_staminaBarTransform;
    [field: SerializeField] public float MaxStamina { get; set; } = 100;
    [field: SerializeField] public float CurrentStamina { get; private set; }
    [field: SerializeField] public float StaminaRegainSpeed = 10f;
    [field: SerializeField] public float StaminaLoseSpeedInRun = 10f;
    [field: SerializeField] public float StaminaLoseSpeedInWalk = 0f;
    [field: SerializeField] public float StaminaLoseSpeedInJump = 10f;
    [field: SerializeField] public float StaminaLoseSpeedInDoubleJump = 5f;
    private Vector3 m_currentStaminaBarScale = Vector3.one;

    private Vector2 CurrentDirectionalInputs { get; set; }
    private float AnimatorRunningValue { get; set; } = 0.5f; // Has to stay between 0.5 and 1
    private float AccelerationRunningValue { get; set; } = 10.0f;

    [SerializeField] private GameObject m_character;
    [SerializeField] public Animator m_animator;
    [SerializeField] public bool test =false;
    [SerializeField] public bool m_isJumping { get; private set; } = false;

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<RunnerState>();
        m_possibleStates.Add(new FreeState());
        m_possibleStates.Add(new JumpState());
        m_possibleStates.Add(new DoubleJumpState());
        m_possibleStates.Add(new RunState());
        m_possibleStates.Add(new RagdollState());
        m_possibleStates.Add(new GettingUpState());
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        HunterMineExplosion.OnExplosionEvent += StartRagdoll;
        m_staminaBarTransform = GameObject.Find("StaminaBar_Slider").transform;
        if (m_staminaBarTransform != null)
        {
            Debug.Log("Stamina Bar Slider found!");
        }
        foreach (RunnerState state in m_possibleStates)
        {
            state.OnStart(this);
        }
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
    }

    private void StartRagdoll(HunterMineExplosion explosion)
    {
        Debug.Log("ExplosionSystem here");

        if (explosion != null)
        {
           test = true;
        }
        StartCoroutine(ResetBool());
    }
    IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(2f);
        test = false;
        Debug.Log("bool reset");
    }
    protected override void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        SetDirectionalInputs();
        RotatePlayerMesh();
        //UpdateMovementsToAnimator();
        //ApplyMovementsOnFloorFU();
        base.FixedUpdate();
        //Set2dRelativeVelocity();
    }

    public float GetCurrentMaxSpeed()
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

    public void SetRunningInput()
    {
        AnimatorRunningValue = 1.0f;
        AccelerationRunningValue = 10.0f;
    }

    public void SetWalkingInput()
    {
        AnimatorRunningValue = 0.5f;
        AccelerationRunningValue = 1.0f;
    }


    public void UpdateMovementsToAnimator()
    {
        // Set the animator values
        m_animator.SetFloat("MoveX", CurrentDirectionalInputs.x * AnimatorRunningValue);
        m_animator.SetFloat("MoveY", CurrentDirectionalInputs.y * AnimatorRunningValue);
    }

    public void Jump()
    {
        RB.AddForce(Vector3.up * JumpIntensity, ForceMode.Acceleration);
        m_animator.SetTrigger("Jump");
        m_isJumping = true;

    }
    public void DoubleJump()
    {
        RB.AddForce(Vector3.up * JumpIntensity, ForceMode.Acceleration);
        m_animator.SetTrigger("DoubleJump");
        m_isJumping = false;
    }

    public void Land()
    {
        m_isJumping = false;
    }

    public void RotatePlayerMesh()
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

    public void ApplyMovementsOnFloorFU()
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

    public void FixedRegainStamina()
    {
        // if current state is FreeState and velocity is > 0, then cannot regain stamina
        if (CurrentStamina == MaxStamina || RB.velocity.magnitude > 0)
        {
            Debug.Log("Stamina is full or player is in action, cannot regain stamina");
            return;
        }
        // value to regain
        float val = StaminaRegainSpeed * Time.fixedDeltaTime;
        CurrentStamina += val;
        //clamp to max value
        if (CurrentStamina > MaxStamina)
        {
            CurrentStamina = MaxStamina;
        }
        float rate = val / MaxStamina;
        Vector3 diff = new Vector3(rate, 0);
        m_staminaBarTransform.localScale += new Vector3(rate, 0);
        Debug.Log("Stamina is regaining by " + val);
    }

    public void FixedLoseStamina(float speed)
    {
        if (CurrentStamina == 0)
        {
            Debug.Log("Stamina is 0, player must rest to regain stamina");
            return;
        }
        float val = speed * Time.fixedDeltaTime;
        CurrentStamina -= val;
        if (CurrentStamina < 0)
        {
            CurrentStamina = 0;
        }
        float rate = val / MaxStamina;
        Vector3 diff = new Vector3(rate, 0);
        m_staminaBarTransform.localScale -= diff;
        //m_staminaBarTransform.localPosition -= diff;
        Debug.Log("Stamina is losing by " + val);
    }

    public bool MustRest(float speed)
    {
        if (CurrentStamina < speed)
        {
            Debug.Log("Current stamina does not support player's action, player must rest to regain stamina");
            return true;
        }
        return false;
    }

    /*    public void UpdateStaminaBarStatus()
        {
            float rate = CurrentStamina / MaxStamina;
            m_currentStaminaBarScale.x = rate;
            m_staminaBarTransform = m_currentStaminaBarScale;
        }*/

}
