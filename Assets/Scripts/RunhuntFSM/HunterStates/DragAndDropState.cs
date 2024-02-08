using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

namespace Mirror
{
    public class DragAndDropState : HunterState, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private bool m_isDragging = false;
        public LayerMask m_raycastLayer;

        public override bool CanEnter(IState currentState)
        {
            if (currentState is not PowerUpState) return false;

            return true;
        }

        public override bool CanExit()
        {
            return Input.GetMouseButtonUp(0);
        }

        public override void OnEnter()
        {
            Debug.Log("Enter state: DragAndDropState\n");
            //m_stateMachine.SetStopLookAt(true);
        }

        public override void OnExit()
        {
            //m_stateMachine.SetStopLookAt(false);
        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnUpdate()
        {
            m_stateMachine.DisableMouseTracking();
            base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_isDragging = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!m_isDragging) return;

            Ray ray = m_stateMachine.Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_raycastLayer))
            {
                // Instantiate the cube only on the server
                if (m_stateMachine.isServer)
                {
                    m_stateMachine.MinesPrefab = Object.Instantiate(m_stateMachine.MinesPrefab, hit.point, Quaternion.identity);
                    NetworkServer.Spawn(m_stateMachine.MinesPrefab);
                }
                else
                {
                    CmdSpawnCube(hit.point); // Request the server to spawn the cube
                }
            }
        }

        [Command] // This method is called on the servercastlenau
        void CmdSpawnCube(Vector3 position)
        {
            m_stateMachine.MinesPrefab = Object.Instantiate(m_stateMachine.MinesPrefab, position, Quaternion.identity);
            NetworkServer.Spawn(m_stateMachine.MinesPrefab);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!m_isDragging) return;

            if (m_stateMachine.MinesPrefab == null)
                return;

            Ray ray = m_stateMachine.Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_raycastLayer))
            {
                Debug.Log("OnDrag pos: " + hit.point);
                m_stateMachine.MinesPrefab.transform.position = hit.point;
                CmdUpdatePosition(hit.point);
            }
        }

        [Command]
        void CmdUpdatePosition(Vector3 newPosition)
        {
            // Update the position on the server
            m_stateMachine.MinesPrefab.transform.position = newPosition;
            RpcUpdatePosition(newPosition);
        }

        [ClientRpc]
        void RpcUpdatePosition(Vector3 newPosition)
        {
            // Update the position on all clients
            m_stateMachine.MinesPrefab.transform.position = newPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag");

            if (!m_stateMachine.isLocalPlayer || m_stateMachine.MinesPrefab == null)
                return;

            //Ray ray = TopDownCam.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;

            //if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer))
            //{
            //    instantiatedCube.transform.position = hit.point;

            //}
        }
    }
}