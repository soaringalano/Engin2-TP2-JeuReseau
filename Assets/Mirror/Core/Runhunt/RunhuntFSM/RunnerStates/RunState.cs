using UnityEngine;

namespace Mirror
{
    public class RunState : RunnerState
    {

        public override bool CanEnter(IState currentState)
        {
            if (m_stateMachine.MustRest(m_stateMachine.StaminaLoseSpeedInRun))
            {
                return false;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return true;
            }
            return false;
        }

        public override bool CanExit()
        {
            if (m_stateMachine.MustRest(m_stateMachine.StaminaLoseSpeedInRun))
            {
                return true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space))
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
            m_stateMachine.FixedLoseStamina(m_stateMachine.StaminaLoseSpeedInRun);
            m_stateMachine.UpdateMovementsToAnimator();
            m_stateMachine.ApplyMovementsOnFloorFU();

        }

    }
}