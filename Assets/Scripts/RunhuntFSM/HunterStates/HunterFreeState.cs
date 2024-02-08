using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class HunterFreeState : HunterState
    {
        public override bool CanEnter(IState currentState)
        {
            return m_stateMachine.GetCurrentDirectionalInput().magnitude > 0;
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
            base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {
            m_stateMachine.FixedMoveByDirectionalInput();
        }
    }
}