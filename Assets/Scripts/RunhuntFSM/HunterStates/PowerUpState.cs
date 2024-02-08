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
            return (Input.GetKeyUp(KeyCode.Space) || !Input.GetKey(KeyCode.Space)
                || Input.GetMouseButton(1));
        }

        public override void OnEnter()
        {
            Debug.Log("Enter state: PowerUpState\n");
            m_stateMachine.SetStopLookAt(true);
        }

        public override void OnExit()
        {
            //Debug.Log("Exit state: PowerUpState\n");
            m_stateMachine.SetStopLookAt(false);
        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnUpdate()
        {
            m_stateMachine.DisableMouseTracking();

            base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {

        }
    }
}