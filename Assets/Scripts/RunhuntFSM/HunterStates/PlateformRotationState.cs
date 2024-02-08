using UnityEngine;

namespace Mirror
{
    public class PlateformRotationState : HunterState
    {
        public override bool CanEnter(IState currentState)
        {
            if (currentState is not PowerUpState) return false;

            //Debug.Log("Can enter state: PlateformRotationState if mouse button 1 pressed: " + Input.GetMouseButton(1));

            return Input.GetMouseButton(1);
        }

        public override bool CanExit()
        {
            return Input.GetMouseButtonUp(1);
        }

        public override void OnEnter()
        {
            //Debug.Log("Enter state: PlateformRotationState\n");
            m_stateMachine.EnterRotation();
        }

        public override void OnExit()
        {
            //Debug.Log("Exit state: PlateformRotationState\n");
            m_stateMachine.ExitRotation();
        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnUpdate()
        {
            m_stateMachine.DisableMouseTracking();

            if (Input.GetMouseButtonDown(1))
            {
                m_stateMachine.SetLastMousePosition(Input.mousePosition);
            }

            base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {
            m_stateMachine.FixedRotatePlatform();
        }
    }
}