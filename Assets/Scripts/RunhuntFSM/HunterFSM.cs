using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using System.Drawing;
using Telepathy;

namespace Mirror
{
    public class HunterFSM : AbstractNetworkFSM<HunterState>
    {
        public Camera Camera { get; set; }

        public CinemachineVirtualCamera VirtualCamera { get; set; }

        /* ################# Do not use in code, it replaces the active Camera variables in Start() ############### */
        [field: SerializeField] private Camera OfflineCamera { get; set; }
        [field: SerializeField] private CinemachineVirtualCamera OfflineVirtualCamera { get; set; }
        /* ######################################################################################################## */

        private Rigidbody RB { get; set; }
        private Transform HunterTransform { get; set; }
        //public GameObject MinesPrefab { get; set; }
        public HunterMinePool MinePool { get; private set; }
        private CinemachinePOV CinemachinePOV { get; set; }
        private CinemachineFramingTransposer FramingTransposer { get; set; }
        private float CinemachinePOVMaxSpeedHorizontal { get; set; }
        private float CinemachinePOVMaxSpeedVertical { get; set; }

        [field: SerializeField] public Transform HunterLookAtFloorBody { get; set; }
        public Transform HunterSelectionPose { get; private set; }

        [field: Header("HunterLookAtFloorBody controls Settings")]
        private float FloorBodyMinSpeed { get; set; } = 120.0f;
        private float FloorBodyMaxSpeed { get; set; } = 280.0f;
        private float FloorBodyCurrentMaxSpeed { get; set; } = 80f;
        private bool IsFloorBodySetToStop { get; set; } = false;
        private float FloorBodyDecelerationRate { get; set; } = 0.2f;
        private float CamDistFloorBodySpeedMultiplier { get; set; } = 0.05f;


        [field: Header("Scrolling Settings")]
        [field: SerializeField] private float ScrollSpeed { get; set; } = 25.0f;
        private float MinCamDist { get; set; } = 30.0f;
        private float MaxCamDist { get; set; } = 80.0f;
        private float MinCamFOV { get; set; } = 1.0f;
        private float MaxCamFOV { get; set; } = 90.0f;
        private float ScrollSmoothDampTime { get; set; } = 0.01f;
        private float FOVmoothDampTime { get; set; } = 0.4f;

        [field: Header("Rotating PLatform Settings")]
        public GameObject TerrainPlane { get; set; }
        [field: SerializeField]  private float RotationSpeed { get; set; } = 0.01f;
        private float MaxRotationAngle { get; set; } = 5f;
        public Vector3 PreviousMousePosition { get; set; }
        private Vector3 m_currentRotation = Vector3.zero;
        public bool IsDragging { get; set; } = false;

        [field: Header("Moving Settings")]
        private Vector2 CurrentDirectionalInputs { get; set; } = Vector3.zero;
        public bool IsInitialized { get; set; } = false;

        protected override void CreatePossibleStates()
        {
            m_possibleStates = new List<HunterState>();
            //m_possibleStates.Add(new HunterCharacterSelectionState());
            m_possibleStates.Add(new HunterFreeState());
            m_possibleStates.Add(new PowerUpState());
            m_possibleStates.Add(new PlateformRotationState());
            m_possibleStates.Add(new DragAndDropState());
        }

        public void Initialize()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            MinePool = GetComponent<HunterMinePool>();
            FloorBodyCurrentMaxSpeed = FloorBodyMinSpeed;
            RB = GetComponentInChildren<Rigidbody>();
            if (RB != null) Debug.Log("Hunter RigidBody found!");
            else Debug.LogError("Hunter RigidBody not found!");

            HunterTransform = RB.transform;
            if (HunterTransform != null) Debug.Log("Hunter Transform found!");
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

            CinemachinePOV = VirtualCamera.GetCinemachineComponent<CinemachinePOV>();
            if (CinemachinePOV != null)
            {
                Debug.Log("CinemachinePOV found!");
                CinemachinePOVMaxSpeedHorizontal = CinemachinePOV.m_HorizontalAxis.m_MaxSpeed;
                CinemachinePOVMaxSpeedVertical = CinemachinePOV.m_VerticalAxis.m_MaxSpeed;
                FramingTransposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                FramingTransposer.m_CameraDistance = MaxCamDist;
            }
            else Debug.LogError("CinemachinePOV not found!");

