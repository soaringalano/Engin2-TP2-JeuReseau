using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

namespace Mirror
{
    public class DragAndDropState : HunterState
    {
        private LayerMask m_raycastLayer;
        private bool IsMineSpawned { get; set; } = false;
        private GameObject CurrentMineGO { get; set; }

        public override bool CanEnter(IState currentState)
        {
            if (currentState is not PowerUpState) return false;

            return m_stateMachine.IsDragging;
        }

        public override bool CanExit()
        {
            return Input.GetMouseButtonUp(0) || !m_stateMachine.IsDragging;
        }

        public override void OnEnter()
        {
            Debug.Log("Enter state: DragAndDropState");
            //m_stateMachine.SetStopLookAt(true);
        }

        public override void OnExit()
        {
            Debug.Log("Exit state: DragAndDropState");
            IsMineSpawned = false;
            //m_stateMachine.SetStopLookAt(false);
            //IsMineSpawned = false;
        }

        public override void OnStart()
        {
            m_raycastLayer = LayerMask.GetMask("RunnerFloor");
            base.OnStart();
        }

        public override void OnUpdate()
        {
            m_stateMachine.DisableMouseTracking();
            base.OnUpdate();

            RayCastMouseOnFloor();
        }

        private void RayCastMouseOnFloor()
        {
            if (!m_stateMachine.IsDragging) return;
            //if (m_isMineSpawned) return;
            Debug.Log("OnUpdate() Is dragging");

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_raycastLayer))
            {
                //Debug.Log("Hit position: " + hit.point);
                //m_stateMachine.MinesPrefab = Object.Instantiate(m_stateMachine.MinesPrefab, hit.point, Quaternion.identity);

                if (!IsMineSpawned)
                {
                    Debug.Log("Mine not spawned yet. Getting from pool.");
                    CurrentMineGO = m_stateMachine.GetMineFromPoolToPosition(hit.point);
                    IsMineSpawned = true;
                    return;
                }
                
                Debug.Log("Mine spawned. Updating position.");
                if (CurrentMineGO == null)
                {
                    Debug.LogError("CurrentMineGO is null.");
                    return;
                }

                m_stateMachine.CmdUpdatePosition(hit.point, CurrentMineGO);
            }
        }

        public override void OnFixedUpdate()
        {

        }
    }
}