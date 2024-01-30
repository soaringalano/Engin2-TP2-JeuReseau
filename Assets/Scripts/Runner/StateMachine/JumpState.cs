using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class JumpState : RunnerState
{

    public override void OnEnter()
    {
        Debug.Log("Enter state: JumpState\n");

        m_stateMachine.Jump();
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: JumpState\n");
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {

    }

    public override bool CanEnter(IState currentState)
    {
        //This must be run in Update absolutely
        if (m_stateMachine.m_floorTrigger.IsOnFloor)
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


}
