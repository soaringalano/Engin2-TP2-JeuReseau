using Mirror;
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
        if (IsInCooldown == true)
        {
            return;
        }

        Debug.Log("MinePowerUpButton: OnUseButton.");
        base.OnUseButton();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsInCooldown == true)
        {
            return;
        }

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
}