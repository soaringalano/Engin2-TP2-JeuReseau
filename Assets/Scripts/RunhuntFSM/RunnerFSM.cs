using System.Collections.Generic;
using UnityEngine;
using Runhunt.Runner;
using System.Collections;

namespace Mirror
{
    public class RunnerFSM : AbstractNetworkFSM<RunnerState>
    {
        [field: SerializeField] public GameObject RunnerUI { get; private set; }
        [field: SerializeField] public NetworkAnimator NetworkAnimator { get; private set; }
        private Transform StaminaBarSlider { get; set; }
        public Camera Camera { get; set; }
        private Rigidbody RB { get; set; }
        public RunnerFloorTrigger FloorTrigger { get; private set; }
        public Transform Scene { get; private set; }

        private float AccelerationValue { get; set; } = 25f;
        private float MaxForwardVelocity { get; set; } = 6f;
        private float MaxSidewaysVelocity { get; set; } = 4f;
        private float MaxBackwardVelocity { get; set; } = 3f;
        private float JumpIntensity { get; set; } = 400.0f;
        private float MeshRotationLerpSpeed { get; set; } = 4.0f;

        private float MaxStamina { get; set; } = 100;
        private float CurrentStamina { get; set; } = 100;
        private float StaminaRegainSpeed { get; set; } = 10f;
        public float StaminaLoseSpeedInRun { get; private set; } = 10f;
        private float StaminaLoseSpeedInWalk { get; set; } = 0f;
        public float StaminaLoseSpeedInJump { get; private set; } = 10f;
        public float StaminaLoseSpeedInDoubleJump { get; private set; } = 5f;

        public bool m_isJumping { get; private set; } = false;
        public Animator Animator { get; private set; }
        private Vector2 CurrentDirectionalInputs { get; set; }
        private float AnimatorRunningValue { get; set; } = 0.5f; // Has to stay between 0.5 and 1
        private float AccelerationRunningValue { get; set; } = 10.0f;

        public bool m_isInRagdoll = false;

        protected override void CreatePossibleStates()
        {
            m_possibleStates = new List<RunnerState>();
            m_possibleStates.Add(new CharacterSelectionState());
            m_possibleStates.Add(new FreeState());
            m_possibleStates.Add(new JumpState());
            m_possibleStates.Add(new DoubleJumpState());
            m_possibleStates.Add(new RunState());
            m_possibleStates.Add(new RagdollState());
            m_possibleStates.Add(new GettingUpState());
        }

        protected override void Awake()
        {
            //Debug.Log("Runner Awake()");
            //Transform staminaBarTransform = RunnerUI.transform.GetChild(1);
            //if (staminaBarTransform == null) Debug.LogError("Stamina Bar not found!");
            //if (staminaBarTransform.gameObject.name != "StaminaBar") Debug.LogError("The GameObject is not StaminaBar! Name is: " + staminaBarTransform.gameObject.name);

            //StaminaBarSlider = staminaBarTransform.transform.GetChild(1);
            //if (StaminaBarSlider == null) Debug.LogError("Stamina Bar Slider not found!");
            //if (StaminaBarSlider.gameObject.name != "StaminaBar_Slider") Debug.LogError("The GameObject is not StaminaBar! Name is: " + StaminaBarSlider.gameObject.name);

            Animator = GetComponent<Animator>();
            if (Animator == null) Debug.LogError("Runner animator not found in self!");

            RB = GetComponentInChildren<Rigidbody>();
            if (RB == null) Debug.LogError("Runner RigidBody not found in children!");
            if (RB.gameObject.name != "RunnerPrefab") Debug.LogError("The GameObject RigidBody might not be the Runner's RB! Name is: " + RB.gameObject.name);

            FloorTrigger = GetComponentInChildren<RunnerFloorTrigger>();
            if (FloorTrigger == null) Debug.LogError("FloorTrigger not found in children!");

            base.Awake();
        }

        protected override void Start()
        {

            Debug.Log("Runner Start()");
            Transform staminaBarTransform = RunnerUI.transform.GetChild(1);
            if (staminaBarTransform == null) Debug.LogError("Stamina Bar not found!");
            if (staminaBarTransform.gameObject.name != "StaminaBar") Debug.LogError("The GameObject is not StaminaBar! Name is: " + staminaBarTransform.gameObject.name);

            StaminaBarSlider = staminaBarTransform.transform.GetChild(1);
            if (StaminaBarSlider == null) Debug.LogError("Stamina Bar Slider not found!");
            if (StaminaBarSlider.gameObject.name != "StaminaBar_Slider") Debug.LogError("The GameObject is not StaminaBar! Name is: " + StaminaBarSlider.gameObject.name);

            Animator = GetComponent<Animator>();
            if (Animator == null) Debug.LogError("Runner animator not found in self!");

            RB = GetComponentInChildren<Rigidbody>();
            if (RB == null) Debug.LogError("Runner RigidBody not found in children!");
            if (RB.gameObject.name != "RunnerPrefab") Debug.LogError("The GameObject RigidBody might not be the Runner's RB! Name is: " + RB.gameObject.name);

            FloorTrigger = GetComponentInChildren<RunnerFloorTrigger>();
            if (FloorTrigger == null) Debug.LogError("FloorTrigger not found in children!");

            Scene = GetScene().transform;
            if (Scene == null) Debug.LogError("Scene not found!");
     
            foreach (RunnerState state in m_possibleStates)
            {
                state.OnStart(this);
            }
            HunterMineExplosion.OnExplosionEvent += StartRagdoll;
            base.Start();
            m_currentState = m_possibleStates[0];
            m_currentState.OnEnter();
        }

        private void StartRagdoll(HunterMineExplosion explosion)
        {
            Debug.Log("ExplosionSystem here");

            if (explosion != null)
            {
                m_isInRagdoll = true;
                StartCoroutine(ResetBool());
            }          
        }

        IEnumerator ResetBool()
        {
            yield return new WaitForSeconds(1.0f);
            m_isInRagdoll = false;
            Debug.Log("bool reset");
        }

        public GameObject GetScene()
        {
            // Source : https://discussions.unity.com/t/find-gameobjects-in-specific-scene-only/163901
            GameObject[] gameObjects = gameObject.scene.GetRootGameObjects();
            GameObject sceneGO = null;

            foreach (GameObject _gameObject in gameObjects)
            {
                if (_gameObject.name != "Scene") continue;

                sceneGO = _gameObject;
                break;
            }

            return sceneGO;
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
        public void GetUp()
        {
            Animator.SetTrigger("IsGettingUp");
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
            StaminaBarSlider.localScale += new Vector3(rate, 0);
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
            StaminaBarSlider.localScale -= diff;
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