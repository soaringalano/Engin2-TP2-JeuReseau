using UnityEngine;

namespace Mirror
{
    public class JumpState : RunnerState
    {
        private const float STATE_EXIT_TIMER = 0.5f;
        private float m_currentStateTimer = 0.0f;

        public override void OnEnter()
        {
            //Debug.Log("Enter state: JumpState\n");

            m_currentStateTimer = STATE_EXIT_TIMER;
            m_stateMachine.Jump();
        }

        public override void OnExit()
        {
            //Debug.Log("Exit state: JumpState\n");
            m_currentStateTimer = 0;
            m_stateMachine.Land();
        }

        public override void OnFixedUpdate()
        {
            m_stateMachine.FixedLoseStamina(m_stateMachine.StaminaLoseSpeedInJump);
        }

        public override void OnUpdate()
        {
            m_currentStateTimer -= Time.deltaTime;
        }

        public override bool CanEnter(IState currentState)
        {
            if (currentState is RagdollState)
            {
                return false;
            }
            //This must be run in Update absolutely
            // if must rest, then cannot enter.
            if (m_stateMachine.MustRest(m_stateMachine.StaminaLoseSpeedInJump))
            {
                return false;
            }
            // if is on the ground
            if (m_stateMachine.FloorTrigger.IsOnFloor)
            {
                // if the timer is 0 and space bar pressed, then enter
                if (m_currentStateTimer == 0 && Input.GetKeyDown(KeyCode.Space))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanExit()
        {
            // if not enough stamina for this action, then go to FreeState to rest
            if (m_stateMachine.MustRest(m_stateMachine.StaminaLoseSpeedInJump))
            {
                return true;
            }
            // if is not on the ground
            if (!m_stateMachine.FloorTrigger.IsOnFloor)
            {
                // if is jumping and space bar pressed, then enter DoubleJumpState
                if (m_stateMachine.m_isJumping && Input.GetKeyDown(KeyCode.Space))
                {
                    return true;
                }
            }
            else
            {
                // Otherwise (on the ground), if timer counts down to 0, then can enter other state
                if (m_currentStateTimer <= 0)
                {
                    return true;
                }
            }
            // other case, cannot exit
            return false;
        }
    }
}