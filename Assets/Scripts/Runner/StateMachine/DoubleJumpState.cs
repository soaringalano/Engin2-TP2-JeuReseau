using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpState : RunnerState
{

    public override bool CanEnter(IState currentState)
    {
        if(currentState.GetType() == typeof(JumpState) &&
            m_stateMachine.m_floorTrigger.IsOnFloor == false &&
            m_stateMachine.m_isJumping == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return true;
            }
        }
        return false;
    }

    public override bool CanExit()
    {
        return m_stateMachine.m_floorTrigger.IsOnFloor;
    }

    public override void OnEnter()
    {
        Debug.Log("Enter state: DoubleJumpState\n");
        m_stateMachine.DoubleJump();
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: DoubleJumpState\n");
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
    }

}
