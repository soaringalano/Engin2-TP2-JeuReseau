using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class HunterFreeState : HunterState
    {
        public override bool CanEnter(IState currentState)
        {
            //Debug.Log("magnitude: " + m_stateMachine.GetCurrentDirectionalInput().magnitude);
            //return m_stateMachine.GetCurrentDirectionalInput().magnitude > 0;
            return (Input.GetMouseButton(1) && Input.GetKey(KeyCode.Space) == false) 
                || (Input.GetKey(KeyCode.Space) == false);
        }

        public override bool CanExit()
        {
            return true;
        }

        public override void OnEnter()
        {
            Debug.Log("Enter state: HunterFreeState\n");
            m_stateMachine.SetStopLookAt(false);
        }

        public override void OnExit()
        {
            Debug.Log("Exit state: HunterFreeState\n");
            m_stateMachine.SetStopLookAt(true);

        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnUpdate()
        {
            m_stateMachine.EnableMouseTracking();

            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) &&
                !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
            {
                //Debug.Log("No key pressed.");
                m_stateMachine.SetStopLookAt(true);
            }

            base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {
            //m_stateMachine.FixedMoveByDirectionalInput();
            m_stateMachine.ApplyMovementsOnFloorFU();
        }
    }
}