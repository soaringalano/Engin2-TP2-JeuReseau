using UnityEngine;

namespace Mirror
{
    public class PowerUpState : HunterState
    {
        public override bool CanEnter(IState currentState)
        {
            return Input.GetKey(KeyCode.Space) && m_stateMachine.GetCurrentDirectionalInput().magnitude == 0;
        }

        public override bool CanExit()
        {
            return Input.GetKeyUp(KeyCode.Space);
        }

        public override void OnEnter()
        {
            Debug.Log("Enter state: PowerUpState\n");
        }

        public override void OnExit()
        {
            Debug.Log("Exit state: PowerUpState\n");

        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnUpdate()
        {
            m_stateMachine.DisableMouseTracking();
            m_stateMachine.SetStopLookAt(true);

            if (Input.GetMouseButtonDown(1))
            {
                m_stateMachine.SetLastMousePosition(Input.mousePosition);
            }
            base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {

        }
    }
}