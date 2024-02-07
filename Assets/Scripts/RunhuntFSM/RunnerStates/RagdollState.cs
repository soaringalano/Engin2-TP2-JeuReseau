using UnityEngine;

namespace Mirror
{
    public class RagdollState : RunnerState
    {
        public override void OnEnter()
        {
            Debug.Log("Enter state: RagdollState\n");
            m_stateMachine.Animator.enabled = false;
            m_stateMachine.NetworkAnimator.enabled = false;
        }

        public override void OnExit()
        {
            Debug.Log("Exit state: RagdollState\n");
            m_stateMachine.Animator.enabled = true;
            m_stateMachine.NetworkAnimator.enabled = true;
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {

        }

        public override bool CanEnter(IState currentState)
        {
            if (m_stateMachine.m_isInRagdoll == true && m_stateMachine.FloorTrigger.ISDetectMine == true)
            {
                return true;
            }
            return false;
        }

        public override bool CanExit()
        {
            if (m_stateMachine.m_isInRagdoll == false)
            {
                return true;
            }
            return false;
        }
    }
}