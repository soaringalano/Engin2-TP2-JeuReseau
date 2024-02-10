using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

namespace Mirror
{
    public class DragAndDropState : HunterState
    {
        private LayerMask m_raycastLayer;
        private bool m_isMineSpawned = false;

        public override bool CanEnter(IState currentState)
        {
            if (currentState is not PowerUpState) return false;

            return m_stateMachine.IsDragging;
        }

        public override bool CanExit()
        {
            return Input.GetMouseButtonUp(0) || m_isMineSpawned;
        }

        public override void OnEnter()
        {
            Debug.Log("Enter state: DragAndDropState");
            //m_stateMachine.SetStopLookAt(true);
        }

        public override void OnExit()
        {
            Debug.Log("Exit state: DragAndDropState");
            //m_stateMachine.SetStopLookAt(false);
            m_isMineSpawned = false;
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

            if (!m_stateMachine.IsDragging) return;
            if (m_isMineSpawned) return;
            Debug.Log("OnUpdate() Is dragging");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_raycastLayer))
            {
                Debug.Log("Hit position: " + hit.point);
                //m_stateMachine.MinesPrefab = Object.Instantiate(m_stateMachine.MinesPrefab, hit.point, Quaternion.identity);
                m_stateMachine.GetMineFromPoolToPosition(hit.point);
                m_isMineSpawned = true;
            }
        }

        public override void OnFixedUpdate()
        {

        }
    }
}