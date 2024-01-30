using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : RunnerState
{

    public override bool CanEnter(IState currentState)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return true;
        }
        return false;
    }

    public override bool CanExit()
    {
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            return true;
        }
        return false;
    }

    public override void OnEnter()
    {
        Debug.Log("Enter state: RunState\n");
        m_stateMachine.SetRunningInput();
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: RunState\n");

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
        base.OnFixedUpdate();
        m_stateMachine.UpdateMovementsToAnimator();
        m_stateMachine.ApplyMovementsOnFloorFU();
    }

}
