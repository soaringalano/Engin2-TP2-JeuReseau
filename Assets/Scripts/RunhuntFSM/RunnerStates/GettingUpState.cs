using UnityEngine;

namespace Runhunt.FSM
{
    public class GettingUpState : RunnerState
    {
        private const float STATE_EXIT_TIMER = 4.1f;
        private float m_currentStateTimer = 0.0f;
        public override void OnEnter()
        {
            Debug.Log("Enter state: GettingUpState\n");
            m_currentStateTimer = STATE_EXIT_TIMER;
            m_stateMachine.GetUp();
        }

        public override void OnExit()
        {
            Debug.Log("Exit state: GettingUpState\n");
            m_currentStateTimer = 0;
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {
            m_currentStateTimer -= Time.deltaTime;
        }

        public override bool CanEnter(IState currentState)
        {
            //This must be run in Update absolutely
            if (currentState is RagdollState)
            {
                return true;
            }
            return false;
        }

        public override bool CanExit()
        {
            if (m_currentStateTimer <= 0)
            {
                return true;
            }
            return false;
        }
    }
}