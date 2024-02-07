using UnityEngine;

public class RagdollState : RunnerState
{
    public override void OnEnter()
    {
        Debug.Log("Enter state: RagdollState\n");
        m_stateMachine.m_animator.enabled = false;
        m_stateMachine.m_networkAnimator.enabled = false;
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: RagdollState\n");
        m_stateMachine.m_animator.enabled = true;
        m_stateMachine.m_networkAnimator.enabled = true;
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {

    }

    public override bool CanEnter(IState currentState)
    {
        if (m_stateMachine.test == true && m_stateMachine.m_floorTrigger.ISDetectMine == true)
        {
            return true;
        }
        return false;
    }

    public override bool CanExit()
    {
        if (m_stateMachine.test == false)
        {
            return true;
        }
        return false;
    }
}
