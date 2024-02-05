using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformRotationState : HunterState
{

    public override bool CanEnter(IState currentState)
    {
        return Input.GetMouseButton(1) && m_stateMachine.GetCurrentDirectionalInput().magnitude == 0;
    }

    public override bool CanExit()
    {
        return Input.GetMouseButtonUp(1);
    }

    public override void OnEnter()
    {
        Debug.Log("Enter state: FreeState\n");
        m_stateMachine.EnterRotation();
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: FreeState\n");
        m_stateMachine.ExitRotation();
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        m_stateMachine.FixedRotatePlatform();
    }


}
