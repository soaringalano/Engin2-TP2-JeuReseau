using UnityEngine;

namespace Mirror
{
    public class GettingUpState : RunnerState
    {
        public override void OnEnter()
        {
            Debug.Log("Enter state: GettingUpState\n");

            m_stateMachine.Jump();
        }

        public override void OnExit()
        {
            Debug.Log("Exit state: GettingUpState\n");
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
                if (Input.GetKeyDown(KeyCode.Space))
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