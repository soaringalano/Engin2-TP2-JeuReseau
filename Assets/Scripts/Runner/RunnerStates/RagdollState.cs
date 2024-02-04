using UnityEngine;

public class RagdollState : RunnerState
{
    public override void OnEnter()
    {
        Debug.Log("Enter state: RagdollState\n");

        // m_stateMachine.m_animator.enabled = false;
        // m_stateMachine.m_networkAnimator.enabled = false;
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: RagdollState\n");
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
        return m_stateMachine.MineTrigger.IsMineExploded;
        //if (m_stateMachine.MineTrigger.IsMineExploded)
        //{
        //    return true;
        //}
        //return false;
    }

    public override bool CanExit()
    {
        if (m_stateMachine.MineTrigger.IsMineExploded ==  false) 
        {
            return true;
        }
        return false;
    }
}
