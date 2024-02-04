using System.Collections.Generic;
using UnityEngine;
using Runhunt.Runner;

namespace Mirror
{
    public class RunnerFSM : AbstractNetworkFSM<RunnerState>
    {
        public Camera Camera { get; set; }
        [field: SerializeField] public Rigidbody RB { get; private set; }
        [field: SerializeField] public RunnerFloorTrigger FloorTrigger { get; private set; }
        [field: SerializeField] public Transform StaminaBarTransform { get; private set; }
        [field: SerializeField] private float AccelerationValue { get; set; }
        [field: SerializeField] private float MaxForwardVelocity { get; set; }
        [field: SerializeField] private float MaxSidewaysVelocity { get; set; }
        [field: SerializeField] private float MaxBackwardVelocity { get; set; }
        [field: SerializeField] private float JumpIntensity { get; set; } = 100.0f;
        [field: SerializeField] private float MeshRotationLerpSpeed { get; set; } = 4.0f;
        [field: SerializeField] public float MaxStamina { get; set; } = 100;
        [field: SerializeField] public float CurrentStamina { get; private set; }
        [field: SerializeField] public float StaminaRegainSpeed { get; private set; } = 10f;
        [field: SerializeField] public float StaminaLoseSpeedInRun { get; private set; } = 10f;
        [field: SerializeField] public float StaminaLoseSpeedInWalk { get; private set; } = 0f;
        [field: SerializeField] public float StaminaLoseSpeedInJump { get; private set; } = 10f;
        [field: SerializeField] public float StaminaLoseSpeedInDoubleJump { get; private set; } = 5f;
        [field: SerializeField] public bool m_isJumping { get; private set; } = false;
        public Animator Animator { get; private set; }
        private Vector2 CurrentDirectionalInputs { get; set; }
        private float AnimatorRunningValue { get; set; } = 0.5f; // Has to stay between 0.5 and 1
        private float AccelerationRunningValue { get; set; } = 10.0f;


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
            Animator = GetComponent<Animator>();
            base.Awake();
        }

        protected override void Start()
        {
            foreach (RunnerState state in m_possibleStates)
            {
                state.OnStart(this);
            }
            m_currentState = m_possibleStates[0];
            m_currentState.OnEnter();
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
            base.FixedUpdate();
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
            Animator.SetFloat("MoveX", CurrentDirectionalInputs.x * AnimatorRunningValue);
            Animator.SetFloat("MoveY", CurrentDirectionalInputs.y * AnimatorRunningValue);
        }

        public void Jump()
        {
            RB.AddForce(Vector3.up * JumpIntensity, ForceMode.Acceleration);
            Animator.SetTrigger("Jump");
            m_isJumping = true;

        }
        public void DoubleJump()
        {
            RB.AddForce(Vector3.up * JumpIntensity, ForceMode.Acceleration);
            Animator.SetTrigger("DoubleJump");
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
                //Debug.Log("Stamina is full or player is in action, cannot regain stamina");
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
            StaminaBarTransform.localScale += new Vector3(rate, 0);
            //Debug.Log("Stamina is regaining by " + val);
        }

        public void FixedLoseStamina(float speed)
        {
            if (CurrentStamina == 0)
            {
                //Debug.Log("Stamina is 0, player must rest to regain stamina");
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
            StaminaBarTransform.localScale -= diff;
            //Debug.Log("Stamina is losing by " + val);
        }

        public bool MustRest(float speed)
        {
            if (CurrentStamina < speed)
            {
                //Debug.Log("Current stamina does not support player's action, player must rest to regain stamina");
                return true;
            }
            return false;
        }
    }
}