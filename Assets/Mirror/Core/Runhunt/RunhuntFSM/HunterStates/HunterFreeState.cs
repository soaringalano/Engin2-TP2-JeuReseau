using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class HunterFreeState : HunterState
    {
        public override bool CanEnter(IState currentState)
        {
            return ((Input.GetMouseButton(1) && Input.GetKey(KeyCode.Space) == false) 
                || (Input.GetKey(KeyCode.Space) == false))
                && m_stateMachine.IsInitialized;
        }

        public override bool CanExit()
        {
            return true;
        }

        public override void OnEnter()
        {
            Debug.Log("Enter state: HunterFreeState.");
            //m_stateMachine.HunterSelectionPose.gameObject.SetActive(false);
            m_stateMachine.SetStopLookAt(false);
        }

        public override void OnExit()
        {
            Debug.Log("Exit state: HunterFreeState.");
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