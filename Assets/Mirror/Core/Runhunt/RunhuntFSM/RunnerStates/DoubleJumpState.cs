using UnityEngine;

namespace Mirror
{
    public class DoubleJumpState : RunnerState
    {
        public override bool CanEnter(IState currentState)
        {
            // if current stamina does not support this action, then don't allow to enter
            if (m_stateMachine.MustRest(m_stateMachine.StaminaLoseSpeedInDoubleJump))
            {
                return false;
            }
            // if current in JumpState and not on the ground and is_jumping
            if (currentState.GetType() == typeof(JumpState) &&
                m_stateMachine.FloorTrigger.IsOnFloor == false &&
                m_stateMachine.m_isJumping == true)
            {
                // if space bar pressed, then enter
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    return true;
                }
            }
            // otherwise, refuse to enter
            return false;
        }

        public override bool CanExit()
        {
            // if must rest, then allow to exit
            if (m_stateMachine.MustRest(m_stateMachine.StaminaLoseSpeedInDoubleJump))
            {
                return true;
            }
            // if is now on floor
            if (m_stateMachine.FloorTrigger.IsOnFloor)
            {
                return true;
            }
            return false;
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
            m_stateMachine.FixedLoseStamina(m_stateMachine.StaminaLoseSpeedInDoubleJump);
        }
    }
}