using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerFreeState : RunnerState
{

    public override bool CanEnter(IState currentState)
    {
        return m_stateMachine.m_floorTrigger.IsOnFloor;
    }

    public override bool CanExit()
    {
        return true;
    }

    public override void OnEnter()
    {
        Debug.Log("Enter state: FreeState\n");
        m_stateMachine.SetWalkingInput();
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: FreeState\n");

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
        m_stateMachine.UpdateMovementsToAnimator();
        m_stateMachine.ApplyMovementsOnFloorFU();
        m_stateMachine.FixedRegainStamina();
    }


}