            HunterLookAtFloorBody = transform.GetChild(0);
            if (HunterLookAtFloorBody == null) Debug.LogError("HunterLookAtFloorBody not found! Please check if it still first child in HunterPrefabs.");
            if (HunterLookAtFloorBody.gameObject.name != "HunterLookAtFloorBody") Debug.LogError("The GameObject is not HunterLookAtFloorBody!: " + HunterLookAtFloorBody.gameObject.name);

            HunterSelectionPose = transform.GetChild(1);
            if (HunterSelectionPose == null) Debug.LogError("HunterSelectionPose not found!");
            if (HunterSelectionPose.gameObject.name != "HunterSelectionPose") Debug.LogError("The GameObject is not HunterSelectionPose!: " + HunterSelectionPose.gameObject.name);
            else Debug.Log("HunterSelectionPose found!");

            IsInitialized = true;
        }

        protected override void Start()
        {
            foreach (HunterState state in m_possibleStates)
            {
                state.OnStart(this);
            }

            base.Start();
            m_currentState = m_possibleStates[0];
            m_currentState.OnEnter();
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

        protected override void Update()
        {
            if (!IsInitialized)
            {
                return;
            }

            if (!isLocalPlayer)
            {
                return;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                SetStopHunterLookAtFloorBody(true);
            }
            base.Update();
        }

        protected override void FixedUpdate()
        {
            if (!IsInitialized)
            {
                return;
            }

            if (!isLocalPlayer)
            {
                return;
            }
            SetDirectionalInputs();
            base.FixedUpdate();
        }

        private void LateUpdate()
        {
            if (!IsInitialized)
            {
                return;
            }

            if (!isLocalPlayer)
            {
                return;
            }

            LateUpdateCameraScroll();
            LateUpdateFOV();
            LateUpdateStopHunterLookAtFloorBodyRBVelocity();
        }

        public Vector3 GetCurrentDirectionalInput()
        {
            return CurrentDirectionalInputs;
        }

        public void ApplyMovementsOnFloorFU()
        {
            var vectorOnFloor = Vector3.ProjectOnPlane(Camera.transform.forward * CurrentDirectionalInputs.y, Vector3.up);

            vectorOnFloor += Vector3.ProjectOnPlane(Camera.transform.right * CurrentDirectionalInputs.x, Vector3.up);
            vectorOnFloor.Normalize();

            RB.AddForce(vectorOnFloor * (GetShiftPressedSpeed() * GetCameraDistanceSpeed()), ForceMode.Acceleration);
        }

        public void SetStopHunterLookAtFloorBody(bool stopHunterLookAtFloorBody)
        {
            IsFloorBodySetToStop = stopHunterLookAtFloorBody;
        }

        public void SetLastMousePosition(Vector3 mousePosition)
        {
            PreviousMousePosition = Input.mousePosition;
        }

        private void LateUpdateFOV()
        {
            float distancePercent = FramingTransposer.m_CameraDistance / MaxCamDist;

            float newFOV = Mathf.Lerp(MaxCamFOV, MinCamFOV, distancePercent * FOVmoothDampTime);
            VirtualCamera.m_Lens.FieldOfView = newFOV;
        }

        private void LateUpdateCameraScroll()
        {
            float scrollDelta = Input.mouseScrollDelta.y;

            if (Mathf.Approximately(scrollDelta, 0f)) return;

            float lerpedScrolDist = Mathf.Lerp(FramingTransposer.m_CameraDistance, FramingTransposer.m_CameraDistance - (scrollDelta * ScrollSpeed), ScrollSmoothDampTime);
            FramingTransposer.m_CameraDistance = Mathf.Clamp(lerpedScrolDist, MinCamDist, MaxCamDist);
        }

        public void LateUpdateStopHunterLookAtFloorBodyRBVelocity()
        {
            if (!IsFloorBodySetToStop) return;
            //Debug.Log("Floor Body Set To Stop");
            RB.velocity = Vector3.Lerp(RB.velocity, Vector3.zero, FloorBodyDecelerationRate);
        }

        public void FixedRotatePlatform()
        {

            Vector3 mouseDelta = Input.mousePosition - PreviousMousePosition;

            float angleZ = mouseDelta.x * RotationSpeed;
            float angleX = mouseDelta.y * RotationSpeed;

            if(angleX == 0 && angleZ == 0)
            {
                return;
            }
            if(MathF.Abs(angleX) >= MathF.Abs(angleZ))
            {
                angleZ = 0;
                m_currentRotation.x += angleX;
            }
            else
            {
                angleX = 0;
                m_currentRotation.z += angleZ;
            }


            m_currentRotation.x = Mathf.Clamp(m_currentRotation.x, -MaxRotationAngle, MaxRotationAngle);
            m_currentRotation.z = Mathf.Clamp(m_currentRotation.z, -MaxRotationAngle, MaxRotationAngle);

            // Calculate delta rotation to apply
            float deltaRotationX = m_currentRotation.x - TerrainPlane.transform.rotation.eulerAngles.x;
            float deltaRotationZ = m_currentRotation.z - TerrainPlane.transform.rotation.eulerAngles.z;

            // Apply rotation
            TerrainPlane.transform.Rotate(Vector3.right, deltaRotationX, Space.World);
            TerrainPlane.transform.Rotate(Vector3.forward, deltaRotationZ, Space.World);
        }

        public void DisableMouseTracking()
        {
            if (CinemachinePOV == null) Debug.LogError("CinemachinePOV not set correctly!");

            Cursor.visible = true;
            CinemachinePOV.m_HorizontalAxis.m_MaxSpeed = 0;
            CinemachinePOV.m_VerticalAxis.m_MaxSpeed = 0;
        }

        public void EnableMouseTracking()
        {
            if (CinemachinePOV == null) Debug.LogError("CinemachinePOV not set correctly!");

            Cursor.visible = false;
            CinemachinePOV.m_HorizontalAxis.m_MaxSpeed = CinemachinePOVMaxSpeedHorizontal;
            CinemachinePOV.m_VerticalAxis.m_MaxSpeed = CinemachinePOVMaxSpeedVertical;
        }

        private float GetShiftPressedSpeed()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                FloorBodyCurrentMaxSpeed = FloorBodyMaxSpeed + 40;
                return FloorBodyMaxSpeed;
            }

