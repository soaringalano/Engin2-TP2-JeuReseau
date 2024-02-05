using UnityEngine;

namespace Mirror
{
    public class RagdollState : RunnerState
    {
        public override void OnEnter()
        {
            Debug.Log("Enter state: RagdollState\n");

            m_stateMachine.Animator.enabled = false;
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
            if (m_stateMachine.FloorTrigger.IsOnFloor)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanExit()
        {
            return m_stateMachine.FloorTrigger.IsOnFloor;
        }
    }
}