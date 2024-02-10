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
            bool isSpaceReleased = Input.GetKeyUp(KeyCode.Space) || !Input.GetKey(KeyCode.Space);
            bool isPivoting = Input.GetMouseButton(1);
            bool isClickAndDragging = Input.GetMouseButton(0) && m_stateMachine.IsDragging;

            //Debug.Log(" ");
            //Debug.Log("Input.GetMouseButton(0): " + Input.GetMouseButton(0));
            //Debug.Log("m_stateMachine.IsDragging" + m_stateMachine.IsDragging);
            //Debug.Log("isClickAndDragging" + isClickAndDragging);
            //Debug.Log(" ");

            return isSpaceReleased
                || isPivoting
                || isClickAndDragging;
        }

        public override void OnEnter()
        {
            Debug.Log("Enter state: PowerUpState\n");
       
            m_stateMachine.SetStopLookAt(true);
        }

        public override void OnExit()
        {
            Debug.Log("Exit state: PowerUpState\n");
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