            FloorBodyCurrentMaxSpeed = FloorBodyMinSpeed;
            return FloorBodyMinSpeed;
        }

        public float GetCameraDistanceSpeed()
        {
            return FramingTransposer.m_CameraDistance * CamDistFloorBodySpeedMultiplier;
        }

        public void SetStopLookAt(bool isSetToStop)
        {
            IsFloorBodySetToStop = isSetToStop;
        }

        //[Command] // This method is called on the servercastlenau
        //public void CmdSpawnCube(Vector3 position)
        //{
        //    MinesPrefab = Instantiate(MinesPrefab, position, Quaternion.identity);
        //    NetworkServer.Spawn(MinesPrefab);
        //}

        [Command]
        public void CmdUpdatePosition(Vector3 newPosition, GameObject mine)
        {
            // Update the position on the server
            mine.transform.position = newPosition;
            RpcUpdatePosition(newPosition, mine);
        }

        [ClientRpc]
        public void RpcUpdatePosition(Vector3 newPosition, GameObject mine)
        {
            // Update the position on all clients
            mine.transform.position = newPosition;
        }

        internal GameObject GetMineFromPoolToPosition(Vector3 point)
        {
            Debug.Log("GetMineFromPoolToPosition");
            // Get a mine from the pool and set its position
            GameObject mine = MinePool.Get(point, Quaternion.identity);
            //if (mine == null) Debug.LogError("Mine not found!");

            mine.SetActive(true);
            mine.transform.position = point;
            NetworkServer.Spawn(mine);

            return mine;
        }
    }
}
//if (MinesPrefab == null)
//    return;

//Vector3 newPosition = point;
//Debug.Log("OnDrag pos: " + point);
//if (isServer)
//{
//    MinesPrefab.transform.position = newPosition;
//    CmdUpdatePosition(newPosition);
//}
//else
//{
//    CmdUpdatePosition(newPosition);
//}