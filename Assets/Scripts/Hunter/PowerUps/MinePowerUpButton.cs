using Mirror;
using Runhunt.Hunter;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinePowerUpButton : HunterPowerUpButton, IPointerDownHandler, IPointerUpHandler
{
    override public void Start()
    {
        base.Start();
    }

    override public void Update()
    {
        base.Update();
    }

    override public void OnUseButton()
    {
        Debug.Log("MinePowerUpButton: OnUseButton.");
        base.OnUseButton();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerCurrentRaycast.gameObject;
        if (gameObject == null) return;
        //Debug.Log("GameObject name is: " + gameObject.name);
        base.OnUseButton();
        Debug.Log("MinePowerUpButton: isDragging.");
        m_stateMachine.IsDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("MinePowerUpButton: !isDragging.");
        m_stateMachine.IsDragging = false;
    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    if (!m_stateMachine.IsDragging) return;

    //    Ray ray = m_stateMachine.Camera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_raycastLayer))
    //    {
    //        // Instantiate the cube only on the server
    //        if (m_stateMachine.isServer)
    //        {
    //            m_stateMachine.MinesPrefab = Instantiate(m_stateMachine.MinesPrefab, hit.point, Quaternion.identity);
    //            NetworkServer.Spawn(m_stateMachine.MinesPrefab);
    //        }
    //        else
    //        {
    //            m_stateMachine.CmdSpawnCube(hit.point); // Request the server to spawn the cube
    //        }
    //    }
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    if (!m_stateMachine.IsDragging) return;
    //    //Debug.Log("MinePowerUpButton: isDragging.");

    //    if (m_stateMachine.MinesPrefab == null)
    //        return;

    //    Ray ray = m_stateMachine.Camera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_raycastLayer))
    //    {
    //        Debug.Log("OnDrag pos: " + hit.point);
    //        m_stateMachine.MinesPrefab.transform.position = hit.point;
    //        m_stateMachine.CmdUpdatePosition(hit.point);
    //    }
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    Debug.Log("OnEndDrag");

    //    if (!m_stateMachine.isLocalPlayer || m_stateMachine.MinesPrefab == null)
    //        return;

    //    //Ray ray = TopDownCam.ScreenPointToRay(Input.mousePosition);
    //    //RaycastHit hit;

    //    //if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer))
    //    //{
    //    //    instantiatedCube.transform.position = hit.point;

    //    //}
    //}
}